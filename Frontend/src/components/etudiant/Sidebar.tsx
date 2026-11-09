import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Logo from '../../assets/Logo.png';
import {
  LayoutDashboard,
  Monitor,
  History,
  UserCircle,
  Settings,
  LogOut,
} from 'lucide-react';
import { logoutApp } from '../../services/authLogout';
import { useCurrentUserDisplay } from '../../hooks/useCurrentUserDisplay';

interface NavItemProps {
  icon: React.ReactNode;
  label: string;
  active?: boolean;
  onClick?: () => void;
  danger?: boolean;
}

const NavItem: React.FC<NavItemProps> = ({ icon, label, active, onClick, danger }) => (
  <div
    onClick={onClick}
    className={`
      flex items-center gap-3 px-4 py-3 rounded-xl cursor-pointer transition-all duration-200
      ${
        danger
          ? 'text-red-500 hover:bg-red-50'
          : active
            ? 'bg-slate-100 text-slate-900 font-semibold shadow-sm'
            : 'text-slate-500 hover:bg-slate-50 hover:text-slate-800'
      }
    `}
  >
    {icon}
    <span className="text-sm tracking-wide">{label}</span>
  </div>
);

const navItems = [
  { icon: <LayoutDashboard size={20} />, label: 'Tableau de bord', path: '/etudiant/dashboard' },
  { icon: <Monitor size={20} />, label: 'Espace de travail', path: '/etudiant/workspace' },
  { icon: <History size={20} />, label: 'Historique', path: '/etudiant/historique' },
  { icon: <UserCircle size={20} />, label: 'Profil', path: '/etudiant/profile' },
];

const Sidebar: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const user = useCurrentUserDisplay();

  return (
    <aside className="w-64 bg-white border-r border-slate-100 hidden md:flex flex-col min-h-screen">
      <div className="p-6 flex items-center gap-3">
        <div className="w-12 h-12 rounded-xl flex items-center justify-center overflow-hidden bg-white shadow-sm border border-slate-100">
          <img src={Logo} alt="Logo" className="w-full h-full object-contain p-1" />
        </div>
        <div>
          <h1 className="font-bold text-lg text-slate-900 leading-tight">Unprompted</h1>
          <p className="text-[10px] text-slate-400 font-semibold tracking-widest uppercase">
            Excellence Académique
          </p>
        </div>
      </div>

      <nav className="flex-1 mt-4 px-4 space-y-1">
        {navItems.map((item) => (
          <NavItem
            key={item.path}
            icon={item.icon}
            label={item.label}
            active={
              location.pathname === item.path || location.pathname.startsWith(item.path + '/')
            }
            onClick={() => navigate(item.path)}
          />
        ))}
      </nav>

      <div className="px-4 pb-4 space-y-2 border-t border-slate-100 pt-4">
        <NavItem icon={<Settings size={20} />} label="Paramètres" onClick={() => {}} />

        <div className="mt-2 flex items-center gap-3 px-4 py-3 bg-slate-50 rounded-xl border border-slate-100">
          <div className="w-9 h-9 rounded-lg bg-blue-100 flex items-center justify-center text-blue-700 text-sm font-bold">
            {user.initials}
          </div>
          <div className="min-w-0">
            <p className="text-sm font-semibold text-slate-900 truncate">{user.fullName}</p>
            <p className="text-xs text-slate-400 truncate">{user.roleLabel}</p>
          </div>
        </div>

        <NavItem
          icon={<LogOut size={20} />}
          label="Déconnexion"
          danger
          onClick={() => logoutApp()}
        />
      </div>
    </aside>
  );
};

export default Sidebar;
