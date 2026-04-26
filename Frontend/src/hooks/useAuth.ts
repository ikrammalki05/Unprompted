import { useState, useEffect } from 'react';
import { keycloak } from '../services/keycloak';

export const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(keycloak.authenticated);
  const [user, setUser] = useState<any>(null);

  useEffect(() => {
    if (keycloak.authenticated) {
      // Récupère les infos du profil (nom, email) depuis Keycloak
      keycloak.loadUserProfile().then((profile) => {
        setUser(profile);
      });
    }
  }, [keycloak.authenticated]);

  const login = () => keycloak.login();
  const logout = () => keycloak.logout();
  const getToken = () => keycloak.token;

  return { isAuthenticated, user, login, logout, getToken };
};