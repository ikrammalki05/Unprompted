import type { Student, Teacher, ClassData } from './types';

export const initialStudents: Student[] = [
  { id: 1, code: '21904562', nom: 'Amine Benjelloun', email: 'a.benjelloun@univ.ma', classe: 'Master IABD – G1', statut: 'Actif' },
  { id: 2, code: '21808912', nom: 'Sarah Mansouri', email: 's.mansouri@univ.ma', classe: 'Master IABD – G2', statut: 'Actif' },
  // ... (Copie le reste de tes étudiants ici)
];

export const initialTeachers: Teacher[] = [
  { id: 1, nom: 'Dr. Ahmed Alami', email: 'a.alami@univ.ma', specialite: 'Intelligence Artificielle', classes: ['M1 IABD', 'M2 Data'], statut: 'Actif' },
  // ... (Copie le reste de tes enseignants ici)
];

export const initialClasses: ClassData[] = [
  { id: 1, nom: 'Master IABD – G1', desc: 'Intelligence Artificielle et Big Data', annee: '2023 – 2024', effectif: 34, capacite: 40, enseignant: 'Dr. Ahmed Alami' },
  // ... (Copie le reste de tes classes ici)
];