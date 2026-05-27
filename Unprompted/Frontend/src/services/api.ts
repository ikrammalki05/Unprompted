import axios from 'axios';
import { keycloak } from './keycloak';   // Make sure path is correct

export const api = axios.create({
  baseURL: (import.meta.env.VITE_API_URL || 'http://localhost:5000') + '/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// 🔥 Important: Attach Keycloak token to every request
api.interceptors.request.use(
  async (config) => {
    if (keycloak.authenticated) {
      try {
        // Refresh token if it's about to expire
        await keycloak.updateToken(30);
        config.headers.Authorization = `Bearer ${keycloak.token}`;
      } catch (error) {
        console.error('Failed to refresh token', error);
        // Optionally logout user
        // keycloak.logout();
      }
    }
    return config;
  },
  (error) => Promise.reject(error)
);

