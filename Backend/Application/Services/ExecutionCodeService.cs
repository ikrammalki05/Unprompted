using System.Diagnostics;
using System.Text;
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
        var folder = Path.Combine(Path.GetTempPath(), "unprompted-exec", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);

        foreach (var file in dto.Fichiers)
        {
            var relative = file.Nom.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            var path = Path.Combine(folder, relative);
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            await File.WriteAllTextAsync(path, file.Contenu ?? string.Empty);
        }

        var (fileName, args) = BuildLocalCommand(dto.Langage, folder, dto.Fichiers.Select(f => f.Nom).ToList());
        var output = new StringBuilder();
        var statut = "completed";

        try
        {
            if (string.IsNullOrEmpty(fileName))
            {
                output.AppendLine($"Langage non supporté: {dto.Langage}");
                statut = "failed";
            }
            else
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = args,
                        WorkingDirectory = folder,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var stdOut = await process.StandardOutput.ReadToEndAsync();
                var stdErr = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (!string.IsNullOrWhiteSpace(stdOut)) output.AppendLine(stdOut.TrimEnd());
                if (!string.IsNullOrWhiteSpace(stdErr)) output.AppendLine(stdErr.TrimEnd());
                if (process.ExitCode != 0)
                {
                    statut = "failed";
                    output.AppendLine($"Exit code: {process.ExitCode}");
                }
            }
        }
        catch (Exception ex)
        {
            statut = "failed";
            output.AppendLine($"Erreur d'exécution: {ex.Message}");
        }

        var execution = new ExecutionCode
        {
            IdExecution = Guid.NewGuid(),
            Langage = dto.Langage,
            IdConteneurDocker = folder,
            Statut = statut,
            Sortie = output.ToString(),
            IdUtilisateur = userId,
            DateFin = DateTime.UtcNow
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
        if (execution == null) return null;

        return new StatutExecutionDto
        {
            IdExecution = execution.IdExecution,
            Statut = execution.Statut,
            Sortie = execution.Sortie
        };
    }

    public async Task ArreterExecutionAsync(Guid executionId)
    {
        var execution = await _repo.GetByIdAsync(executionId);
        if (execution == null)
            throw new Exception("Execution introuvable");

        execution.Statut = "stopped";
        execution.DateFin = DateTime.UtcNow;
        await _repo.UpdateAsync(execution);
    }

    private static (string fileName, string args) BuildLocalCommand(string language, string folder, List<string> files)
    {
        var lang = (language ?? "").Trim().ToLowerInvariant();
        var mainPy = files.FirstOrDefault(f => f.EndsWith("main.py", StringComparison.OrdinalIgnoreCase))
            ?? files.FirstOrDefault(f => f.EndsWith(".py", StringComparison.OrdinalIgnoreCase));
        var mainJs = files.FirstOrDefault(f => f.EndsWith("main.js", StringComparison.OrdinalIgnoreCase))
            ?? files.FirstOrDefault(f => f.EndsWith(".js", StringComparison.OrdinalIgnoreCase));

        return lang switch
        {
            "python" or "py" => ("python", Quote(Path.Combine(folder, mainPy ?? "main.py"))),
            "javascript" or "js" or "node" => ("node", Quote(Path.Combine(folder, mainJs ?? "main.js"))),
            "csharp" or "cs" => ("dotnet", $"script {Quote(Path.Combine(folder, files.FirstOrDefault(f => f.EndsWith(".cs")) ?? "Program.cs"))}"),
            _ => ("", "")
        };
    }

    private static string Quote(string path) => $"\"{path}\"";
}
