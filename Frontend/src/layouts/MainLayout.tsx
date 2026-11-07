import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { Topbar } from "../features/admin/dash/components/Topbar";
import { logoutApp } from "../services/authLogout";
import { useCurrentUserDisplay } from "../hooks/useCurrentUserDisplay";

const navItems = [
  { id: "dashboard", label: "Tableau de bord" },
  { id: "users", label: "Gestion des utilisateurs" },
  { id: "profile", label: "Profil" },
];

export default function MainLayout() {
  const location = useLocation();
  const navigate = useNavigate();
  const [activeNav, setActiveNav] = useState("dashboard");
  const user = useCurrentUserDisplay();

  useEffect(() => {
    if (location.pathname.includes("dashboard")) setActiveNav("dashboard");
    if (location.pathname.includes("gestion")) setActiveNav("users");
    if (location.pathname.includes("profile")) setActiveNav("profile");
  }, [location.pathname]);

  const handleNav = (id: string) => {
    setActiveNav(id);
    const rolePrefix = `/${location.pathname.split("/")[1]}`;
    if (id === "dashboard") navigate(`${rolePrefix}/dashboard`);
    if (id === "users") navigate(`${rolePrefix}/gestion`);
    if (id === "profile") navigate(`${rolePrefix}/profile`);
  };

  return (
    <div className="flex min-h-screen bg-gray-50 font-sans text-slate-900">
      <aside className="w-[280px] min-w-[280px] bg-[#021124] border-r border-white/10 flex flex-col h-screen sticky top-0">
        <div className="flex items-center gap-3 px-5 py-6">
          <div className="w-10 h-10 bg-white rounded-full flex items-center justify-center shadow-lg text-[#021124] font-bold">
            U
          </div>
          <div className="flex flex-col">
            <span className="text-[17px] font-bold text-white leading-tight">Unprompted</span>
            <span className="text-[10px] font-medium text-[#a6c1ee] uppercase tracking-[0.1em] mt-0.5">
              Gouvernance scolaire
            </span>
          </div>
        </div>
        <nav className="flex-1 px-3 flex flex-col gap-1.5">
          {navItems.map((item) => {
            const isActive = activeNav === item.id;
            return (
              <button
                key={item.id}
                type="button"
                onClick={() => handleNav(item.id)}
                className={`w-full text-left rounded-xl px-4 py-3 text-sm font-medium transition-colors ${
                  isActive ? "bg-white/10 text-[#a6c1ee]" : "text-slate-300 hover:bg-white/5 hover:text-white"
                }`}
              >
                {item.label}
              </button>
            );
          })}
        </nav>
        <div className="p-4 border-t border-white/10 space-y-3">
          <div className="flex items-center gap-3 px-3 py-2 rounded-xl bg-white/5">
            <div className="w-9 h-9 rounded-lg bg-[#a6c1ee] text-[#021124] flex items-center justify-center text-sm font-bold">
              {user.initials}
            </div>
            <div className="min-w-0">
              <p className="text-sm font-semibold text-white truncate">{user.fullName}</p>
              <p className="text-[10px] text-[#a6c1ee] font-semibold tracking-wider">{user.roleLabel}</p>
            </div>
          </div>
          <button
            type="button"
            onClick={() => logoutApp()}
            className="w-full rounded-xl px-4 py-3 text-sm font-semibold text-red-300 bg-red-500/10 hover:bg-red-500/20"
          >
            Déconnexion
          </button>
        </div>
      </aside>
      <div className="flex-1 min-w-0">
        <Topbar />
        <Outlet />
      </div>
    </div>
  );
}
