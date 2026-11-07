import { keycloak } from '../services/keycloak';

export type AppRole = 'admin' | 'enseignant' | 'etudiant' | 'unauthorized';

export const getUserMainRole = (): AppRole => {
  const roles = (keycloak.tokenParsed?.realm_access?.roles || []).map((r) =>
    String(r).toLowerCase()
  );

  if (roles.includes('admin')) return 'admin';
  if (roles.includes('enseignant')) return 'enseignant';
  if (roles.includes('etudiant')) return 'etudiant';

  return 'unauthorized';
};
