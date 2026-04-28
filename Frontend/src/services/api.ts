import axios from 'axios';
import { keycloak } from './keycloak';

export const api = axios.create({
  baseURL: 'http://localhost:5000/api', // Remplace par l'URL de ton vrai Backend
  headers: {
    'Content-Type': 'application/json'
  }
});

// Intercepteur : Avant chaque requête, on ajoute le token
api.interceptors.request.use(
  (config) => {
    if (keycloak.token) {
      config.headers.Authorization = `Bearer ${keycloak.token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);