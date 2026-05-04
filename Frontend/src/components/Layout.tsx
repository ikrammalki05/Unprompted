import React from 'react';
import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import TopBar from './TopBar';

const Layout: React.FC = () => {
  return (
    <div className="flex min-h-screen bg-gray-50/80">
      <Sidebar />
      <div className="flex-1 flex flex-col min-h-screen">
        <TopBar />
        <main className="flex-1 overflow-auto">
          <Outlet />
        </main>
        {/* Footer */}
        <footer className="border-t border-gray-100 bg-white/60 backdrop-blur-sm py-4 px-8">
          <p className="text-center text-xs text-gray-400">
            © 2024 Unprompted Platform — Plateforme de Gouvernance Académique de l'IA
          </p>
        </footer>
      </div>
    </div>
  );
};

export default Layout;
