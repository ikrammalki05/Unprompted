using System.Diagnostics;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class TerminalService : ITerminalService
{
    private readonly ISessionTerminalRepository
        _repository;

    public TerminalService(
        ISessionTerminalRepository repository)
    {
        _repository = repository;
    }

    public async Task<SessionTerminalDto>
        CreerSessionAsync(string idUtilisateur)
    {
        var containerId =
            await CreerConteneurDocker();

        var session = new SessionTerminal
        {
            IdSession = Guid.NewGuid(),
            IdUtilisateur = idUtilisateur,
            IdConteneurDocker = containerId,
            Active = true
        };

        await _repository.AjouterAsync(session);

        return new SessionTerminalDto
        {
            IdSession = session.IdSession
        };
    }

    public async Task<ResultatTerminalDto>
        ExecuterCommandeAsync(
            Guid idSession,
            string commande)
    {
        var session =
            await _repository.ObtenirParIdAsync(
                idSession);

        if (session == null || !session.Active)
            throw new Exception(
                "Session introuvable");

        VerifierCommande(commande);

        var process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = "docker",

            Arguments =
                $"exec {session.IdConteneurDocker} bash -c \"{commande}\"",

            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        process.Start();

        string sortie =
            await process.StandardOutput
                .ReadToEndAsync();

        string erreur =
            await process.StandardError
                .ReadToEndAsync();

        process.WaitForExit();

        return new ResultatTerminalDto
        {
            Sortie = string.IsNullOrWhiteSpace(erreur)
                ? sortie
                : erreur
        };
    }

    public async Task FermerSessionAsync(
        Guid idSession)
    {
        var session =
            await _repository.ObtenirParIdAsync(
                idSession);

        if (session == null)
            return;

        var process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = "docker",

            Arguments =
                $"stop {session.IdConteneurDocker}",

            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        process.Start();

        process.WaitForExit();

        session.Active = false;

        await _repository.MettreAJourAsync(
            session);
    }

    // Docker
    private async Task<string>
        CreerConteneurDocker()
    {
        var process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = "docker",

            Arguments =
                "run -dit --rm python:3.9 bash",

            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        process.Start();

        string containerId =
            await process.StandardOutput
                .ReadToEndAsync();

        process.WaitForExit();

        return containerId.Trim();
    }

    // Sécurité
    private void VerifierCommande(
        string commande)
    {
        var interdites = new[]
        {
            "shutdown",
            "rm -rf",
            "wget",
            "curl",
            "powershell"
        };

        if (interdites.Any(x =>
            commande.ToLower().Contains(x)))
        {
            throw new Exception(
                "Commande interdite");
        }
    }
}