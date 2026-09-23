import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import './index.css'
import App from './App.tsx'
import { keycloak } from './services/keycloak'

const root = createRoot(document.getElementById('root')!);

// Affichage d'un état de chargement initial
root.render(<div style={{ display: 'flex', height: '100vh', alignItems: 'center', justifyContent: 'center', fontFamily: 'sans-serif' }}>Initialisation de la session...</div>);

keycloak.init({
  onLoad: 'login-required',
  checkLoginIframe: false
}).then((authenticated) => {
  if (authenticated) {
    root.render(
      <StrictMode>
        <BrowserRouter>
          <App />
        </BrowserRouter>
      </StrictMode>,
    )
  } else {
    // Si pour une raison obscure on n'est pas authentifié malgré login-required
    window.location.reload();
  }
}).catch((err) => {
  console.error("Keycloak initialization failed", err);
  root.render(
    <div style={{ padding: '20px', color: 'red', textAlign: 'center' }}>
      <h1>Erreur de connexion</h1>
      <p>Impossible de communiquer avec le serveur d'authentification.</p>
      <button onClick={() => window.location.reload()}>Réessayer</button>
    </div>
  );
});
