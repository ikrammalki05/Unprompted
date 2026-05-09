import React from 'react';
<<<<<<< HEAD
import { useLocation, useNavigate } from 'react-router-dom';
import Logo from '../assets/Logo.png';
import {
  LayoutDashboard,
  FolderRoot,
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
=======
import {keycloak} from "../services/keycloak"

interface NavItem {
  id: string;
  label: string;
  icon: React.ReactNode;
}

const navItems: NavItem[] = [
  {
    id: 'dashboard',
    label: 'Tableau de bord',
    icon: (
      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
        <rect x="3" y="3" width="7" height="7" rx="1" />
        <rect x="14" y="3" width="7" height="7" rx="1" />
        <rect x="3" y="14" width="7" height="7" rx="1" />
        <rect x="14" y="14" width="7" height="7" rx="1" />
      </svg>
    ),
  },
  {
    id: 'users',
    label: 'Gestion des utilisateurs',
    icon: (
      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
        <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
        <circle cx="9" cy="7" r="4" />
        <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
        <path d="M16 3.13a4 4 0 0 1 0 7.75" />
      </svg>
    ),
  },
  {
    id: 'profile',
    label: 'Profil',
    icon: (
      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
        <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
        <circle cx="12" cy="7" r="4" />
      </svg>
    ),
  },
];

interface SidebarProps {
  activeNav: string;
  setActiveNav: (id: string) => void;
}

export default function Sidebar({ activeNav, setActiveNav }: SidebarProps) {

  const handleLogout = () => {
    keycloak.logout({
      // Optionnel : Tu peux forcer Keycloak à te ramener à la racine de ton site après la déconnexion
      redirectUri: window.location.origin
    });
  };

  return (
    <aside className="w-[var(--sidebar-width)] min-w-[var(--sidebar-width)] bg-[#fcfcfd] border-r border-[#f1f5f9] flex flex-col h-screen sticky top-0 overflow-hidden">
      <div className="flex items-center gap-3 p-[24px_16px_20px]">
        <div className="w-10 h-10 bg-white border border-[#f1f5f9] rounded-lg flex items-center justify-center p-1.5 shadow-sm">
          <img src="/logo.png" alt="Logo" className="w-full h-full object-contain" />
        </div>
        <div className="flex flex-col">
          <span className="text-[17px] font-bold text-[#1e293b] leading-tight tracking-tight">Unprompted</span>
          <span className="text-[10px] font-medium text-[#94a3b8] uppercase tracking-[0.1em] mt-0.5">Gouvernance Scolaire</span>
        </div>
      </div>

      <nav className="flex-1 p-[8px_12px] flex flex-col gap-1.5 mt-2">
        {navItems.map((item) => {
          const isActive = activeNav === item.id;
          return (
            <button
              key={item.id}
              className={`w-full flex items-center gap-3 p-[10px_14px] border-none rounded-xl cursor-pointer text-left font-['Inter',_sans-serif] text-[13.5px] font-medium transition-[var(--transition)] relative group ${
                isActive 
                  ? 'bg-white text-[var(--accent)] shadow-[0_2px_12px_rgba(37,99,235,0.06)]' 
                  : 'text-[#64748b] hover:bg-[#f8fafc] hover:text-[#1e293b]'
              }`}
              onClick={() => setActiveNav(item.id)}
            >
              <span className={`flex items-center shrink-0 transition-colors ${isActive ? 'text-[var(--accent)]' : 'text-[#94a3b8] group-hover:text-[#64748b]'}`}>
                {item.icon}
              </span>
              <span className="leading-none">{item.label}</span>
              {isActive && (
                <span className="absolute right-0 top-1/2 -translate-y-1/2 w-[3px] h-5 bg-[var(--accent)] rounded-full mr-0.5" />
              )}
            </button>
          );
        })}
      </nav>

      <div className="p-4 flex flex-col gap-2 bg-[#fcfcfd]">
        <button className="w-full flex items-center gap-3 p-[10px_14px] border-none rounded-xl bg-transparent cursor-pointer font-['Inter',_sans-serif] text-sm font-medium text-[#64748b] transition-[var(--transition)] hover:bg-[#f8fafc] hover:text-[#1e293b]">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <circle cx="12" cy="12" r="10" /><path d="M9.09 9a3 3 0 0 1 5.83 1 c0 2-3 3-3 3" /><line x1="12" y1="17" x2="12.01" y2="17" />
          </svg>
          Assistance
        </button>
        <button 
          onClick={handleLogout}
          className="w-full flex items-center gap-3 p-[10px_14px] border-none rounded-xl cursor-pointer font-['Inter',_sans-serif] text-sm font-semibold transition-[var(--transition)] bg-[#fee2e2] text-[#ef4444] mt-1 hover:bg-[#fecaca] hover:text-[#dc2626]"
        >
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4" /><polyline points="16 17 21 12 16 7" /><line x1="21" y1="12" x2="9" y2="12" />
          </svg>
          Déconnexion
        </button>
      </div>
    </aside>
  );
}
>>>>>>> feature/etudiants-projets-cahier-charge
