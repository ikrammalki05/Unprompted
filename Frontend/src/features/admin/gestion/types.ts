export interface Student {
  id: number;
  codeApogee: string;
  nom: string;
  nomComplet?: string;
  email: string;
  classeNom: string;
  statut: 'Actif' | 'Inactif';
}

export interface Teacher {
  id: number;
  nom: string;
  nomComplet?: string;
  email: string;
  specialite: string;
  classes: string[];
  statut: 'Actif' | 'Inactif';
}

export interface ClassData {
  id: number;
  nomClasse: string;
  desc: string;
  anneeAcademique: string;
  effectifActuel: number;
  effectifMax: number;
  enseignantReferent: string;
}