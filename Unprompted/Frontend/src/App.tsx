import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import DashboardPage from './pages/etudiant/DashboardPage';
import ProfilePage from './pages/etudiant/ProfilePage';
import CahierDesChargesPage from './pages/etudiant/CahierDesChargesPage';
import WorkspacePage from './pages/etudiant/workspace/WorkspacePage';
import HistoryPage from './pages/etudiant/HistoryPage';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Workspace has its own full-screen IDE layout */}
        <Route path="/espace-travail" element={<WorkspacePage />} />

        <Route element={<Layout />}>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/historique" element={<HistoryPage />} />
          <Route path="/profil" element={<ProfilePage />} />
          <Route path="/projet/:id/cahier-des-charges" element={<CahierDesChargesPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
