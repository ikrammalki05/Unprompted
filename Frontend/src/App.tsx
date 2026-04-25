import { useState } from 'react';
import Sidebar from './components/Sidebar';
import Header from './components/Header';
import ConfigurationIA from './pages/ConfigurationIA';
import Evaluation from './pages/Evaluation';
import Profile from './pages/Profile';
import ProjetDetails from './pages/ProjetDetails';
import NouveauProjetModal from './components/NouveauProjetModal';
import TableauDeBord from './pages/TableauDeBord';
import GestionProjets from './pages/GestionProjets';

function App() {
  const [activePage, setActivePage] = useState('profile');
  const [modalOpen, setModalOpen] = useState(false);

  const renderPage = () => {
    switch (activePage) {
      case 'dashboard':
        return <TableauDeBord />;
      case 'projects':
        return <GestionProjets />;
      case 'config':
        return <ConfigurationIA />;
      case 'evaluation':
        return <Evaluation />;
      case 'profile':
        return <Profile />;
      case 'details':
        return <ProjetDetails />;
      default:
        return <TableauDeBord />;
    }
  };

  const getActivePath = () => {
    switch (activePage) {
      case 'dashboard':
        return ['Unprompted', 'Tableau de bord'];
      case 'projects':
        return ['Unprompted', 'Gestion des projets'];
      case 'config':
        return ['Unprompted', 'Configuration IA'];
      case 'evaluation':
        return ['Unprompted', 'Évaluation'];
      case 'profile':
        return ['Unprompted', 'Profil Enseignant'];
      case 'details':
        return ['Projets', 'Détails du projet'];
      default:
        return ['Unprompted', 'Tableau de bord'];
    }
  };

  return (
    <div className="flex min-h-screen bg-[#f8fafc] font-sans text-slate-900 overflow-x-hidden">
      <Sidebar activePage={activePage} onPageChange={setActivePage} onNewProject={() => setModalOpen(true)} />
      <div className="flex-1 ml-[300px] flex flex-col min-h-screen">
        <Header title={''} activePath={getActivePath()} />
        <main className="flex-1">
          {renderPage()}
        </main>
      </div>
      <NouveauProjetModal open={modalOpen} onClose={() => setModalOpen(false)} />
    </div>
  );
}

export default App;
