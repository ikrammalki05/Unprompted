import { useState } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Sidebar from './components/Sidebar';
import Header from './components/Header';
import ConfigurationIA from './pages/ConfigurationIA';
import Evaluation from './pages/Evaluation';
import Profile from './pages/Profile';
import ProjetDetails from './pages/ProjetDetails';
import NouveauProjetModal from './components/NouveauProjetModal';
import TableauDeBord from './pages/TableauDeBord';
import GestionProjets from './pages/GestionProjets';
import { RoleRedirect } from './features/auth/RoleRedirect';
import { UnauthorizedPage } from './features/auth/UnauthorizedPage';

function App() {
  const [activePage, setActivePage] = useState('dashboard');
  const [selectedProjetId, setSelectedProjetId] = useState<number | null>(null);
  const [modalOpen, setModalOpen] = useState(false);

  const handleProjectSelect = (id: number) => {
    setSelectedProjetId(id);
    setActivePage('details');
  };

  const renderPage = () => {
    switch (activePage) {
      case 'dashboard':
        return <TableauDeBord onProjectClick={handleProjectSelect} />;
      case 'projects':
        return <GestionProjets />;
      case 'config':
        return <ConfigurationIA />;
      case 'evaluation':
        return <Evaluation />;
      case 'profile':
        return <Profile />;
      case 'details':
        return <ProjetDetails id={selectedProjetId} />;
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

  const MainLayout = () => (
    <div className="flex min-h-screen bg-[#f8fafc] font-sans text-slate-900 overflow-x-hidden">
      <Sidebar activePage={activePage} onPageChange={setActivePage} />
      <div className="flex-1 ml-[300px] flex flex-col min-h-screen">
        <Header title={''} activePath={getActivePath()} />
        <main className="flex-1">
          {renderPage()}
        </main>
      </div>
      <NouveauProjetModal open={modalOpen} onClose={() => setModalOpen(false)} />
    </div>
  );

  return (
    <Routes>
      <Route path="/" element={<RoleRedirect />} />
      <Route path="/unauthorized" element={<UnauthorizedPage />} />
      <Route path="/enseignant/dashboard" element={<MainLayout />} />
      {/* Fallback pour les anciennes URLs ou redirections */}
      <Route path="/admin/dashboard" element={<Navigate to="/unauthorized" />} />
      <Route path="/etudiant/dashboard" element={<Navigate to="/unauthorized" />} />
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
}

export default App;

