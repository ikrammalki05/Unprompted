using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.DTOs;
using Unprompted.Services.Projet.Application.Interfaces;
using Unprompted.Services.Projet.Domain.Entities;

namespace Unprompted.Services.Projet.Application.Services;

public class GroupeService
{
    private readonly IGroupeRepository _groupeRepository;
    private readonly IAffectationRepository _affectationRepository;

    public GroupeService(IGroupeRepository groupeRepository, IAffectationRepository affectationRepository)
    {
        _groupeRepository = groupeRepository;
        _affectationRepository = affectationRepository;
    }

    public async Task<GroupeDto> CreateGroupeAsync(GroupeCreateDto dto)
    {
        var groupe = new Groupe
        {
            NomGroupe = dto.NomGroupe,
            DateCreation = DateTime.UtcNow,
            IdProjet = dto.IdProjet
        };

        await _groupeRepository.AddAsync(groupe);
        await _groupeRepository.SaveChangesAsync();

        return new GroupeDto
        {
            IdGroupe = groupe.IdGroupe,
            NomGroupe = groupe.NomGroupe,
            DateCreation = groupe.DateCreation,
            IdProjet = groupe.IdProjet
        };
    }

    public async Task<bool> AssignerEtudiantAsync(int groupeId, int etudiantId, int roleId)
    {
        var affectation = new Affectation
        {
            IdGroupe = groupeId,
            IdEtudiant = etudiantId,
            IdRole = roleId,
            DateAffectation = DateTime.UtcNow
        };

        await _affectationRepository.AddAsync(affectation);
        return await _affectationRepository.SaveChangesAsync();
    }
}