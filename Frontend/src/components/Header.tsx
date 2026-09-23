import React from 'react';
import { Bell, Settings, Search } from 'lucide-react';

interface HeaderProps {
  title: string;
  activePath?: string[];
}

const Header: React.FC<HeaderProps> = ({ activePath = ["Unprompted"] }) => {
  return (
    <header className="h-20 bg-white border-b border-slate-100 flex items-center justify-between px-10 sticky top-0 z-10">
      {/* Breadcrumbs */}
      <div className="flex items-center gap-2 text-sm">
        {activePath.map((item, index) => (
          <React.Fragment key={item}>
            <span className={`font-bold ${index === activePath.length - 1 ? 'text-slate-900' : 'text-slate-400'}`}>
              {item}
            </span>
            {index < activePath.length - 1 && <span className="text-slate-300">/</span>}
          </React.Fragment>
        ))}
      </div>

      {/* Search Bar */}
      <div className="flex-1 max-w-md mx-10">
        <div className="relative group">
          <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400 group-focus-within:text-blue-500 transition-colors" />
          <input 
            type="text" 
            placeholder="Rechercher..." 
            className="w-full pl-11 pr-4 py-2.5 bg-slate-50 border border-slate-100 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all placeholder:text-slate-400"
          />
        </div>
      </div>
      
      <div className="flex items-center gap-6">
        <button className="relative p-2 text-slate-400 hover:text-slate-600 transition-colors">
          <Bell className="w-5 h-5" />
          <span className="absolute top-2 right-2 w-2 h-2 bg-red-500 rounded-full border-2 border-white"></span>
        </button>
        
        <button className="p-2 text-slate-400 hover:text-slate-600 transition-colors">
          <Settings className="w-5 h-5" />
        </button>
        
        <div className="flex items-center gap-4 pl-4 border-l border-slate-100 group cursor-pointer">
          <span className="text-sm font-bold text-slate-900 leading-tight">Mr Ghailani</span>
          <div className="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center text-blue-600 font-bold border-2 border-blue-50 group-hover:scale-105 transition-transform">
            G
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;
