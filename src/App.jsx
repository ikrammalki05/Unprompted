import React, { useState } from 'react';
import Sidebar from './components/Sidebar';
import GestionPage from './pages/GestionPage';
import ProfilePage from './pages/ProfilePage';
import './App.css';

export default function App() {
  const [activeNav, setActiveNav] = useState('profile'); // Default to profile as requested by the user's focus on it

  return (
    <div className="app-layout">
      <Sidebar activeNav={activeNav} setActiveNav={setActiveNav} />
      <main className="app-main">
        {activeNav === 'users' && <GestionPage />}
        {activeNav === 'dashboard' && <PlaceholderPage title="Tableau de bord" icon="📊" desc="Vue d'ensemble des statistiques académiques." />}
        {activeNav === 'profile' && <ProfilePage />}
      </main>
    </div>
  );
}

function PlaceholderPage({ title, icon, desc }) {
  return (
    <div className="placeholder-page">
      <div className="placeholder-icon">{icon}</div>
      <h2 className="placeholder-title">{title}</h2>
      <p className="placeholder-desc">{desc}</p>
    </div>
  );
}
