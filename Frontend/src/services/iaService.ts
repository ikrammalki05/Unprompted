import api from './api';

export interface ConfigurationIumCreateDto {
  idProjet: number;
  quotaRequetes?: number;
  quotaTokens?: number;
  periodeQuota?: string;
  generationCodeAutorisee: boolean;
}

const iaService = {
  // Upload PDF
  uploadCahierCharge: async (file: File) => {
    const formData = new FormData();
    formData.append('file', file);

    const response = await api.post('/File/upload', formData, {
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

  // Get prompts activity
  getPrompts: async () => {
    const response = await api.get('/Prompt/log');
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