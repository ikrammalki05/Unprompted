import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import DashboardPage from './pages/DashboardPage';
import ProfilePage from './pages/ProfilePage';
import CahierDesChargesPage from './pages/CahierDesChargesPage';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/profil" element={<ProfilePage />} />
          <Route path="/projet/:id/cahier-des-charges" element={<CahierDesChargesPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
