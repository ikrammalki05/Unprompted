import { useState } from 'react';
import Sidebar from './components/Sidebar';
import GestionPage from './pages/GestionPage';
import ProfilePage from './pages/ProfilePage';

export default function App() {
  const [activeNav, setActiveNav] = useState('profile');

  return (
    <div className="flex min-h-screen bg-[var(--bg)]">
      <Sidebar activeNav={activeNav} setActiveNav={setActiveNav} />
      <main className="flex-1 overflow-y-auto min-w-0">
        {activeNav === 'users' && <GestionPage />}
        {activeNav === 'dashboard' && (
          <PlaceholderPage 
            title="Tableau de bord" 
            icon="📊" 
            desc="Vue d'ensemble des statistiques académiques." 
          />
        )}
        {activeNav === 'profile' && <ProfilePage />}
      </main>
    </div>
  );
}

interface PlaceholderProps {
  title: string;
  icon: string;
  desc: string;
}

function PlaceholderPage({ title, icon, desc }: PlaceholderProps) {
  return (
    <div className="flex flex-col items-center justify-center h-full min-h-[60vh] gap-4 p-10 text-center">
      <div className="text-6xl leading-none grayscale-[0.2]">{icon}</div>
      <h2 className="text-[22px] font-bold text-[var(--text-primary)]">{title}</h2>
      <p className="text-sm text-[var(--text-secondary)] max-w-[320px]">{desc}</p>
    </div>
  );
}
