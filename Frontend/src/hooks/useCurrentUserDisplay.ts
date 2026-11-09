import { useEffect, useState } from 'react';
import { keycloak } from '../services/keycloak';
import { getUserMainRole } from '../utils/authUtils';
import { api } from '../services/api';

export type DisplayUser = {
  fullName: string;
  firstName: string;
  lastName: string;
  initials: string;
  roleLabel: string;
  email?: string;
};

function fromToken(): DisplayUser {
  const given = keycloak.tokenParsed?.given_name as string | undefined;
  const family = keycloak.tokenParsed?.family_name as string | undefined;
  const preferred = keycloak.tokenParsed?.preferred_username as string | undefined;
  const email = keycloak.tokenParsed?.email as string | undefined;
  const firstName = given || preferred || 'Utilisateur';
  const lastName = family || '';
  const fullName = [firstName, lastName].filter(Boolean).join(' ').trim();
  const role = getUserMainRole();
  const roleLabel =
    role === 'admin' ? 'ADMIN' : role === 'enseignant' ? 'ENSEIGNANT' : role === 'etudiant' ? 'ÉTUDIANT' : 'UTILISATEUR';

  return {
    fullName,
    firstName,
    lastName,
    initials: fullName
      .split(/\s+/)
      .filter(Boolean)
      .slice(0, 2)
      .map((p) => p[0]?.toUpperCase() ?? '')
      .join('') || 'U',
    roleLabel,
    email,
  };
}

export function useCurrentUserDisplay(): DisplayUser {
  const [user, setUser] = useState<DisplayUser>(fromToken);

  useEffect(() => {
    const load = async () => {
      const role = getUserMainRole();
      try {
        if (role === 'enseignant') {
          const res = await api.get('/enseignant/me');
          const d = res.data;
          const firstName = d.prenom || d.Prenom || fromToken().firstName;
          const lastName = d.nom || d.Nom || fromToken().lastName;
          const fullName = `${firstName} ${lastName}`.trim();
          setUser({
            fullName,
            firstName,
            lastName,
            initials: fullName
              .split(/\s+/)
              .slice(0, 2)
              .map((p: string) => p[0]?.toUpperCase() ?? '')
              .join(''),
            roleLabel: 'ENSEIGNANT',
            email: d.email || d.Email,
          });
          return;
        }

        if (role === 'etudiant') {
          const res = await api.get('/Profil/me');
          const d = res.data;
          const fullName = d.nomComplet || d.NomComplet || fromToken().fullName;
          const parts = String(fullName).trim().split(/\s+/);
          setUser({
            fullName,
            firstName: parts[0] || 'Étudiant',
            lastName: parts.slice(1).join(' '),
            initials: parts
              .slice(0, 2)
              .map((p) => p[0]?.toUpperCase() ?? '')
              .join(''),
            roleLabel: 'ÉTUDIANT',
            email: d.email || d.Email,
          });
          return;
        }

        if (role === 'admin') {
          const tokenUser = fromToken();
          setUser({ ...tokenUser, roleLabel: 'ADMIN' });
        }
      } catch {
        setUser(fromToken());
      }
    };

    void load();
  }, []);

  return user;
}
