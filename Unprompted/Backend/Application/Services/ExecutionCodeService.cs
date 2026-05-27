using System.Diagnostics;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ExecutionCodeService : IExecutionCodeService
{
    private readonly IExecutionCodeRepository _repo;

    public ExecutionCodeService(IExecutionCodeRepository repo)
    {
        _repo = repo;
    }

    public async Task<ExecutionCodeDto> DemarrerExecutionAsync(ExecutionProjetDto dto, string userId)
    {
        var folder = Path.Combine("temp", Guid.NewGuid().ToString());
        Directory.CreateDirectory(folder);

        foreach (var file in dto.Fichiers)
        {
            var path = Path.Combine(folder, file.Nom);
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);
            await File.WriteAllTextAsync(path, file.Contenu);
        }

        string command = GetDockerCommand(dto.Langage, folder);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true, // ✅ ajouter
                UseShellExecute = false
            }
        };

        process.Start();

        string containerId = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit(); // ✅ attendre

        Console.WriteLine($"[DEBUG] containerId: '{containerId.Trim()}'");
        Console.WriteLine($"[DEBUG] error: '{error}'");

        if (string.IsNullOrWhiteSpace(containerId))
            throw new Exception($"Docker n'a pas retourné d'ID: {error}");

        var execution = new ExecutionCode
        {
            IdExecution = Guid.NewGuid(),
            Langage = dto.Langage,
            IdConteneurDocker = containerId.Trim(),
            Statut = "running",
            IdUtilisateur = userId
        };

        await _repo.AddAsync(execution);

        return new ExecutionCodeDto
        {
            IdExecution = execution.IdExecution,
            Statut = execution.Statut
        };
    }

    public async Task<StatutExecutionDto?> GetExecutionAsync(Guid executionId)
    {
        var execution = await _repo.GetByIdAsync(executionId);

        if (execution == null)
            return null;

        var dockerlogs = await GetDockerLogs(execution.IdConteneurDocker);

        execution.Sortie = dockerlogs;

        // vérifier si terminé
        bool running = await IsContainerRunning(execution.IdConteneurDocker);

        if (!running)
        {
            var logs = await GetDockerLogs(execution.IdConteneurDocker); // ✅ logs avant rm
            execution.Sortie = logs;
            execution.Statut = "completed";
            execution.DateFin = DateTime.UtcNow;

            await RunCommand($"docker rm {execution.IdConteneurDocker}"); // rm après
            await _repo.UpdateAsync(execution);
        }

        // Retourner les logs depuis la DB, pas depuis docker
        return new StatutExecutionDto
        {
            IdExecution = execution.IdExecution,
            Statut = execution.Statut,
            Sortie = execution.Sortie // ✅ vient de la DB, pas du conteneur détruit
        };
    }

    public async Task ArreterExecutionAsync(Guid executionId)
    {
        var execution = await _repo.GetByIdAsync(executionId);

        if (execution == null)
            throw new Exception("Execution introuvable");

        await RunCommand($"docker stop {execution.IdConteneurDocker}");
        await RunCommand($"docker rm {execution.IdConteneurDocker}");

        execution.Statut = "stopped";
        execution.DateFin = DateTime.UtcNow;

        await _repo.UpdateAsync(execution);
    }

    private string GetDockerCommand(string language, string folder)
    {
        var path = Path.GetFullPath(folder);

        return language.ToLower() switch
        {   
            "python" => $"docker run -d --memory=100m --cpus=0.5 --network none -v \"{path}\":/app python:3.9 bash -c \"cd /app && python main.py\"",

            "javascript" => $"docker run -d --memory=100m --cpus=0.5 --network none -v \"{path}\":/app node:18 bash -c \"cd /app && node main.js\"",

            "csharp" => $"docker run -d --memory=200m --cpus=1 -v \"{path}\":/app mcr.microsoft.com/dotnet/sdk:7.0 bash -c \"cd /app && dotnet new console -n app && cp *.cs app/ && cd app && dotnet run\"",

            "java" => $"docker run -d --memory=100m --cpus=0.5 --network none -v \"{path}\":/app openjdk:17 bash -c \"cd /app && javac *.java && java Main\"",

            "cpp" => $"docker run -d --memory=100m --cpus=0.5 --network none -v \"{path}\":/app gcc:latest bash -c \"cd /app && g++ *.cpp -o main && ./main\"",

            _ => ""
        };
    }

    private async Task<string> GetDockerLogs(string containerId)
    {
        return await RunCommand($"docker logs {containerId}");
    }

    private async Task<bool> IsContainerRunning(string containerId)
    {
        var result = await RunCommand(
            $"docker ps -q -f id={containerId}");

        return !string.IsNullOrWhiteSpace(result);
    }

    private async Task<string> RunCommand(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        // retourner les deux (docker logs écrit parfois sur stderr)
        return output + error;
    }
}