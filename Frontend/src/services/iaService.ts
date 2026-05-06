import api from "./api";

// CONFIGURATION IA
export const configIA = (data: any) => {
  return api.post(`/ConfigurationIum`, data);
};

// ANALYTICS projet
export const getAnalyticsProjet = (idProjet: number) => {
  return api.get(`/Analytics/projet/${idProjet}`);
};