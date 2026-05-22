using System;

namespace Unprompted.Services.Projet.Application.DTOs;

public class GroupeCreateDto
{
    public string NomGroupe { get; set; } = string.Empty;
    public int IdProjet { get; set; }
}