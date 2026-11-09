import api from './api';

export interface ConfigurationIumCreateDto {
  idProjet: number;
  quotaRequetes?: number;
  quotaTokens?: number;
  periodeQuota?: string;
  generationCodeAutorisee: boolean;
}

const iaService = {
  // Upload PDF + index RAG
  uploadCahierCharge: async (file: File, idProjet: number) => {
    const formData = new FormData();
    formData.append('fichier', file);
    formData.append('file', file);

    const response = await api.post(`/projet/${idProjet}/upload-cahier`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });

    return response.data;
  },

  // Save IA configuration
  saveConfiguration: async (data: ConfigurationIumCreateDto) => {
    const response = await api.post('/ConfigurationIum', data);
    return response.data;
  },

  // Get prompts activity for a project
  getPrompts: async (idProjet: number) => {
    const response = await api.get(`/Prompt/projet/${idProjet}`);
    return response.data;
  },

  // Export CSV
  exportCsv: async (idProjet: number) => {
    const response = await api.get(
      `/Analytics/projet/${idProjet}`,
      {
        responseType: 'blob',
      }
    );

    return response.data;
  },
};

export default iaService;