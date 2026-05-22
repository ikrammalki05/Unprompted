using System;

namespace Unprompted.Services.Projet.Application.DTOs;

public class GroupeDto
{
    public int IdGroupe { get; set; }
    public string NomGroupe { get; set; } = string.Empty;
    public DateTime? DateCreation { get; set; }
    public int IdProjet { get; set; }
}