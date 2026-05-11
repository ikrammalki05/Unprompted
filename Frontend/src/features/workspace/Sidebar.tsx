import React, { useState, useEffect } from 'react';
import { api } from "../../services/api"; // Assure-toi que ce chemin correspond à ton projet

// ─── Icônes ──────────────────────────────────────────────────────────────────
const icons: Record<string, { color: string; symbol: string }> = {
  css:    { color: '#519aba', symbol: '#' },
  jsx:    { color: '#61dafb', symbol: '⚛' },
  tsx:    { color: '#61dafb', symbol: '⚛' },
  js:     { color: '#e8c84d', symbol: 'JS' },
  ts:     { color: '#519aba', symbol: 'TS' },
  json:   { color: '#e8c84d', symbol: '{}' },
  md:     { color: '#519aba', symbol: 'MD' },
  svg:    { color: '#e37933', symbol: '◈' },
  html:   { color: '#e37933', symbol: '</>' },
  folder: { color: '#dcb862', symbol: '▶' },
  default:{ color: '#cccccc', symbol: '·' },
};

function getExt(name: string) {
  const parts = name.split('.');
  return parts.length > 1 ? parts[parts.length - 1].toLowerCase() : 'default';
}

function FileIcon({ name, isFolder, isOpen }: { name: string; isFolder: boolean; isOpen?: boolean }) {
  if (isFolder) {
    return (
      <span style={{ color: '#dcb862', fontSize: 13, fontWeight: 700, marginRight: 4 }}>
        {isOpen ? '📂' : '📁'}
      </span>
    );
  }
  const ext = getExt(name);
  const icon = icons[ext] || icons.default;
  return (
    <span style={{
      color: icon.color,
      fontSize: ext === 'jsx' || ext === 'tsx' ? 14 : 11,
      fontWeight: 700,
      marginRight: 4,
      fontFamily: 'monospace',
      minWidth: 16,
      display: 'inline-block',
      textAlign: 'center',
    }}>
      {icon.symbol}
    </span>
  );
}

// ─── Interfaces ──────────────────────────────────────────────────────────────
interface TreeNode {
  id: string; 
  name: string; 
  isFolder: boolean; 
  children?: TreeNode[];
  dimmed?: boolean;
}

interface SidebarProps { 
  onFileSelect?: (id: string) => void; 
}

interface ContextMenuState {
  x: number;
  y: number;
  nodeId: string;
  isFolder: boolean;
}

// ─── Composant Principal ─────────────────────────────────────────────────────
export const Sidebar: React.FC<SidebarProps> = ({ onFileSelect }) => {
  const [treeData, setTreeData] = useState<TreeNode[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [openFolders, setOpenFolders] = useState<Set<string>>(new Set());
  const [selected, setSelected] = useState<string>('');
  
  // État pour les sections rétractables du bas
  const [collapsed, setCollapsed] = useState<Record<string, boolean>>({});
  
  // État du menu contextuel
  const [contextMenu, setContextMenu] = useState<ContextMenuState | null>(null);

  // Fonction pour transformer le JSON C# imbriqué en format TreeNode
  const mapBackendData = (data: any[]): TreeNode[] => {
    // 1. Filtrer pour ne garder que les dossiers racines
    const roots = data.filter(item => item.dossierParentId === null);

    // 2. Fonction récursive pour traiter les dossiers et les fichiers
    const mapItem = (item: any, isFolder: boolean): TreeNode => {
      if (isFolder) {
        const subFolders = (item.sousDossiers || []).map((sf: any) => mapItem(sf, true));
        const files = (item.fichiers || []).map((f: any) => mapItem(f, false));

        return {
          id: `folder-${item.id}`,
          name: item.nom,
          isFolder: true,
          children: [...subFolders, ...files]
        };
      } else {
        const extension = item.extension ? (item.extension.startsWith('.') ? item.extension : `.${item.extension}`) : '';
        return {
          id: `file-${item.id}`,
          name: `${item.nom}${extension}`,
          isFolder: false
        };
      }
    };

    return roots.map(root => mapItem(root, true));
  };

  // Fonction pour charger les données depuis l'API
  const fetchTreeData = async () => {
    try {
      setIsLoading(true);
      const response = await api.get('/Dossier/projet/1');
      const formattedTree = mapBackendData(response.data);
      setTreeData(formattedTree);
      
      // Optionnel : Ouvrir le premier dossier par défaut
      if (formattedTree.length > 0) {
        setOpenFolders(new Set([formattedTree[0].id]));
      }
    } catch (error) {
      console.error("Erreur lors de la récupération des dossiers :", error);
    } finally {
      setIsLoading(false);
    }
  };

  // Chargement initial
  useEffect(() => {
    fetchTreeData();
  }, []);

  // Fermer le menu contextuel si on clique ailleurs
  useEffect(() => {
    const closeMenu = () => setContextMenu(null);
    window.addEventListener('click', closeMenu);
    return () => window.removeEventListener('click', closeMenu);
  }, []);

  // Ouvrir/Fermer un dossier
  const toggleFolder = (id: string) => {
    setOpenFolders(prev => {
      const next = new Set(prev);
      next.has(id) ? next.delete(id) : next.add(id);
      return next;
    });
  };

  // Sélectionner un fichier
  const handleSelect = (id: string) => {
    setSelected(id);
    onFileSelect?.(id); // Informe le composant parent (ex: pour ouvrir Monaco Editor)
  };

  // Actions CRUD du menu contextuel
  const handleMenuAction = async (action: 'createFolder' | 'createFile' | 'rename' | 'delete') => {
    if (!contextMenu) return;
    const { nodeId, isFolder } = contextMenu;
    const cleanId = nodeId.split('-')[1]; // Extrait l'ID numérique du backend

    try {
      if (action === 'createFolder') {
        const nom = prompt("Nom du nouveau dossier :");
        if (nom) {
          await api.post('/Dossier', { nom, dossierParentId: cleanId, idProjet: 1 });
          fetchTreeData(); // Rafraîchit l'arbre
        }
      } 
      else if (action === 'createFile') {
        const nomComplet = prompt("Nom du fichier (ex: App.tsx) :");
        if (nomComplet) {
          const parts = nomComplet.split('.');
          const ext = parts.length > 1 ? `.${parts.pop()}` : '';
          const nom = parts.join('.');
          await api.post('/Fichier', { 
            nom: nom, 
            extension: ext, 
            contenu: "", 
            taille: 0, 
            idProjet: 1, 
            idDossier: cleanId 
          });
          fetchTreeData();
        }
      }
      else if (action === 'rename') {
        const nouveauNom = prompt("Nouveau nom :");
        if (nouveauNom) {
          const url = isFolder ? `/Dossier/${cleanId}/rename` : `/Fichier/${cleanId}/rename`;
          await api.put(url, { nom: nouveauNom });
          fetchTreeData();
        }
      } 
      else if (action === 'delete') {
        if (window.confirm("Êtes-vous sûr de vouloir supprimer cet élément ?")) {
          const url = isFolder ? `/Dossier/${cleanId}` : `/Fichier/${cleanId}`;
          await api.delete(url);
          fetchTreeData();
        }
      }
    } catch (error) {
      console.error("Erreur API:", error);
      alert("Une erreur est survenue avec le backend.");
    }
  };

  // Moteur de rendu récursif de l'arbre
  const renderNode = (node: TreeNode, depth = 0) => {
    const isOpen = openFolders.has(node.id);
    const isSelected = selected === node.id;
    const indent = depth * 16 + 8;

    return (
      <div key={node.id}>
        <div
          onClick={() => node.isFolder ? toggleFolder(node.id) : handleSelect(node.id)}
          onContextMenu={(e) => {
            e.preventDefault();
            e.stopPropagation();
            setContextMenu({ x: e.clientX, y: e.clientY, nodeId: node.id, isFolder: node.isFolder });
          }}
          style={{
            display: 'flex', alignItems: 'center', paddingLeft: indent, paddingRight: 8,
            height: 24, cursor: 'pointer', fontSize: 13, 
            color: node.dimmed ? '#666' : isSelected ? '#ffffff' : '#cccccc',
            background: isSelected ? '#37373d' : 'transparent',
            userSelect: 'none', position: 'relative',
          }}
          onMouseEnter={e => { if (!isSelected) (e.currentTarget as HTMLElement).style.background = '#2a2d2e'; }}
          onMouseLeave={e => { if (!isSelected) (e.currentTarget as HTMLElement).style.background = 'transparent'; }}
        >
          {/* Flèche de dossier */}
          <span style={{ width: 16, display: 'inline-flex', justifyContent: 'center', fontSize: 10, flexShrink: 0 }}>
            {node.isFolder && (
              <span style={{ transform: isOpen ? 'rotate(90deg)' : 'rotate(0deg)', transition: 'transform 0.1s' }}>▶</span>
            )}
          </span>
          
          {/* Icône du fichier/dossier */}
          <FileIcon name={node.name} isFolder={node.isFolder} isOpen={isOpen} />
          
          {/* Nom de l'élément */}
          <span style={{ flex: 1, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
            {node.name}
          </span>
        </div>
        
        {/* Enfants récursifs */}
        {node.isFolder && isOpen && node.children?.map(child => renderNode(child, depth + 1))}
      </div>
    );
  };

  const sections = ['STRUCTURE', 'CHRONOLOGIE'];

  return (
    <div style={{ width: 260, height: '100vh', background: '#252526', display: 'flex', flexDirection: 'column', overflow: 'hidden', fontFamily: "'Segoe UI', system-ui, sans-serif" }}>
      
      {/* En-tête de l'explorateur */}
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', padding: '0 14px', height: 35, fontSize: 11, fontWeight: 700, letterSpacing: '0.08em', color: '#bbbbbb', textTransform: 'uppercase', flexShrink: 0 }}>
        <span>Explorateur</span>
        <span style={{ fontSize: 16, cursor: 'pointer', opacity: 0.6 }}>···</span>
      </div>

      {/* Titre du projet en cours */}
      <div style={{ display: 'flex', alignItems: 'center', padding: '4px 8px', fontSize: 11, fontWeight: 700, color: '#cccccc', textTransform: 'uppercase', letterSpacing: '0.05em', flexShrink: 0 }}>
        <span style={{ marginRight: 4, fontSize: 10 }}>▼</span>
        UNPROMPTED [WORKSPACE]
      </div>

      {/* Arborescence principale */}
      <div style={{ flex: 1, overflowY: 'auto', overflowX: 'hidden' }} className="vscode-scrollbar">
        {isLoading ? (
          <div style={{ padding: '20px', color: '#888', fontSize: '12px', textAlign: 'center' }}>
            Chargement...
          </div>
        ) : treeData.length === 0 ? (
          <div style={{ padding: '20px', color: '#888', fontSize: '12px', textAlign: 'center' }}>
            Dossier vide.
          </div>
        ) : (
          treeData.map(node => renderNode(node, 0))
        )}
      </div>

      {/* Sections rétractables en bas (Structure, Chronologie, etc.) */}
      {sections.map(section => (
        <div key={section} style={{ flexShrink: 0, borderTop: '1px solid #3c3c3c' }}>
          <div
            onClick={() => setCollapsed(c => ({ ...c, [section]: !c[section] }))}
            style={{
              display: 'flex', alignItems: 'center',
              padding: '4px 8px', height: 28, cursor: 'pointer',
              fontSize: 11, fontWeight: 700, color: '#cccccc',
              textTransform: 'uppercase', letterSpacing: '0.05em',
              userSelect: 'none',
            }}
          >
            <span style={{
              marginRight: 4, fontSize: 10,
              transform: collapsed[section] ? 'rotate(-90deg)' : 'rotate(0deg)',
              display: 'inline-block', transition: 'transform 0.1s',
            }}>▼</span>
            {section}
          </div>
        </div>
      ))}

      {/* Menu Contextuel (Dessiné au-dessus de tout) */}
      {contextMenu && (
        <div style={{
          position: 'fixed', top: contextMenu.y, left: contextMenu.x, zIndex: 9999,
          background: '#252526', border: '1px solid #454545', boxShadow: '0 4px 10px rgba(0,0,0,0.5)',
          padding: '4px 0', minWidth: '180px', borderRadius: '4px',
          color: '#cccccc', fontSize: '13px', fontFamily: "'Segoe UI', system-ui, sans-serif"
        }}>
          {/* Si c'est un dossier, on offre la création */}
          {contextMenu.isFolder && (
            <>
              <div 
                onClick={() => handleMenuAction('createFile')}
                onMouseEnter={e => (e.currentTarget as HTMLElement).style.background = '#04395e'}
                onMouseLeave={e => (e.currentTarget as HTMLElement).style.background = 'transparent'}
                style={{ padding: '6px 20px', cursor: 'pointer' }}
              >
                Nouveau Fichier...
              </div>
              <div 
                onClick={() => handleMenuAction('createFolder')}
                onMouseEnter={e => (e.currentTarget as HTMLElement).style.background = '#04395e'}
                onMouseLeave={e => (e.currentTarget as HTMLElement).style.background = 'transparent'}
                style={{ padding: '6px 20px', cursor: 'pointer' }}
              >
                Nouveau Dossier...
              </div>
              <div style={{ height: 1, background: '#454545', margin: '4px 0' }} />
            </>
          )}
          
          <div 
            onClick={() => handleMenuAction('rename')}
            onMouseEnter={e => (e.currentTarget as HTMLElement).style.background = '#04395e'}
            onMouseLeave={e => (e.currentTarget as HTMLElement).style.background = 'transparent'}
            style={{ padding: '6px 20px', cursor: 'pointer' }}
          >
            Renommer...
          </div>
          
          <div 
            onClick={() => handleMenuAction('delete')}
            onMouseEnter={e => (e.currentTarget as HTMLElement).style.background = '#f14c4c'}
            onMouseLeave={e => (e.currentTarget as HTMLElement).style.background = 'transparent'}
            style={{ padding: '6px 20px', cursor: 'pointer' }}
          >
            Supprimer
          </div>
        </div>
      )}

    </div>
  );
};

export default Sidebar;