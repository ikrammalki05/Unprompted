using System;

namespace Unprompted.Services.Projet.Domain.Entities;

public class Affectation
{
    public int IdAffectation { get; set; }
    
    // Relations internes au microservice
    public int IdGroupe { get; set; }
    public virtual Groupe? IdGroupeNavigation { get; set; }

    // RUPTURE : L'étudiant et son rôle deviennent de simples identifiants numériques externes
    public int IdEtudiant { get; set; }
    public int IdRole { get; set; } 
    
    public DateTime? DateAffectation { get; set; }
}