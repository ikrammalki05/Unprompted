// Application/Services/TreeService.cs

using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

public class TreeService : ITreeService
{
    private readonly IDossierRepository _dossierRepo;
    private readonly IFichierRepository _fichierRepo;

    public TreeService(IDossierRepository dossierRepo, IFichierRepository fichierRepo)
    {
        _dossierRepo = dossierRepo;
        _fichierRepo = fichierRepo;
    }

    public async Task<List<TreeNodeDto>> GetProjectTreeAsync(int projectId)
    {
        var dossiers = (await _dossierRepo.GetByProjectIdAsync(projectId)).ToList();
        var fichiers = (await _fichierRepo.GetByProjectIdAsync(projectId)).ToList();

        return BuildTree(null, dossiers, fichiers);
    }

    private List<TreeNodeDto> BuildTree(int? parentId, List<Dossier> dossiers, List<Fichier> fichiers)
    {
        var nodes = new List<TreeNodeDto>();

        // dossiers
        var subFolders = dossiers
            .Where(d => d.DossierParentId == parentId)
            .ToList();

        foreach (var folder in subFolders)
        {
            var node = new TreeNodeDto
            {
                Id = folder.IdDossier,
                Name = folder.Nom,
                Type = "folder",
                Children = BuildTree(folder.IdDossier, dossiers, fichiers)
            };

            nodes.Add(node);
        }

        // fichiers
        var files = fichiers
            .Where(f => f.IdDossier == parentId)
            .ToList();

        foreach (var file in files)
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