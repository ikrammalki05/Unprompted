import React from 'react';
import { GitBranch, Settings, Bell, Play } from 'lucide-react';
import Logo from '../../../assets/Logo.png';

const WorkspaceTopBar: React.FC = () => {
  return (
    <div className="ws-topbar">
      {/* Left section */}
      <div className="ws-topbar-left">
        <div className="ws-topbar-brand">
          <img src={Logo} alt="Logo" />
          <span>Gouvernance IA</span>
        </div>

        <div className="ws-badge-active">
          Actif — En ligne
        </div>

        <div className="ws-branch">
          <GitBranch size={14} />
          <span>main</span>
        </div>
      </div>

      {/* Right section */}
      <div className="ws-topbar-actions">
        <button className="ws-btn">
          Commit
        </button>
        <button className="ws-btn">
          Push
        </button>
        <button className="ws-btn ws-btn-primary">
          <Play size={14} fill="currentColor" />
          Lancer
        </button>
        <button className="ws-icon-btn" title="Paramètres">
          <Settings size={18} />
        </button>
        <button className="ws-icon-btn" title="Notifications">
          <Bell size={18} />
        </button>
      </div>
    </div>
  );
};

export default WorkspaceTopBar;
