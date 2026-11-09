using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class TerminalService : ITerminalService
{
    private static readonly ConcurrentDictionary<Guid, string> WorkingDirs = new();
    private readonly ISessionTerminalRepository _repository;
    private readonly IFichierRepository _fichierRepository;
    private readonly IDossierRepository _dossierRepository;

    public TerminalService(
        ISessionTerminalRepository repository,
        IFichierRepository fichierRepository,
        IDossierRepository dossierRepository)
    {
        _repository = repository;
        _fichierRepository = fichierRepository;
        _dossierRepository = dossierRepository;
    }

    public async Task<SessionTerminalDto> CreerSessionAsync(string idUtilisateur, int? idProjet = null)
    {
        var workDir = Path.Combine(
            Path.GetTempPath(),
            "unprompted-term",
            idProjet.HasValue ? $"p{idProjet.Value}" : "anon",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workDir);

        if (idProjet.HasValue)
            await MaterialiserProjetAsync(idProjet.Value, workDir);

        EnsureGitRepository(workDir);

        var session = new SessionTerminal
        {
            IdSession = Guid.NewGuid(),
            IdUtilisateur = idUtilisateur,
            IdConteneurDocker = workDir,
            Active = true
        };

        await _repository.AjouterAsync(session);
        WorkingDirs[session.IdSession] = workDir;

        return new SessionTerminalDto { IdSession = session.IdSession };
    }

    public async Task<ResultatTerminalDto> ExecuterCommandeAsync(Guid idSession, string commande)
    {
        var session = await _repository.ObtenirParIdAsync(idSession);
        if (session == null || !session.Active)
            throw new Exception("Session introuvable");

        VerifierCommande(commande);

        var workDir = WorkingDirs.GetOrAdd(idSession, session.IdConteneurDocker);
        if (!Directory.Exists(workDir))
            Directory.CreateDirectory(workDir);

        if (commande.TrimStart().StartsWith("git ", StringComparison.OrdinalIgnoreCase)
            || commande.Trim().Equals("git", StringComparison.OrdinalIgnoreCase))
        {
            EnsureGitRepository(workDir);
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {commande}",
                WorkingDirectory = workDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            }
        };

        process.Start();
        var sortie = await process.StandardOutput.ReadToEndAsync();
        var erreur = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        return new ResultatTerminalDto
        {
            Sortie = string.IsNullOrWhiteSpace(erreur)
                ? sortie
                : string.IsNullOrWhiteSpace(sortie) ? erreur : $"{sortie}\n{erreur}"
        };
    }

    public async Task FermerSessionAsync(Guid idSession)
    {
        var session = await _repository.ObtenirParIdAsync(idSession);
        if (session == null) return;

        session.Active = false;
        await _repository.MettreAJourAsync(session);
        WorkingDirs.TryRemove(idSession, out _);
    }

    private async Task MaterialiserProjetAsync(int idProjet, string workDir)
    {
        var dossiers = (await _dossierRepository.GetByProjectIdAsync(idProjet)).ToList();
        var fichiers = (await _fichierRepository.GetByProjectIdAsync(idProjet)).ToList();

        var dossierPaths = new Dictionary<int, string>();
        foreach (var d in dossiers.OrderBy(x => x.DossierParentId.HasValue ? 1 : 0).ThenBy(x => x.IdDossier))
        {
            var parentPath = d.DossierParentId.HasValue && dossierPaths.TryGetValue(d.DossierParentId.Value, out var p)
                ? p
                : workDir;
            var path = Path.Combine(parentPath, SanitizeName(d.Nom));
            Directory.CreateDirectory(path);
            dossierPaths[d.IdDossier] = path;
        }

        foreach (var f in fichiers)
        {
            var dir = f.IdDossier.HasValue && dossierPaths.TryGetValue(f.IdDossier.Value, out var dp)
                ? dp
                : workDir;
            var fileName = SanitizeName((f.Nom ?? "fichier") + (f.Extension ?? ""));
            var fullPath = Path.Combine(dir, fileName);
            await File.WriteAllTextAsync(fullPath, f.Contenu ?? string.Empty, Encoding.UTF8);
        }
    }

    private static void EnsureGitRepository(string workDir)
    {
        var gitDir = Path.Combine(workDir, ".git");
        if (Directory.Exists(gitDir)) return;

        RunSilent(workDir, "git init");
        RunSilent(workDir, "git config user.email \"etudiant@unprompted.local\"");
        RunSilent(workDir, "git config user.name \"Etudiant Unprompted\"");
    }

    private static void RunSilent(string workDir, string command)
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                WorkingDirectory = workDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            process?.WaitForExit(8000);
        }
        catch
        {
            /* git may be unavailable — command will surface the error */
        }
    }

    private static string SanitizeName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return string.IsNullOrWhiteSpace(name) ? "fichier" : name;
    }

    private static void VerifierCommande(string commande)
    {
        if (string.IsNullOrWhiteSpace(commande))
            throw new Exception("Commande vide");

        var blocked = new[] { "format ", "del /s", "rm -rf /", "shutdown", "reg delete", "net user" };
        var lower = commande.ToLowerInvariant();
        if (blocked.Any(b => lower.Contains(b)))
            throw new Exception("Commande non autorisée");
    }
}
