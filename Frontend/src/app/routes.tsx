import { createBrowserRouter } from 'react-router-dom';
import { RoleRedirect } from '../features/auth/components/RoleRedirect';
import MainLayout from '../layouts/MainLayout';
// Tes imports de pages...
import { DashboardPage } from '../pages/admin/DashboardPage'; 

import { GestionPage } from '../pages/admin/GestionPage';
import { ProfilePage } from '../pages/admin/ProfilePage';
import { UnauthorizedPage } from '../features/auth/components/UnauthorizedPage';
import CodeEditorPage from '../pages/student/CodeEditorPage';
import { UnifiedHistoryPage } from '../pages/student/Historique';

export const router = createBrowserRouter([
  {
    // L'entrée principale de l'application
    path: '/',
    element: <RoleRedirect />, 
  },
  {
    // Espace Administrateur
    path: '/admin',
    element: <MainLayout />,
    children: [
      { path: 'dashboard', element: <DashboardPage /> },
      { path: 'gestion', element: <GestionPage /> },
      { path: 'profile', element: <ProfilePage /> },
      { path: 'code', element: <CodeEditorPage /> },
      { path: 'code', element: <CodeEditorPage /> },
      { path: 'historique', element: <UnifiedHistoryPage /> },

      // Autres routes admin...
    ]
  },
  // {
  //   // Espace Étudiant (Tu pourras créer un Layout spécifique si la Sidebar est différente)
  //   path: '/etudiant',
  //   element: <MainLayout />, 
  //   children: [
  //     { path: 'dashboard', element: <EtudiantDashboard /> },
  //   ]
  // },
  // {
  //   // Espace Enseignant
  //   path: '/enseignant',
  //   element: <MainLayout />,
  //   children: [
  //     { path: 'dashboard', element: <EnseignantDashboard /> },
  //   ]
  // },
  {
    // Page d'erreur si pas de rôle
    path: '/unauthorized',
    element: <UnauthorizedPage />
  }
]);