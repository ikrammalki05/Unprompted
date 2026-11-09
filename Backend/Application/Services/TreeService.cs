using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class TreeService : ITreeService
{
    private readonly IDossierRepository _dossierRepo;
    private readonly IFichierRepository _fichierRepo;
    private readonly IFichierService _fichierService;

    public TreeService(
        IDossierRepository dossierRepo,
        IFichierRepository fichierRepo,
        IFichierService fichierService)
    {
        _dossierRepo = dossierRepo;
        _fichierRepo = fichierRepo;
        _fichierService = fichierService;
    }

    public async Task<List<TreeNodeDto>> GetProjectTreeAsync(int projectId)
    {
        var dossiers = (await _dossierRepo.GetByProjectIdAsync(projectId)).ToList();
        var fichiers = (await _fichierRepo.GetByProjectIdAsync(projectId)).ToList();

        if (dossiers.Count == 0 && fichiers.Count == 0)
        {
            await SeedDefaultWorkspaceAsync(projectId);
            dossiers = (await _dossierRepo.GetByProjectIdAsync(projectId)).ToList();
            fichiers = (await _fichierRepo.GetByProjectIdAsync(projectId)).ToList();
        }

        return BuildTree(null, dossiers, fichiers);
    }

    private async Task SeedDefaultWorkspaceAsync(int projectId)
    {
        var src = new Dossier
        {
            Nom = "src",
            IdProjet = projectId,
            DossierParentId = null
        };
        await _dossierRepo.AddAsync(src);

        await _fichierService.CreateAsync(new FichierCreateDto
        {
            Nom = "main",
            Extension = ".py",
            Contenu = "print('Hello Unprompted')\n",
            IdProjet = projectId,
            IdDossier = src.IdDossier
        }, "system");

        await _fichierService.CreateAsync(new FichierCreateDto
        {
            Nom = "README",
            Extension = ".md",
            Contenu = "# Projet Unprompted\n\nÉditez vos fichiers ici.\n",
            IdProjet = projectId,
            IdDossier = null
        }, "system");
    }

    private List<TreeNodeDto> BuildTree(int? parentId, List<Dossier> dossiers, List<Fichier> fichiers)
    {
        var nodes = new List<TreeNodeDto>();

        foreach (var folder in dossiers.Where(d => d.DossierParentId == parentId))
        {
            nodes.Add(new TreeNodeDto
            {
                Id = folder.IdDossier,
                Name = folder.Nom,
                Type = "folder",
                Children = BuildTree(folder.IdDossier, dossiers, fichiers)
            });
        }

        foreach (var file in fichiers.Where(f => f.IdDossier == parentId))
        {
            nodes.Add(new TreeNodeDto
            {
                Id = file.IdFichier,
                Name = $"{file.Nom}{file.Extension}",
                Type = "file"
            });
        }

        return nodes;
    }
}
