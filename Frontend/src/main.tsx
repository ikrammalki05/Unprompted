<<<<<<< HEAD
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import './index.css'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
=======
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import './index.css';
import { keycloak } from './services/keycloak';

const root = ReactDOM.createRoot(document.getElementById('root')!);

// Keycloak intercepte l'utilisateur avant même de charger React
keycloak.init({
  // C'EST ICI QUE TOUT SE JOUE 👇
  onLoad: 'login-required', 
  pkceMethod: 'S256',
}).then((authenticated) => {
  
  if (authenticated) {
    // Si l'utilisateur est connecté, on lance l'application React
    root.render(
      <React.StrictMode>
        <App />
      </React.StrictMode>,
    );
  } else {
    // Théoriquement, on ne devrait jamais arriver ici avec 'login-required'
    // car Keycloak redirige automatiquement vers sa page de login.
    window.location.reload();
  }

}).catch(() => {
  console.error("Échec de l'initialisation de Keycloak");
  root.render(
    <div className="flex h-screen items-center justify-center bg-gray-50 text-red-600 font-bold">
      Serveur d'authentification injoignable.
    </div>
  );
});
>>>>>>> feature/etudiants-projets-cahier-charge
