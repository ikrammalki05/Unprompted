import api from "./api";

// GET projets enseignant
export const getProjetsByEnseignant = (idEnseignant: number) => {
  return api.get(`/Projet/enseignant/${idEnseignant}`);
};

// CREATE projet
export const createProjet = (data: any) => {
  return api.post(`/Projet`, data);
};

// ASSIGNER équipe
export const assignEquipe = (data: any) => {
  return api.post(`/AssignationProjet`, data);
};