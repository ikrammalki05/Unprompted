import api from "./api";

export const getProfile = (id: number) => {
  return api.get(`/Profil/${id}`);
};