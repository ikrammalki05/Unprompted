import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { keycloak } from '../../../services/keycloak';

export const RoleRedirect = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const redirectByRole = () => {
      if (!keycloak.authenticated) {
        navigate('/unauthorized');
        return;
      }

      const roles = keycloak.realmAccess?.roles || [];

      console.log("🔑 Roles détectés:", roles); // Debug

      // Priority order
      if (roles.includes('Admin')) {
        navigate('/admin/dashboard', { replace: true });
      } 
      else if (roles.includes('Enseignant')) {
        navigate('/enseignant/dashboard', { replace: true });
      } 
      else if (roles.includes('Etudiant')) {
        navigate('/etudiant/dashboard', { replace: true });   // ← Change if you prefer /profile
      } 
      else {
        navigate('/unauthorized', { replace: true });
      }
    };

    redirectByRole();
    setLoading(false);
  }, [navigate]);

  if (loading) {
    return (
      <div className="flex h-screen items-center justify-center">
        <p className="text-lg">Redirection en cours...</p>
      </div>
    );
  }

  return null;
};