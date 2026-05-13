import api from './api';

export interface ProjetDto {
  id?: number;
  titre: string;
  description?: string;
  dateDebut?: string;
  dateFin?: string;
  urlGit?: string;
  duree?: number;
}

export interface AssignerEtudiantDto {
  idEtudiant: number;
  idProjet: number;
}

export interface AssignerGroupeDto {
  idProjet: number;
  idGroupe: number;
}

export interface AssignationProjetRequestDto {
  idProjet: number;
  nomGroupe: string;
  etudiants: Array<{
    idEtudiant: number;
    idRole: number;
  }>;
}

const projetService = {
  // GET ALL
  getAll: async () => {
    const response = await api.get('/projet');
    return response.data;
  },

  // GET BY ID
  getById: async (id: number) => {
    const response = await api.get(`/projet/${id}`);
    return response.data;
  },

  // CREATE
  create: async (projet: ProjetDto, enseignantId: number) => {
    const response = await api.post(
      `/projet/enseignant/${enseignantId}`,
      projet
    );

    return response.data;
  },

  // UPDATE
  update: async (
    id: number,
    enseignantId: number,
    projet: ProjetDto
  ) => {
    const response = await api.put(
      `/projet/${id}/enseignant/${enseignantId}`,
      projet
    );

    return response.data;
  },

  // DELETE
  delete: async (id: number, enseignantId: number) => {
    const response = await api.delete(
      `/projet/${id}/enseignant/${enseignantId}`
    );

    return response.data;
  },

  // COUNT
  count: async () => {
    const response = await api.get('/projet/count');
    return response.data;
  },

  // UPDATE SUIVI
  updateSuivi: async (
    id: number,
    data: {
      progression: number;
      notes_enseignant?: string;
    }
  ) => {
    const response = await api.patch(
      `/projet/${id}/suivi`,
      data
    );

    return response.data;
  },

  // GET BY ENSEIGNANT
  getByEnseignant: async (enseignantId: number) => {
    const response = await api.get(`/projet/enseignant/${enseignantId}`);
    return response.data;
  },

  // GET GROUPES DU PROJET
  getGroupes: async (id: number) => {
    const response = await api.get(`/projet/${id}/groupes`);
    return response.data;
  },

  // ASSIGNER ETUDIANT
  assignerEtudiant: async (data: AssignerEtudiantDto, enseignantId: number) => {
    const response = await api.post(`/projet/assigner-etudiant?idEnseignant=${enseignantId}`, data);
    return response.data;
  },

  // ASSIGNER GROUPE
  assignerGroupe: async (data: AssignerGroupeDto) => {
    const response = await api.post('/projet/assigner-groupe', data);
    return response.data;
  },

  // ASSIGNATION COMPLETE (CREER EQUIPE ET ASSIGNER)
  assignerEquipeComplet: async (data: AssignationProjetRequestDto) => {
    const response = await api.post('/AssignationProjet', data);
    return response.data;
  },

  // GET CONTRIBUTIONS (COMMITS)
  getContributions: async (id: number) => {
    const response = await api.get(`/projet/${id}/contributions`);
    return response.data;
  },
};

export default projetService;