import api from './api';

export interface EvaluationDto {
  id?: number;
  etudiantId: number;
  projetId: number;
  performanceTechnique: number;
  noteFinale: number;
  commentaire: string;
}

const evaluationService = {
  // GET ALL
  getAll: async () => {
    const response = await api.get('/evaluation');
    return response.data;
  },

  // CREATE
  create: async (evaluation: EvaluationDto) => {
    const response = await api.post('/evaluation', evaluation);
    return response.data;
  },

  // UPDATE
  update: async (id: number, evaluation: EvaluationDto) => {
    const response = await api.put(`/evaluation/${id}`, evaluation);
    return response.data;
  },

  // DELETE
  delete: async (id: number) => {
    const response = await api.delete(`/evaluation/${id}`);
    return response.data;
  },
};

export default evaluationService;