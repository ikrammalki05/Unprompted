import React from 'react';
import { 
  Files, 
  Search, 
  GitBranch, 
  PlaySquare, 
  Blocks, 
  User, 
  Settings 
} from 'lucide-react';

interface ActivityBarProps {
  activeTab: string;
  onTabChange: (tab: string) => void;
}

export const ActivityBar: React.FC<ActivityBarProps> = ({ activeTab, onTabChange }) => {
  // Petit composant interne pour styliser les icônes facilement
  const IconButton = ({ id, icon: Icon }: { id: string, icon: any }) => {
    const isActive = activeTab === id;
    return (
      <div 
        onClick={() => onTabChange(id)}
        className={`w-full flex justify-center py-3 cursor-pointer relative
          ${isActive ? 'text-white' : 'text-[#858585] hover:text-white transition-colors'}`}
      >
        {/* Liseré bleu à gauche pour l'onglet actif */}
        {isActive && (
          <div className="absolute left-0 top-0 bottom-0 w-[2px] bg-[#007acc]"></div>
        )}
        <Icon strokeWidth={1.5} size={28} />
      </div>
    );
  };

  return (
    // La barre fait 50px de large, a un fond très sombre (#181818)
    <div className="w-[50px] shrink-0 bg-[#181818] flex flex-col justify-between h-full border-r border-[#2b2b2b] select-none">
      
      {/* Menu du haut */}
      <div className="flex flex-col items-center pt-2">
        <IconButton id="explorer" icon={Files} />
        <IconButton id="search" icon={Search} />
        <IconButton id="git" icon={GitBranch} />
        <IconButton id="debug" icon={PlaySquare} />
        <IconButton id="extensions" icon={Blocks} />
      </div>

      {/* Menu du bas (Profil / Paramètres) */}
      <div className="flex flex-col items-center pb-2">
        <IconButton id="profile" icon={User} />
        <IconButton id="settings" icon={Settings} />
      </div>

    </div>
  );
};