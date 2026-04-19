import { createBrowserRouter, Navigate } from "react-router-dom";

import MainLayout from "../layouts/MainLayout";

import { DashboardPage } from "../pages/DashboardPage";
import GestionPage from "../pages/GestionPage";
import ProfilePage from "../pages/ProfilePage";
import { LoginPage } from "../pages/LoginPage";

export const router = createBrowserRouter([
  {
    element: <MainLayout />,
    children: [
      { path: "/dashboard", element: <DashboardPage /> },
      { path: "/gestion", element: <GestionPage /> },
      { path: "/profile", element: <ProfilePage /> },

      
      { path: "/", element: <Navigate to="/dashboard" replace /> },
    ],
  },
  {
    path: "/login",
    element: <LoginPage />,
  },
]);