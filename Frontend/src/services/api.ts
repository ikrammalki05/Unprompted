import axios from "axios";

// Prefer 127.0.0.1 to match Vite --host and Keycloak redirect URIs.
export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || "http://127.0.0.1:5066/api",
  headers: {
    "Content-Type": "application/json",
  },
});

import { keycloak } from './keycloak';
import { logoutApp } from './authLogout';

// DEBUG
console.log("API BASE URL =", api.defaults.baseURL);

// Ajouter automatiquement le token JWT de Keycloak
api.interceptors.request.use(
  async (config) => {
    // Si Keycloak est initialisé et qu'on a un token
    if (keycloak.token) {
      try {
        // On rafraîchit le token s'il expire dans moins de 30 secondes
        await keycloak.updateToken(30);
        config.headers.Authorization = `Bearer ${keycloak.token}`;
      } catch (error) {
        console.error("Session expirée ou erreur de rafraîchissement", error);
        keycloak.login();
      }
    }

    // DEBUG URL appelée
    console.log("REQUEST =>", `${config.baseURL}${config.url}`);

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Gestion globale des erreurs
api.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error(
      "API ERROR =>",
      error.response?.status,
      error.response?.data
    );

    if (error.response?.status === 401) {
      logoutApp();
    }

    return Promise.reject(error);
  }
);

export default api;