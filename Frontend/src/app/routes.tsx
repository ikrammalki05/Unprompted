import { createBrowserRouter, Navigate } from 'react-router-dom';
import { LoginPage } from '../pages/LoginPage';
import { DashboardPage } from '../pages/DashboardPage'; // 👈 Ajoute cet import

export const router = createBrowserRouter([
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    path: '/dashboard', // 👈 Nouvelle route pour le dashboard
    element: <DashboardPage />,
  },
  {
    path: '/',
    // Tu pourras changer ça plus tard pour vérifier si l'utilisateur est connecté
    element: <Navigate to="/dashboard" replace />, 
  },
  // ...
]);