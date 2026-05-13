import { keycloak } from '../services/keycloak';

// Définition de nos rôles applicatifs
export type AppRole = 'admin' | 'enseignant' | 'etudiant' | 'unauthorized';

export const getUserMainRole = (): AppRole => {
    // On récupère le tableau des rôles depuis le token décodé
    const roles = keycloak.tokenParsed?.realm_access?.roles || [];

    // L'ordre des "if" définit la priorité si un utilisateur a plusieurs rôles
    if (roles.includes('admin')) return 'admin';
    if (roles.includes('enseignant')) return 'enseignant';
    if (roles.includes('etudiant')) return 'etudiant';

    return 'unauthorized';
};