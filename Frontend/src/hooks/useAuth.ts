import { useState, useEffect } from 'react';
import { keycloak } from '../services/keycloak';

export const useAuth = () => {
  // 1. Valeur dérivée directement, PAS de useState !
  const isAuthenticated = !!keycloak.authenticated;

  type User = {
    name?: string;
    email?: string;
  };

  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    // 2. On utilise uniquement le useEffect pour l'opération asynchrone (le profil)
    if (isAuthenticated) {
      keycloak.loadUserProfile().then((profile) => {
        setUser(profile);
      });
    } else {
      setUser(null);
    }
  }, [isAuthenticated]); // Le hook réagit à la valeur dérivée

  const login = () => keycloak.login();
  const logout = () => keycloak.logout();
  const getToken = () => keycloak.token;

  return { isAuthenticated, user, login, logout, getToken };
};