import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import WorkspaceTopBar from './WorkspaceTopBar';
import WorkspaceSidebar from './WorkspaceSidebar';
import CodeEditor from './CodeEditor';
import AIAssistant from './AIAssistant';
import Terminal from './Terminal';
import workspaceService, { type TreeNode } from '../../../services/workspaceService';
import { api } from '../../../services/api';
import './workspace.css';

function findFirstFile(nodes: TreeNode[]): TreeNode | null {
  for (const n of nodes) {
    if (n.type === 'file') return n;
    if (n.children?.length) {
      const found = findFirstFile(n.children);
      if (found) return found;
    }
  }
  return null;
}

function collectFiles(
  nodes: TreeNode[],
  contents: Record<number, string>,
  prefix = ''
): { nom: string; contenu: string }[] {
  const out: { nom: string; contenu: string }[] = [];
  for (const n of nodes) {
    const path = prefix ? `${prefix}/${n.name}` : n.name;
    if (n.type === 'file') {
      out.push({ nom: path, contenu: contents[n.id] ?? '' });
    } else if (n.children) {
      out.push(...collectFiles(n.children, contents, path));
    }
  }
  return out;
}

const WorkspacePage: React.FC = () => {
  const { projectId: projectIdParam } = useParams();
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const projectId = Number(projectIdParam || searchParams.get('projectId') || 0);

  const [tree, setTree] = useState<TreeNode[]>([]);
  const [projectTitle, setProjectTitle] = useState('Projet');
  const [activeFileId, setActiveFileId] = useState<number | null>(null);
  const [activeFileName, setActiveFileName] = useState('');
  const [fileContents, setFileContents] = useState<Record<number, string>>({});
  const [dirty, setDirty] = useState<Record<number, boolean>>({});
  const [terminalOpen, setTerminalOpen] = useState(true);
  const [terminalLines, setTerminalLines] = useState<{ type: string; content: string }[]>([
    { type: 'info', content: 'Terminal prêt. Utilisez Lancer ou saisissez une commande.' },
  ]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [running, setRunning] = useState(false);
  const [assistantWidth, setAssistantWidth] = useState(() => {
    const saved = Number(localStorage.getItem('ws-assistant-width'));
    return Number.isFinite(saved) && saved >= 280 ? saved : 360;
  });
  const [resizingAssistant, setResizingAssistant] = useState(false);
  const [terminalSessionId, setTerminalSessionId] = useState<string | null>(null);

  const handleAssistantWidthChange = useCallback((width: number) => {
    setAssistantWidth(width);
    localStorage.setItem('ws-assistant-width', String(width));
  }, []);

  const loadTree = useCallback(async (id: number) => {
    const nodes = await workspaceService.getTree(id);
    setTree(nodes);
    return nodes;
  }, []);

  useEffect(() => {
    const init = async () => {
      try {
        setLoading(true);
        setError(null);

        let id = projectId;
        if (!id || Number.isNaN(id)) {
          const res = await api.get('/projet/etudiant/me');
          const list = Array.isArray(res.data) ? res.data : [];
          id = list[0]?.idProjet ?? list[0]?.IdProjet ?? 0;
          if (id) {
            navigate(`/etudiant/workspace/${id}`, { replace: true });
            return;
          }
          setError('Aucun projet assigné. Demandez à votre enseignant de vous assigner un projet.');
          return;
        }

        const projet = await workspaceService.getProject(id);
        setProjectTitle(projet.titre || projet.Titre || `Projet #${id}`);

        const nodes = await loadTree(id);
        const first = findFirstFile(nodes);
        if (first) {
          const file = await workspaceService.getFile(first.id);
          setActiveFileId(first.id);
          setActiveFileName(first.name);
          setFileContents((prev) => ({ ...prev, [first.id]: file.contenu ?? '' }));
        }
      } catch (e: any) {
        console.error(e);
        setError(e?.response?.data?.message || 'Impossible de charger le workspace.');
      } finally {
        setLoading(false);
      }
    };
    void init();
  }, [projectId, navigate, loadTree]);

  const openFile = async (node: TreeNode) => {
    if (node.type !== 'file') return;
    setActiveFileId(node.id);
    setActiveFileName(node.name);
    if (fileContents[node.id] !== undefined) return;
    const file = await workspaceService.getFile(node.id);
    setFileContents((prev) => ({ ...prev, [node.id]: file.contenu ?? '' }));
  };

  const onContentChange = (value: string) => {
    if (activeFileId == null) return;
    setFileContents((prev) => ({ ...prev, [activeFileId]: value }));
    setDirty((prev) => ({ ...prev, [activeFileId]: true }));
  };

  useEffect(() => {
    if (activeFileId == null || !dirty[activeFileId]) return;
    const content = fileContents[activeFileId] ?? '';
    const timer = setTimeout(async () => {
      try {
        setSaving(true);
        await workspaceService.autosave(activeFileId, content);
        setDirty((prev) => ({ ...prev, [activeFileId]: false }));
      } catch (e) {
        console.error(e);
      } finally {
        setSaving(false);
      }
    }, 800);
    return () => clearTimeout(timer);
  }, [fileContents, activeFileId, dirty]);

  const detectLanguage = useMemo(() => {
    if (activeFileName.endsWith('.py')) return 'python';
    if (activeFileName.endsWith('.js') || activeFileName.endsWith('.ts')) return 'javascript';
    if (activeFileName.endsWith('.cs')) return 'csharp';
    return 'python';
  }, [activeFileName]);

  const handleRun = async () => {
    if (!projectId) return;
    setRunning(true);
    setTerminalOpen(true);
    try {
      const fichiers = collectFiles(tree, fileContents);
      if (fichiers.length === 0) {
        setTerminalLines((prev) => [...prev, { type: 'error', content: 'Aucun fichier à exécuter.' }]);
        return;
      }
      setTerminalLines((prev) => [
        ...prev,
        { type: 'command', content: `run ${detectLanguage}` },
      ]);
      const started = await workspaceService.run(detectLanguage, fichiers);
      const status = await workspaceService.getExecution(started.idExecution);
      setTerminalLines((prev) => [
        ...prev,
        { type: 'output', content: status.sortie || `(statut: ${status.statut})` },
      ]);
    } catch (e: any) {
      setTerminalLines((prev) => [
        ...prev,
        { type: 'error', content: e?.response?.data?.message || e?.message || 'Échec exécution' },
      ]);
    } finally {
      setRunning(false);
    }
  };

  const handleCommit = async () => {
    if (!projectId) return;
    const message = window.prompt('Message de commit');
    if (!message?.trim()) return;
    try {
      await workspaceService.commit(projectId, message.trim());
      setTerminalLines((prev) => [
        ...prev,
        { type: 'info', content: `Commit enregistré: ${message.trim()}` },
      ]);
    } catch (e: any) {
      alert(e?.response?.data?.message || 'Échec du commit');
    }
  };

  const handleNewFile = async () => {
    if (!projectId) return;
    const raw = window.prompt('Nom du fichier (ex: utils.py)');
    if (!raw?.trim()) return;
    const name = raw.trim();
    const dot = name.lastIndexOf('.');
    const nom = dot > 0 ? name.slice(0, dot) : name;
    const extension = dot > 0 ? name.slice(dot) : '.txt';
    await workspaceService.createFile({
      nom,
      extension,
      contenu: '',
      idProjet: projectId,
      idDossier: null,
    });
    const nodes = await loadTree(projectId);
    const created = findFirstFile(nodes.filter((n) => n.name === name || n.name.startsWith(nom)));
    if (created) await openFile(created);
  };

  if (loading) {
    return <div className="ws-layout" style={{ color: '#94a3b8', padding: 24 }}>Chargement du workspace…</div>;
  }

  if (error) {
    return (
      <div className="ws-layout" style={{ color: '#fecaca', padding: 24 }}>
        <p>{error}</p>
        <button className="ws-btn" onClick={() => navigate('/etudiant/dashboard')}>
          Retour dashboard
        </button>
      </div>
    );
  }

  return (
    <div
      className={`ws-layout ${terminalOpen ? 'ws-terminal-open' : ''} ${resizingAssistant ? 'ws-resizing-assistant' : ''}`}
      style={{ ['--ws-assistant-width' as string]: `${assistantWidth}px` }}
    >
      <WorkspaceTopBar
        projectTitle={projectTitle}
        saving={saving}
        running={running}
        onRun={handleRun}
        onCommit={handleCommit}
        onToggleTerminal={() => setTerminalOpen((v) => !v)}
        onBack={() => navigate('/etudiant/dashboard')}
      />
      <WorkspaceSidebar
        tree={tree}
        activeFileId={activeFileId}
        projectTitle={projectTitle}
        onFileSelect={(node) => void openFile(node)}
        onNewFile={() => void handleNewFile()}
      />
      <CodeEditor
        fileName={activeFileName || '—'}
        content={activeFileId != null ? fileContents[activeFileId] ?? '' : ''}
        onChange={onContentChange}
      />
      <AIAssistant
        projectId={projectId}
        width={assistantWidth}
        onWidthChange={handleAssistantWidthChange}
        onResizeStart={() => setResizingAssistant(true)}
        onResizeEnd={() => setResizingAssistant(false)}
      />
      {terminalOpen && (
        <Terminal
          lines={terminalLines}
          onClose={() => {
            if (terminalSessionId) {
              void workspaceService.closeTerminalSession(terminalSessionId).catch(() => undefined);
              setTerminalSessionId(null);
            }
            setTerminalOpen(false);
          }}
          onCommand={async (cmd) => {
            setTerminalLines((prev) => [...prev, { type: 'command', content: cmd }]);
            try {
              let sessionId = terminalSessionId;
              if (!sessionId) {
                const session = await workspaceService.createTerminalSession(projectId || undefined);
                sessionId = session.idSession;
                setTerminalSessionId(sessionId);
              }
              const result = await workspaceService.runTerminalCommand(sessionId, cmd);
              const out = (result.sortie || '').trim();
              setTerminalLines((prev) => [
                ...prev,
                {
                  type: /fatal:|error:/i.test(out) ? 'error' : 'output',
                  content: out || '(ok)',
                },
              ]);
            } catch (e: any) {
              setTerminalLines((prev) => [
                ...prev,
                {
                  type: 'error',
                  content: e?.response?.data?.message || e?.message || 'Erreur terminal',
                },
              ]);
            }
          }}
        />
      )}
    </div>
  );
};

export default WorkspacePage;
