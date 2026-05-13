import api from './api';

export interface StudentProfile {
  idEtudiant: number;
  nomComplet: string;
  email: string;
  codeApogee: string;
  niveau: string;
  filiere: string;
  statut: string;
}

const profileService = {
  // Obtenir le profil de l'étudiant connecté
  getMyProfile: async (): Promise<StudentProfile> => {
    const response = await api.get('/Profil/me');
    return response.data;
  },

  // Obtenir le profil d'un étudiant par son ID (pour les enseignants/admin)
  getStudentProfile: async (id: number): Promise<StudentProfile> => {
    const response = await api.get(`/Profil/etudiant/${id}`);
    return response.data;
  },

  // Mettre à jour le profil de l'étudiant connecté
  updateMyProfile: async (data: any) => {
    const response = await api.put('/Profil/me', data);
    return response.data;
  },
};

export default profileService;
