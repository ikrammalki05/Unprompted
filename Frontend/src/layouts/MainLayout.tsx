import { Outlet, useLocation, useNavigate } from "react-router-dom";
// Plus besoin d'importer useState et useEffect !
import Sidebar from "../components/Sidebar";
import { Topbar } from "../features/admin/dash/components/Topbar";

export default function MainLayout() {
  const location = useLocation();
  const navigate = useNavigate();

  // 1. Calcul direct de l'onglet actif basé sur l'URL actuelle
  const activeNav = 
    location.pathname.includes("dashboard") ? "dashboard" :
    location.pathname.includes("gestion") ? "users" :
    location.pathname.includes("profile") ? "profile" : 
    "dashboard"; // Valeur par défaut

  const handleNav = (id: string) => {
    // 2. Plus besoin de setActiveNav(id) ! 
    // Le simple fait d'appeler navigate() va changer location.pathname, 
    // re-rendre le composant, et recalculer activeNav automatiquement.

    // 💡 On extrait le rôle depuis l'URL
    const rolePrefix = `/${location.pathname.split('/')[1]}`;

    // On utilise ce préfixe dynamique pour la navigation
    if (id === "dashboard") navigate(`${rolePrefix}/dashboard`);
    if (id === "users") navigate(`${rolePrefix}/gestion`);
    if (id === "profile") navigate(`${rolePrefix}/profile`);
  };

  return (
    <div className="flex">
      {/* On passe toujours handleNav à la prop setActiveNav de la Sidebar */}
      <Sidebar activeNav={activeNav} setActiveNav={handleNav} />
      <div className="flex-1">
        <Topbar />
        <Outlet />
      </div>
    </div>
  );
}