import api from './api';

export interface EtudiantRoleDto {
  idEtudiant: number;
  idRole: number;
}

export interface GroupeCreateDto {
  nomGroupe: string;
  idProjet?: number;
  etudiants: EtudiantRoleDto[];
}

export interface GroupeDto {
  idGroupe: number;
  nomGroupe: string;
  idProjet?: number;
  etudiants: Array<{
    idEtudiant: number;
    nomComplet: string;
    role: string;
  }>;
}

const groupeService = {
  // GET ALL
  getAll: async (): Promise<GroupeDto[]> => {
    const response = await api.get('/groupe');
    return response.data;
  },

  // GET BY ID
  getById: async (id: number): Promise<GroupeDto> => {
    const response = await api.get(`/groupe/${id}`);
    return response.data;
  },

  // CREATE
  create: async (data: GroupeCreateDto): Promise<GroupeDto> => {
    const response = await api.post('/groupe', data);
    return response.data;
  },

  // DELETE
  delete: async (id: number): Promise<void> => {
    await api.delete(`/groupe/${id}`);
  },

  // ADD STUDENT TO GROUP
  addEtudiant: async (id: number, etudiant: EtudiantRoleDto): Promise<void> => {
    await api.post(`/groupe/${id}/etudiant`, etudiant);
  },

  // REMOVE STUDENT FROM GROUP
  removeEtudiant: async (id: number, idEtudiant: number): Promise<void> => {
    await api.delete(`/groupe/${id}/etudiant/${idEtudiant}`);
  },
};

export default groupeService;
