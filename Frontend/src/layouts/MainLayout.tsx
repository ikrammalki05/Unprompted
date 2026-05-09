import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import Sidebar from "../components/Sidebar";
import { Topbar } from "../features/admin/dash/components/Topbar";

export default function MainLayout() {
  const location = useLocation();
  const navigate = useNavigate();

  const [activeNav, setActiveNav] = useState("dashboard");

  useEffect(() => {
    // La logique "includes" continue de fonctionner même avec les préfixes
    if (location.pathname.includes("dashboard")) setActiveNav("dashboard");
    if (location.pathname.includes("gestion")) setActiveNav("users");
    if (location.pathname.includes("profile")) setActiveNav("profile");
  }, [location.pathname]);

  const handleNav = (id: string) => {
    setActiveNav(id);

    // 💡 L'ASTUCE EST ICI : On extrait le rôle depuis l'URL
    // Si l'URL est "/admin/dashboard", split('/') donne ["", "admin", "dashboard"]
    // Donc basePath deviendra "/admin"
    const rolePrefix = `/${location.pathname.split('/')[1]}`;

    // On utilise ce préfixe dynamique pour la navigation
    if (id === "dashboard") navigate(`${rolePrefix}/dashboard`);
    if (id === "users") navigate(`${rolePrefix}/gestion`);
    if (id === "profile") navigate(`${rolePrefix}/profile`);
  };

  return (
    <div className="flex">
      <Sidebar activeNav={activeNav} setActiveNav={handleNav} />
      <div className="flex-1">
        <Topbar />
        <Outlet />
      </div>
    </div>
  );
}