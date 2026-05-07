using System.Diagnostics;
using Application.Interfaces;
using Application.DTOs;

namespace Application.Services;

public class CodeExecutionService : ICodeExecutionService
{
    public async Task<string> ExecuteAsync(RunProjectDto dto)
    {
        var folder = Path.Combine(Path.GetTempPath(), "unprompted", Guid.NewGuid().ToString());
        Directory.CreateDirectory(folder);

        try
        {
            // 🔥 écrire tous les fichiers
            foreach (var file in dto.Files)
            {
                var filePath = Path.Combine(folder, file.Name);

                var dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir!);

                await File.WriteAllTextAsync(filePath, file.Content);
            }

            string command = GetDockerCommand(dto.Language, folder);

            if (string.IsNullOrEmpty(command))
                return "Langage non supporté";

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

            process.WaitForExit(5000); // timeout

            return string.IsNullOrEmpty(error) ? output : error;
        }
        finally
        {
            // nettoyage
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);
        }
    }

    private string GetDockerCommand(string language, string folder)
    {
        var path = Path.GetFullPath(folder);

        return language.ToLower() switch
        {
            "python" => $"docker run --rm --memory=100m --cpus=0.5 --network none -v {path}:/app python:3.9 bash -c \"cd /app && python main.py\"",

            "javascript" => $"docker run --rm --memory=100m --cpus=0.5 --network none -v {path}:/app node:18 bash -c \"cd /app && node main.js\"",

            "csharp" => $"docker run --rm --memory=200m --cpus=1 -v {path}:/app mcr.microsoft.com/dotnet/sdk:7.0 bash -c \"cd /app && dotnet new console -n app && cp *.cs app/ && cd app && dotnet run\"",

            "java" => $"docker run --rm --memory=100m --cpus=0.5 --network none -v {path}:/app openjdk:17 bash -c \"cd /app && javac *.java && java Main\"",

            "cpp" => $"docker run --rm --memory=100m --cpus=0.5 --network none -v {path}:/app gcc:latest bash -c \"cd /app && g++ *.cpp -o main && ./main\"",

            _ => ""
        };
    }
}