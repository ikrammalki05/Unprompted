import  { useState } from 'react';
import { Sidebar } from '../../features/workspace/Sidebar';
import { MonacoEditor } from '../../components/editor/MonacoEditor';
import { Terminal } from '../../features/workspace/Terminal';
import { ActivityBar } from '../../features/workspace/ActivityBar'; // <-- Nouvel import

const CodeEditorPage = () => {
  const [activeFileId, setActiveFileId] = useState<string>('App.tsx');
  
  // NOUVEAU : État pour savoir quel menu gauche est actif
  const [activeTab, setActiveTab] = useState<string>('explorer');
  // NOUVEAU : État pour savoir si la Sidebar (Explorateur) est ouverte ou repliée
  const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(true);

  // Fonction pour gérer le clic sur la barre d'activité
  const handleTabChange = (tabId: string) => {
    if (activeTab === tabId) {
      // Si on clique sur l'onglet déjà actif, on ferme/ouvre la sidebar
      setIsSidebarOpen(!isSidebarOpen);
    } else {
      // Sinon, on change d'onglet et on s'assure que la sidebar est ouverte
      setActiveTab(tabId);
      setIsSidebarOpen(true);
    }
  };

  return (
    <div className="flex h-screen w-screen overflow-hidden bg-[#1e1e1e] text-[#cccccc] font-sans">
      
      {/* 1. NOUVEAU : La Barre d'Activité tout à gauche */}
      <ActivityBar activeTab={activeTab} onTabChange={handleTabChange} />

      {/* 2. L'Explorateur (Sidebar) - Conditionnellement affiché */}
      {isSidebarOpen && (
        <aside className="w-[260px] shrink-0 border-r border-[#333333] flex flex-col bg-[#252526]">
          {/* Plus tard, tu pourras afficher <Search /> ou <Git /> ici en fonction de 'activeTab' */}
          {activeTab === 'explorer' && (
             <Sidebar onFileSelect={(id) => setActiveFileId(id)} />
          )}
        </aside>
      )}

      {/* 3. Zone de travail (Main) */}
      <main className="flex-1 flex flex-col min-w-0">
        
        {/* Onglets (Tabs) */}
        <div className="flex bg-[#2d2d2d] border-b border-[#1e1e1e]">
          <div className="px-4 py-2 bg-[#1e1e1e] text-[#ffffff] text-sm border-t-2 border-[#007acc]">
            {activeFileId}
          </div>
        </div>

        {/* L'Éditeur Monaco */}
        <div className="flex-1 relative min-h-0">
          <MonacoEditor 
            language="javascript" 
            value={`// Contenu du fichier ${activeFileId}`} 
            onChange={(val) => console.log(val)} 
          />
        </div>

        {/* Terminal en bas */}
        <section className="h-[250px] border-t border-[#333333] bg-[#1e1e1e] flex flex-col">
          <Terminal />
        </section>

      </main>
    </div>
  );
};

export default CodeEditorPage;