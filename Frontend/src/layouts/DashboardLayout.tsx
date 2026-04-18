import React from 'react';
// Tu pourras importer tes propres icônes (ex: react-icons ou lucide-react)
import logo from '../assets/file.svg';

interface DashboardLayoutProps {
  children: React.ReactNode;
}

export const DashboardLayout = ({ children }: DashboardLayoutProps) => {
  return (
    <div className="flex h-screen w-full bg-[#f8f9fa] font-sans overflow-hidden">
      
      {/* ============================== */}
      {/* SIDEBAR (Barre latérale gauche) */}
      {/* ============================== */}
      <aside className="w-64 bg-white border-r border-gray-100 flex flex-col justify-between hidden md:flex">
        
        <div>
          {/* Logo & Titre */}
          <div className="w-20 h-20 rounded-full bg-white flex item-center justify-center p-3 ml-6 mt-2 shadow-xl">
          <img 
            src={logo} 
            alt="Logo Unprompted" 
            // h-16 définit la hauteur (tu peux tester h-12, h-20, etc. selon la taille de ton SVG)
            // w-auto permet de garder les bonnes proportions sans déformer l'image
            className="max-h-full w-auto object-contain" 
          />
        </div>

          {/* Menu de navigation */}
          <nav className="p-4 space-y-1 mt-4">
            {/* Lien Actif */}
            <a href="#" className="flex items-center gap-3 px-4 py-3 text-sm font-semibold text-[#0066cc] bg-blue-50 rounded-xl">
              <span>📊</span> Tableau de bord
            </a>
            {/* Liens Inactifs */}
            <a href="#" className="flex items-center gap-3 px-4 py-3 text-sm font-medium text-gray-600 hover:bg-gray-50 rounded-xl transition-colors">
              <span>📁</span> Projet
            </a>
            <a href="#" className="flex items-center gap-3 px-4 py-3 text-sm font-medium text-gray-600 hover:bg-gray-50 rounded-xl transition-colors">
              <span>👥</span> Espace de travail
            </a>
            <a href="#" className="flex items-center gap-3 px-4 py-3 text-sm font-medium text-gray-600 hover:bg-gray-50 rounded-xl transition-colors">
              <span>⏱️</span> Historique
            </a>
            <a href="#" className="flex items-center gap-3 px-4 py-3 text-sm font-medium text-gray-600 hover:bg-gray-50 rounded-xl transition-colors">
              <span>👤</span> Profil
            </a>
          </nav>
        </div>

        {/* Bas de la Sidebar (Paramètres & Profil) */}
        <div className="p-4 border-t border-gray-50">
          <a href="#" className="flex items-center gap-3 px-4 py-3 text-sm font-medium text-gray-600 hover:bg-gray-50 rounded-xl transition-colors mb-2">
            <span>⚙️</span> Paramètres
          </a>
          <div className="flex items-center gap-3 p-3 bg-gray-50 rounded-xl">
            <div className="w-8 h-8 rounded-full bg-[#021124] text-white flex items-center justify-center text-xs font-bold">AL</div>
            <div>
              <p className="text-xs font-bold text-gray-900">Alami</p>
              <p className="text-[10px] text-gray-500">Étudiant M2</p>
            </div>
          </div>
        </div>

      </aside>


      {/* ============================== */}
      {/* COLONNE DROITE (Topbar + Contenu) */}
      {/* ============================== */}
      <div className="flex-1 flex flex-col min-w-0">
        
        {/* TOPBAR */}
        <header className="h-16 bg-white border-b border-gray-100 flex items-center justify-between px-8 flex-none">
          {/* Recherche */}
          <div className="relative w-96">
            <span className="absolute left-3 top-2.5 text-gray-400 text-sm">🔍</span>
            <input 
              type="text" 
              placeholder="Rechercher des projets, des rapports..." 
              className="w-full pl-9 pr-4 py-2 bg-gray-50 border-none rounded-lg text-sm focus:ring-2 focus:ring-blue-100 outline-none"
            />
          </div>

          {/* Actions Topbar */}
          <div className="flex items-center gap-6">
            <button className="text-gray-400 hover:text-gray-600">🔔</button>
            <div className="flex items-center gap-3 border-l border-gray-200 pl-6">
              <span className="text-sm font-semibold text-[#0066cc]">Espace Travail</span>
              <div className="w-8 h-8 rounded-full bg-[#021124] text-white flex items-center justify-center text-xs font-bold cursor-pointer">UP</div>
            </div>
          </div>
        </header>

        {/* ZONE DE CONTENU SCROLLABLE */}
        {/* C'est ici que les pages vont s'injecter */}
        <main className="flex-1 overflow-y-auto p-8">
          <div className="max-w-6xl mx-auto">
            {children}
          </div>
        </main>

      </div>
    </div>
  );
};