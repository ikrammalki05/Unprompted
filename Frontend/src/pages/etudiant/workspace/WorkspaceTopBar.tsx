import React from 'react';
import { GitBranch, Play, TerminalSquare, ArrowLeft } from 'lucide-react';
import Logo from '../../../assets/Logo.png';

interface Props {
  projectTitle: string;
  saving?: boolean;
  running?: boolean;
  onRun: () => void;
  onCommit: () => void;
  onToggleTerminal: () => void;
  onBack: () => void;
}

const WorkspaceTopBar: React.FC<Props> = ({
  projectTitle,
  saving,
  running,
  onRun,
  onCommit,
  onToggleTerminal,
  onBack,
}) => {
  return (
    <header className="ws-topbar">
      <div className="ws-topbar-left">
        <button className="ws-btn" onClick={onBack} title="Retour">
          <ArrowLeft size={14} />
        </button>
        <div className="ws-topbar-brand">
          <img src={Logo} alt="Unprompted" />
          <span>{projectTitle}</span>
        </div>
        <div className="ws-badge-active">{saving ? 'Enregistrement…' : 'Connecté'}</div>
        <div className="ws-branch">
          <GitBranch size={14} />
          <span>workspace</span>
        </div>
      </div>
      <div className="ws-topbar-right" style={{ display: 'flex', gap: 8 }}>
        <button className="ws-btn" onClick={onToggleTerminal} title="Terminal">
          <TerminalSquare size={14} />
        </button>
        <button className="ws-btn" onClick={onCommit}>
          Commit
        </button>
        <button className="ws-btn ws-btn-primary" onClick={onRun} disabled={running}>
          <Play size={14} fill="currentColor" />
          {running ? 'Exécution…' : 'Lancer'}
        </button>
      </div>
    </header>
  );
};

export default WorkspaceTopBar;
