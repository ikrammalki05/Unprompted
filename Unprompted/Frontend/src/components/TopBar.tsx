import React from 'react';
import { Search, Bell } from 'lucide-react';

const TopBar: React.FC = () => {
  return (
    <header className="sticky top-0 z-30 bg-white/80 backdrop-blur-md border-b border-gray-100">
      <div className="flex items-center justify-between px-8 py-3">
        {/* Search */}
        <div className="relative flex-1 max-w-md">
          <Search size={18} className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" />
          <input
            type="text"
            placeholder="Rechercher des projets, des rapports..."
            className="w-full bg-gray-50 border border-gray-100 rounded-xl pl-11 pr-4 py-2.5 text-sm text-gray-700 placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-100 focus:border-blue-200 transition-all"
          />
        </div>

        {/* Right side */}
        <div className="flex items-center gap-4 ml-6">
          {/* Notification */}
          <button className="relative p-2 rounded-xl hover:bg-gray-50 text-gray-500 hover:text-gray-700 transition-all">
            <Bell size={20} />
          </button>

          {/* Workspace link */}
          <button className="text-sm font-semibold text-blue-600 hover:text-blue-700 transition-colors px-3 py-1.5 rounded-lg hover:bg-blue-50">
            Espace Travail
          </button>

          {/* Avatar */}
          <div className="w-9 h-9 rounded-full bg-gradient-to-br from-blue-500 to-indigo-600 flex items-center justify-center text-white text-sm font-bold shadow-md shadow-blue-200 ring-2 ring-white cursor-pointer hover:shadow-lg transition-shadow">
            UP
          </div>
        </div>
      </div>
    </header>
  );
};

export default TopBar;
