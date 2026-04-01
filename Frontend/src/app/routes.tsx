import { createBrowserRouter, Navigate } from 'react-router-dom';
import { LoginPage } from '../pages/LoginPage';

export const router = createBrowserRouter([
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    // Redirection par défaut : si l'utilisateur arrive sur la racine '/', 
    // on l'envoie vers la page de connexion pour l'instant.
    path: '/',
    element: <Navigate to="/login" replace />,
  },
  {
    // Page 404 (Optionnel pour le moment) : si l'URL n'existe pas
    path: '*',
    element: <div className="flex h-screen items-center justify-center">Page introuvable</div>,
  }
]);