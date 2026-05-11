import React, { useState, useEffect, useCallback } from 'react';
import { Sidebar } from '../../features/workspace/Sidebar';
import { MonacoEditor } from '../../components/editor/MonacoEditor';
import { Terminal } from '../../features/workspace/Terminal';
import { ActivityBar } from '../../features/workspace/ActivityBar';
import { api } from '../../services/api';
import { X } from 'lucide-react';

// ─── Interface pour nos fichiers ouverts ─────────────────────────────────────
interface OpenFile {
  id: string;
  name: string;
  content: string;
  language: string;
  isDirty?: boolean; // 👈 NOUVEAU : Indique si le fichier a des modifications non sauvegardées
}

const getLanguageFromFileName = (fileName: string) => {
  const name = fileName.toLowerCase();
  if (name.endsWith('.cs')) return 'csharp';
  if (name.endsWith('.ts') || name.endsWith('.tsx')) return 'typescript';
  if (name.endsWith('.js') || name.endsWith('.jsx')) return 'javascript';
  if (name.endsWith('.json')) return 'json';
  if (name.endsWith('.css')) return 'css';
  if (name.endsWith('.html')) return 'html';
  if (name.endsWith('.md')) return 'markdown';
  return 'plaintext';
};

const CodeEditorPage = () => {
  const [activeTab, setActiveTab] = useState<string>('explorer');
  const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(true);

  const [openFiles, setOpenFiles] = useState<OpenFile[]>([]);
  const [activeFileId, setActiveFileId] = useState<string | null>(null);
  const [isLoadingFile, setIsLoadingFile] = useState<boolean>(false);
  const [isSaving, setIsSaving] = useState<boolean>(false); // 👈 Pour l'état de sauvegarde

  const handleTabChange = (tabId: string) => {
    if (activeTab === tabId) setIsSidebarOpen(!isSidebarOpen);
    else { setActiveTab(tabId); setIsSidebarOpen(true); }
  };

  // 1. Clic sur un fichier dans l'explorateur
  const handleFileSelect = async (nodeId: string) => {
    if (!nodeId.startsWith('file-')) return;
    const fileId = nodeId.split('-')[1];

    const existingFile = openFiles.find(f => f.id === fileId);
    if (existingFile) {
      setActiveFileId(fileId);
      return;
    }

    setIsLoadingFile(true);
    try {
      const response = await api.get(`/Fichier/${fileId}`);
      const data = response.data;
      const extension = data.extension?.startsWith('.') ? data.extension : `.${data.extension}`;
      const fullName = `${data.nom}${extension}`;

      const newFile: OpenFile = {
        id: fileId,
        name: fullName,
        content: data.contenu || '',
        language: getLanguageFromFileName(fullName),
        isDirty: false // Propre au chargement
      };

      setOpenFiles(prev => [...prev, newFile]);
      setActiveFileId(fileId);
    } catch (error) {
      console.error("Erreur lors du chargement :", error);
    } finally {
      setIsLoadingFile(false);
    }
  };

  // 2. Fermeture d'un onglet
  const handleCloseTab = (e: React.MouseEvent, idToClose: string) => {
    e.stopPropagation();
    
    // Alerte si on essaie de fermer un fichier non sauvegardé
    const fileToClose = openFiles.find(f => f.id === idToClose);
    if (fileToClose?.isDirty) {
      const confirmClose = window.confirm("Ce fichier contient des modifications non sauvegardées. Voulez-vous vraiment le fermer ?");
      if (!confirmClose) return;
    }

    const newOpenFiles = openFiles.filter(f => f.id !== idToClose);
    setOpenFiles(newOpenFiles);

    if (activeFileId === idToClose) {
      setActiveFileId(newOpenFiles.length > 0 ? newOpenFiles[newOpenFiles.length - 1].id : null);
    }
  };

  // 3. Quand on tape du code dans l'éditeur
  const handleEditorChange = (newValue: string | undefined) => {
    if (!activeFileId) return;
    
    setOpenFiles(prevFiles => 
      prevFiles.map(file => 
        // 👈 On passe isDirty à true dès qu'il y a une modification
        file.id === activeFileId ? { ...file, content: newValue || '', isDirty: true } : file
      )
    );
  };

  // ─── NOUVEAU : Fonction de Sauvegarde (avec userId) ─────────────────────────
  const handleSave = useCallback(async () => {
    if (!activeFileId) return;
    
    const currentFile = openFiles.find(f => f.id === activeFileId);
    if (!currentFile || !currentFile.isDirty) return;

    setIsSaving(true);
    try {
      // 💡 Ajout du paramètre userId dans l'URL
      // Tu pourras remplacer "1" par la variable de ton utilisateur connecté (via ton Context ou Redux)
      const userId = 1; 
      
      // 📡 Envoi avec ?userId=1
      await api.put(`/Fichier/${activeFileId}?userId=${userId}`, { 
        contenu: currentFile.content 
      });

      // On repasse isDirty à false car c'est sauvegardé
      setOpenFiles(prevFiles => 
        prevFiles.map(file => 
          file.id === activeFileId ? { ...file, isDirty: false } : file
        )
      );
      
      console.log(`${currentFile.name} sauvegardé avec succès !`);
    } catch (error) {
      console.error("Erreur lors de la sauvegarde :", error);
      alert("Échec de la sauvegarde.");
    } finally {
      setIsSaving(false);
    }
  }, [activeFileId, openFiles]);

  // ─── NOUVEAU : Intercepteur de Raccourci Clavier (Ctrl+S) ──────────────────
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      // Vérifie si Ctrl (Windows) ou Cmd (Mac) + 's' ou 'S' est pressé
      if ((e.ctrlKey || e.metaKey) && e.key.toLowerCase() === 's') {
        e.preventDefault(); // Bloque la fenêtre d'enregistrement "Enregistrer sous..." du navigateur
        handleSave();       // Appelle notre fonction de sauvegarde
      }
    };

    document.addEventListener('keydown', handleKeyDown);
    return () => document.removeEventListener('keydown', handleKeyDown);
  }, [handleSave]); // Se met à jour si la fonction handleSave change

  const activeFile = openFiles.find(f => f.id === activeFileId);

  return (
    <div className="flex h-screen w-screen overflow-hidden bg-[#1e1e1e] text-[#cccccc] font-sans">
      <ActivityBar activeTab={activeTab} onTabChange={handleTabChange} />

      {isSidebarOpen && (
        <aside className="w-[260px] shrink-0 border-r border-[#333333] flex flex-col bg-[#252526]">
          {activeTab === 'explorer' && (
             <Sidebar onFileSelect={handleFileSelect} />
          )}
        </aside>
      )}

      <main className="flex-1 flex flex-col min-w-0 relative bg-[#1e1e1e]">
        
        {/* ─── Barre des Onglets ─── */}
        <div className="flex bg-[#252526] overflow-x-auto vscode-scrollbar min-h-[35px]">
          {openFiles.map(file => {
            const isActive = file.id === activeFileId;
            return (
              <div 
                key={file.id}
                onClick={() => setActiveFileId(file.id)}
                className={`flex items-center gap-2 px-3 py-2 text-[13px] cursor-pointer border-r border-[#1e1e1e] group select-none min-w-fit
                  ${isActive 
                    ? 'bg-[#1e1e1e] text-[#ffffff] border-t-[1px] border-t-[#007acc]' 
                    : 'bg-[#2d2d2d] text-[#969696] border-t-[1px] border-t-transparent hover:bg-[#2b2b2b]'
                  }`}
              >
                <span className={file.isDirty ? 'italic' : ''}>{file.name}</span>
                
                {/* 👈 NOUVEAU : Le petit point blanc (Dot) VS Code si non sauvegardé */}
                {file.isDirty && !isActive && (
                  <div className="w-2 h-2 rounded-full bg-white opacity-50"></div>
                )}
                {file.isDirty && isActive && (
                  <div className="w-2 h-2 rounded-full bg-white"></div>
                )}

                {/* Bouton de fermeture */}
                {!file.isDirty && (
                  <div 
                    onClick={(e) => handleCloseTab(e, file.id)}
                    className={`p-0.5 rounded-md hover:bg-[#454545] ${isActive ? 'opacity-100' : 'opacity-0 group-hover:opacity-100'}`}
                  >
                    <X size={14} />
                  </div>
                )}
              </div>
            );
          })}
        </div>

        {/* ─── L'Éditeur Monaco ─── */}
        <div className="flex-1 relative min-h-0 bg-[#1e1e1e]">
          {isLoadingFile ? (
            <div className="flex items-center justify-center h-full text-gray-500">
              Chargement du fichier...
            </div>
          ) : activeFile ? (
            <>
              {/* Petit indicateur de sauvegarde en haut à droite */}
              {isSaving && (
                <div className="absolute top-2 right-4 z-10 text-xs text-[#007acc] bg-[#1e1e1e] px-2 py-1 rounded border border-[#333333]">
                  Sauvegarde en cours...
                </div>
              )}
              <MonacoEditor 
                language={activeFile.language} 
                value={activeFile.content} 
                onChange={handleEditorChange} 
              />
            </>
          ) : (
            <div className="flex items-center justify-center h-full text-gray-500 text-sm">
              Sélectionnez un fichier pour commencer à coder (Ctrl+P pour chercher).
            </div>
          )}
        </div>

        {/* ─── Terminal ─── */}
        <section className="h-[250px] border-t border-[#333333] bg-[#1e1e1e] flex flex-col">
          <Terminal />
        </section>

      </main>
    </div>
  );
};

export default CodeEditorPage;