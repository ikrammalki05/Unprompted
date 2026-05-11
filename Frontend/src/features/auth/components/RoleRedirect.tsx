import { Navigate } from 'react-router-dom';
import { getUserMainRole } from '../../../utils/authUtils';

export const RoleRedirect = () => {
  const role = getUserMainRole();

  // On redirige vers la bonne URL en fonction du rôle
  switch (role) {
    case 'admin':
      return <Navigate to="/admin/dashboard" replace />;
    case 'enseignant':
      return <Navigate to="/enseignant/dashboard" replace />;
    case 'etudiant':
      return <Navigate to="/etudiant/dashboard" replace />;
    default:
      // Si l'utilisateur n'a aucun rôle reconnu, on peut l'envoyer sur une page d'erreur
      return <Navigate to="/unauthorized" replace />;
  }
};