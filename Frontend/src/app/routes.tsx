import { createBrowserRouter, Navigate } from 'react-router-dom';
import MainLayout from '../layouts/MainLayout';
import { DashboardPage } from '../pages/DashboardPage';
import  GestionPage from '../pages/GestionPage';
import ProfilePage from '../pages/ProfilePage';
// Tu peux supprimer l'import de LoginPage

export const router = createBrowserRouter([
  {
    path: '/',
    // Redirection automatique de la racine vers le dashboard
    element: <Navigate to="/dashboard" replace />, 
  },
  {
    // On englobe nos pages dans le MainLayout
    element: <MainLayout />,
    children: [
      {
        path: '/dashboard',
        element: <DashboardPage />,
      },
      {
        path: '/gestion',
        element: <GestionPage />,
      },
      {
        path: '/profile',
        element: <ProfilePage />,
      },
      // Tes futures pages iront ici...
      // { path: '/gestion', element: <UsersPage /> },
    ]
  },
]);