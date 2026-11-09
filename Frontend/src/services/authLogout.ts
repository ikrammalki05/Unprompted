import { keycloak } from './keycloak';

/**
 * Déconnexion Keycloak robuste (localhost + 127.0.0.1).
 */
export function logoutApp() {
  const origin = window.location.origin.replace(/\/$/, '');
  const redirectUri = `${origin}/`;

  // Nettoyage local avant redirection
  try {
    localStorage.removeItem('token');
    sessionStorage.clear();
  } catch {
    /* ignore */
  }

  if (keycloak.authenticated) {
    keycloak.logout({ redirectUri });
    return;
  }

  window.location.assign(redirectUri);
}
