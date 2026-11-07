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
import MainLayout from './layouts/MainLayout';
import EtudiantLayout from './components/etudiant/Layout';
import { DashboardPage } from './pages/DashboardPage';
import { GestionPage } from './pages/GestionPage';
import { ProfilePage } from './pages/ProfilePage';
import EtudiantDashboard from './pages/etudiant/DashboardPage';
import EtudiantProfile from './pages/etudiant/ProfilePage';
import EtudiantHistory from './pages/etudiant/HistoryPage';
import CahierDesChargesPage from './pages/etudiant/CahierDesChargesPage';
import WorkspacePage from './pages/etudiant/workspace/WorkspacePage';

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

  const TeacherShell = () => (
    <div className="flex min-h-screen bg-gray-50 font-sans text-slate-900 overflow-x-hidden">
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

      <Route path="/enseignant/dashboard" element={<TeacherShell />} />

      <Route path="/admin" element={<MainLayout />}>
        <Route path="dashboard" element={<DashboardPage />} />
        <Route path="gestion" element={<GestionPage />} />
        <Route path="profile" element={<ProfilePage />} />
      </Route>

      {/* IDE plein écran (hors layout sidebar) */}
      <Route path="/etudiant/workspace/:projectId" element={<WorkspacePage />} />
      <Route path="/etudiant/workspace" element={<WorkspacePage />} />
      <Route path="/espace-travail" element={<Navigate to="/etudiant/workspace" replace />} />

      <Route path="/etudiant" element={<EtudiantLayout />}>
        <Route path="dashboard" element={<EtudiantDashboard />} />
        <Route path="historique" element={<EtudiantHistory />} />
        <Route path="profile" element={<EtudiantProfile />} />
        <Route path="profil" element={<Navigate to="/etudiant/profile" replace />} />
        <Route path="projet/:id/cahier-des-charges" element={<CahierDesChargesPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
}

export default App;
