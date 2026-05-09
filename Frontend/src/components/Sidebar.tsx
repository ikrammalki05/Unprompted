import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Logo from '../assets/Logo.png';
import {
  LayoutDashboard,
  Monitor,
  History,
  UserCircle,
  Settings,
} from 'lucide-react';

interface NavItemProps {
  icon: React.ReactNode;
  label: string;
  active?: boolean;
  onClick?: () => void;
}

const NavItem: React.FC<NavItemProps> = ({ icon, label, active, onClick }) => (
  <div
    onClick={onClick}
    className={`
      flex items-center gap-3 px-4 py-3 rounded-xl cursor-pointer transition-all duration-200
      ${active
        ? 'bg-blue-50 text-blue-600 shadow-sm border border-blue-100 font-semibold'
        : 'text-gray-500 hover:bg-gray-50 hover:text-gray-900'}
    `}
  >
    {icon}
    <span className="text-sm tracking-wide">{label}</span>
  </div>
);

const navItems = [
  { icon: <LayoutDashboard size={20} />, label: 'Tableau de bord', path: '/' },
  { icon: <Monitor size={20} />, label: 'Espace de travail', path: '/espace-travail' },
  { icon: <History size={20} />, label: 'Historique', path: '/historique' },
  { icon: <UserCircle size={20} />, label: 'Profil', path: '/profil' },
];

const Sidebar: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();

  return (
    <aside className="w-64 bg-white border-r border-gray-100 hidden md:flex flex-col min-h-screen">
      {/* Logo */}
      <div className="p-6 flex items-center gap-3">
        <div className="w-12 h-12 rounded-xl flex items-center justify-center overflow-hidden bg-white shadow-sm border border-gray-100">
          <img src={Logo} alt="Logo" className="w-full h-full object-contain p-1" />
        </div>
        <div>
          <h1 className="font-bold text-lg text-gray-900 leading-tight">Unprompted</h1>
          <p className="text-[10px] text-blue-500 font-semibold tracking-widest uppercase">Excellence Académique</p>
        </div>
      </div>

      {/* Navigation */}
      <nav className="flex-1 mt-4 px-4 space-y-1">
        {navItems.map((item) => (
          <NavItem
            key={item.path}
            icon={item.icon}
            label={item.label}
            active={location.pathname === item.path}
            onClick={() => navigate(item.path)}
          />
        ))}
      </nav>

      {/* Bottom section */}
      <div className="px-4 pb-4 space-y-2">
        {/* Settings */}
        <NavItem
          icon={<Settings size={20} />}
          label="Paramètres"
          onClick={() => { }}
        />

        {/* User info */}
        <div className="mt-2 flex items-center gap-3 px-4 py-3 bg-gray-50 rounded-xl border border-gray-100">
          <div className="w-9 h-9 rounded-lg bg-gradient-to-br from-blue-500 to-blue-600 flex items-center justify-center text-white text-sm font-bold shadow-sm">
            A
          </div>
          <div className="min-w-0">
            <p className="text-sm font-semibold text-gray-800 truncate">Alami</p>
            <p className="text-xs text-gray-400 truncate">Etudiant M2</p>
          </div>
        </div>
      </div>
    </aside>
  );
};

export default Sidebar;
