import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import Sidebar from "../components/Sidebar";
import { Topbar } from "../features/admin/components/Topbar";

export default function MainLayout() {
  const location = useLocation();
  const navigate = useNavigate();

  const [activeNav, setActiveNav] = useState("dashboard");

  useEffect(() => {
    if (location.pathname.includes("dashboard")) setActiveNav("dashboard");
    if (location.pathname.includes("gestion")) setActiveNav("users");
    if (location.pathname.includes("profile")) setActiveNav("profile");
  }, [location.pathname]);

  const handleNav = (id: string) => {
    setActiveNav(id);

    if (id === "dashboard") navigate("/dashboard");
    if (id === "users") navigate("/gestion");
    if (id === "profile") navigate("/profile");
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