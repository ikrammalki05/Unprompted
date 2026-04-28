import { useState, useEffect } from 'react';
import { keycloak } from '../services/keycloak';

export const useAuth = () => {
  // On force la valeur en booléen avec !! (au cas où ce soit undefined au démarrage)
  const [isAuthenticated, setIsAuthenticated] = useState(!!keycloak.authenticated);
  const [user, setUser] = useState<any>(null);

  useEffect(() => {
    // 1. On utilise enfin setIsAuthenticated pour informer React du statut !
    setIsAuthenticated(!!keycloak.authenticated);

    if (keycloak.authenticated) {
      // 2. Récupère les infos du profil (nom, email) depuis Keycloak
      keycloak.loadUserProfile().then((profile) => {
        setUser(profile);
      });
    } else {
      // 3. Par sécurité, on vide les infos si l'utilisateur n'est plus authentifié
      setUser(null);
    }
  }, [keycloak.authenticated]);

  const login = () => keycloak.login();
  const logout = () => keycloak.logout();
  const getToken = () => keycloak.token;

  return { isAuthenticated, user, login, logout, getToken };
};