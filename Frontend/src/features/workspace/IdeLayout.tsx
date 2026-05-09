import  { useState } from 'react';
import { MonacoEditor } from '../../components/editor/MonacoEditor';
import './IdeLayout.css'; // Ou utilise Tailwind si tu l'as configuré

export const IdeLayout = () => {
  const [code, setCode] = useState<string>(
`import './App.css';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <p>GitHub Codespaces ❤️ React</p>
      </header>
    </div>
  );
}

export default App;`
  );

  const handleCodeChange = (newValue: string | undefined) => {
    if (newValue !== undefined) {
      setCode(newValue);
    }
  };

  return (
    <div className="ide-container" style={{ display: 'flex', height: '100vh', backgroundColor: '#1e1e1e', color: 'white' }}>
      
      {/* Barre latérale (Explorateur de fichiers simulé) */}
      <aside style={{ width: '250px', borderRight: '1px solid #333', padding: '10px' }}>
        <h3 style={{ fontSize: '12px', textTransform: 'uppercase', color: '#ccc' }}>Explorateur</h3>
        <ul style={{ listStyleType: 'none', padding: 0, marginTop: '10px', fontSize: '14px' }}>
          <li style={{ cursor: 'pointer', backgroundColor: '#37373d', padding: '4px' }}>📄 App.jsx</li>
          <li style={{ cursor: 'pointer', padding: '4px' }}>📄 index.css</li>
          <li style={{ cursor: 'pointer', padding: '4px' }}>📄 package.json</li>
        </ul>
      </aside>

      {/* Zone principale (Onglets + Éditeur + Terminal simulé) */}
      <main style={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
        
        {/* Barre d'onglets */}
        <div style={{ display: 'flex', backgroundColor: '#2d2d2d', borderBottom: '1px solid #1e1e1e' }}>
          <div style={{ padding: '8px 16px', backgroundColor: '#1e1e1e', borderTop: '2px solid #007acc' }}>
            App.jsx
          </div>
        </div>

        {/* L'éditeur Monaco */}
        <div style={{ flex: 1 }}>
          <MonacoEditor 
            language="javascript" 
            value={code} 
            onChange={handleCodeChange} 
          />
        </div>

        {/* Terminal simulé en bas */}
        <div style={{ height: '200px', borderTop: '1px solid #333', padding: '10px', fontFamily: 'monospace' }}>
          <div style={{ display: 'flex', gap: '15px', borderBottom: '1px solid #333', paddingBottom: '5px', marginBottom: '10px' }}>
            <span style={{ cursor: 'pointer' }}>PROBLÈMES</span>
            <span style={{ cursor: 'pointer', color: '#fff', borderBottom: '1px solid #007acc' }}>TERMINAL</span>
          </div>
          <div style={{ color: '#00ff00' }}>➜ /workspaces/unprompted (main) $ npm start</div>
        </div>

      </main>
    </div>
  );
};