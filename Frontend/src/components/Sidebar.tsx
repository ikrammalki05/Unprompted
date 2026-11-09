import React from 'react';
import {
  LayoutGrid,
  FolderCog,
  BarChart2,
  ClipboardCheck,
  Settings,
  LogOut,
  HelpCircle,
} from 'lucide-react';
import logo from '../assets/logo.png';
import { logoutApp } from '../services/authLogout';
import { useCurrentUserDisplay } from '../hooks/useCurrentUserDisplay';

interface SidebarItemProps {
  icon: React.ElementType | React.FC;
  label: string;
  active?: boolean;
}

const AIConfigIcon: React.FC<{ className?: string }> = ({ className }) => (
  <svg
    className={className}
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
  >
    <path d="M12 2C9.0 2 6.5 4.5 6.5 8c0 2.2 1.1 4.1 2.8 5.3V15h5.4v-1.7C16.4 12.1 17.5 10.2 17.5 8c0-3.5-2.5-6-5.5-6z" />
    <path d="M9.3 15v1.5a2.7 2.7 0 0 0 5.4 0V15" />
    <circle cx="12" cy="8.5" r="1.2" />
    <path d="M12 6.2V7m0 3v.8M10.1 7.2l.6.6m2.6 2.4.6.6M9.7 8.5H10.5m3 0h.8M10.1 9.8l.6-.6m2.6-2.4.6-.6" />
  </svg>
);

const SidebarItem: React.FC<SidebarItemProps> = ({ icon: Icon, label, active }) => (
  <div
    className={`flex items-center gap-4 px-4 py-3 rounded-2xl cursor-pointer transition-all duration-200 group ${
      active
        ? 'bg-slate-100 text-slate-900 font-bold shadow-sm'
        : 'text-slate-500 hover:bg-slate-50 hover:text-slate-800'
    }`}
  >
    <Icon
      className={`w-5 h-5 transition-colors ${
        active ? 'text-blue-600' : 'text-slate-400 group-hover:text-blue-500'
      }`}
      strokeWidth={2.5}
    />
    <span className="text-[14px] tracking-tight">{label}</span>
  </div>
);

interface SidebarProps {
  activePage: string;
  onPageChange: (page: string) => void;
  onNewProject?: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ activePage, onPageChange }) => {
  const user = useCurrentUserDisplay();

  return (
    <aside className="w-[300px] h-screen bg-white border-r border-slate-100 flex flex-col fixed left-0 top-0 z-20">
      <div className="p-8 mb-2 flex flex-col items-center gap-3">
        <div className="w-16 h-16 rounded-full bg-white flex items-center justify-center p-2 shadow-md border border-slate-100">
          <img src={logo} alt="Unprompted Logo" className="max-h-full w-auto object-contain" />
        </div>
        <p className="text-slate-800 text-sm font-bold tracking-wide">UNPROMPTED</p>
        <p className="text-[10px] text-slate-400 font-semibold uppercase tracking-widest -mt-2">
          Gouvernance &amp; Évaluation de l&apos;IA
        </p>
      </div>

      <nav className="flex-1 px-5 space-y-1 overflow-y-auto">
        <div onClick={() => onPageChange('dashboard')}>
          <SidebarItem icon={LayoutGrid} label="Tableau de bord" active={activePage === 'dashboard'} />
        </div>
        <div onClick={() => onPageChange('projects')}>
          <SidebarItem icon={FolderCog} label="Gestion des projets" active={activePage === 'projects'} />
        </div>
        <div onClick={() => onPageChange('details')}>
          <SidebarItem icon={BarChart2} label="Détails du projet" active={activePage === 'details'} />
        </div>
        <div onClick={() => onPageChange('config')}>
          <SidebarItem icon={AIConfigIcon} label="Configuration IA" active={activePage === 'config'} />
        </div>
        <div onClick={() => onPageChange('evaluation')}>
          <SidebarItem icon={ClipboardCheck} label="Évaluation" active={activePage === 'evaluation'} />
        </div>
      </nav>

      <div className="p-5 border-t border-slate-100 space-y-2 bg-white">
        <div onClick={() => onPageChange('profile')} className="cursor-pointer group">
          <div
            className={`flex items-center gap-4 px-4 py-3 rounded-2xl transition-all ${
              activePage === 'profile' ? 'bg-slate-100' : 'hover:bg-slate-50'
            }`}
          >
            <div className="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center text-blue-700 font-bold">
              {user.initials}
            </div>
            <div className="flex flex-col min-w-0">
              <span className="text-sm font-bold text-slate-900 leading-tight truncate">
                {user.fullName}
              </span>
              <span className="text-[10px] text-slate-400 font-semibold tracking-wider">
                {user.roleLabel}
              </span>
            </div>
          </div>
        </div>

        <div onClick={() => onPageChange('settings')}>
          <SidebarItem icon={Settings} label="Paramètres" active={activePage === 'settings'} />
        </div>
        <div onClick={() => {}}>
          <SidebarItem icon={HelpCircle} label="Aide" active={false} />
        </div>
        <div onClick={() => logoutApp()}>
          <div className="flex items-center gap-4 px-4 py-3 rounded-2xl cursor-pointer transition-all text-red-500 hover:bg-red-50 group">
            <LogOut className="w-5 h-5" strokeWidth={2.5} />
            <span className="text-[14px] tracking-tight font-bold">Déconnexion</span>
          </div>
        </div>
      </div>
    </aside>
  );
};

export default Sidebar;
