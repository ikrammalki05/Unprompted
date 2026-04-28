export interface Student {
  id: number;
  code: string;
  nom: string;
  email: string;
  classe: string;
  statut: 'Actif' | 'Inactif';
}

export interface Teacher {
  id: number;
  nom: string;
  email: string;
  specialite: string;
  classes: string[];
  statut: 'Actif' | 'Inactif';
}

export interface ClassData {
  id: number;
  nom: string;
  desc: string;
  annee: string;
  effectif: number;
  capacite: number;
  enseignant: string;
}