import React, { useState } from 'react';
import {
  Files, Search, GitBranch, Puzzle, Bot,
  ChevronDown, ChevronRight,
  Folder, FileText, File,
  HelpCircle, Settings,
  UserPlus
} from 'lucide-react';
import Logo from '../../../assets/Logo.png';

interface FileNode {
  name: string;
  type: 'file' | 'folder';
  icon?: string; // 'py' | 'md' | 'json' | 'default'
  children?: FileNode[];
}

const fileTree: FileNode[] = [
  {
    name: 'src',
    type: 'folder',
    children: [
      { name: 'main.py', type: 'file', icon: 'py' },
      { name: 'utils.py', type: 'file', icon: 'py' },
    ],
  },
  {
    name: 'data',
    type: 'folder',
    children: [],
  },
  { name: 'README.md', type: 'file', icon: 'md' },
];

interface SidebarNavItem {
  icon: React.ReactNode;
  label: string;
  id: string;
}

const navItems: SidebarNavItem[] = [
  { icon: <Files size={20} />, label: 'Fichiers', id: 'files' },
  { icon: <Search size={20} />, label: 'Rechercher', id: 'search' },
  { icon: <GitBranch size={20} />, label: 'Source Control', id: 'git' },
  { icon: <Puzzle size={20} />, label: 'Extensions', id: 'extensions' },
  { icon: <Bot size={20} />, label: 'AI Assistant', id: 'ai' },
];

interface Props {
  activeFile: string;
  onFileSelect: (fileName: string) => void;
}

const WorkspaceSidebar: React.FC<Props> = ({ activeFile, onFileSelect }) => {
  const [activeNav, setActiveNav] = useState('files');
  const [expandedFolders, setExpandedFolders] = useState<Set<string>>(new Set(['src']));

  const toggleFolder = (name: string) => {
    setExpandedFolders(prev => {
      const next = new Set(prev);
      if (next.has(name)) next.delete(name);
      else next.add(name);
      return next;
    });
  };

  const getFileIcon = (icon?: string) => {
    switch (icon) {
      case 'py': return <FileText size={15} />;
      case 'md': return <File size={15} />;
      default: return <File size={15} />;
    }
  };

  const getFileIconClass = (icon?: string) => {
    switch (icon) {
      case 'py': return 'file-icon-py';
      case 'md': return 'file-icon-md';
      default: return 'file-icon-default';
    }
  };

  const renderTree = (nodes: FileNode[], depth: number = 0) => {
    return nodes.map((node) => {
      if (node.type === 'folder') {
        const isExpanded = expandedFolders.has(node.name);
        return (
          <React.Fragment key={node.name}>
            <div
              className="ws-tree-item folder"
              onClick={() => toggleFolder(node.name)}
              style={{ paddingLeft: `${14 + depth * 16}px` }}
            >
              <span className="ws-tree-icon" style={{ color: '#64748b' }}>
                {isExpanded ? <ChevronDown size={14} /> : <ChevronRight size={14} />}
              </span>
              <span className="ws-tree-icon folder-icon">
                <Folder size={15} />
              </span>
              <span>{node.name}</span>
            </div>
            {isExpanded && node.children && renderTree(node.children, depth + 1)}
          </React.Fragment>
        );
      }

      return (
        <div
          key={node.name}
          className={`ws-tree-item ${activeFile === node.name ? 'active' : ''}`}
          onClick={() => onFileSelect(node.name)}
          style={{ paddingLeft: `${14 + (depth + 1) * 16}px` }}
        >
          <span className="ws-tree-indent" />
          <span className={`ws-tree-icon ${getFileIconClass(node.icon)}`}>
            {getFileIcon(node.icon)}
          </span>
          <span>{node.name}</span>
        </div>
      );
    });
  };

  return (
    <div className="ws-sidebar">
      {/* Icon navigation rail */}
      <div className="ws-sidebar-icons">
        {navItems.map((item) => (
          <button
            key={item.id}
            className={`ws-sidebar-icon ${activeNav === item.id ? 'active' : ''}`}
            onClick={() => setActiveNav(item.id)}
            title={item.label}
          >
            {item.icon}
          </button>
        ))}

        <div className="ws-sidebar-spacer" />

        <button className="ws-sidebar-icon" title="Aide">
          <HelpCircle size={20} />
        </button>
        <button className="ws-sidebar-icon" title="Paramètres">
          <Settings size={20} />
        </button>
      </div>

      {/* Explorer panel */}
      <div className="ws-explorer">
        {/* Project info */}
        <div className="ws-sidebar-project">
          <div className="ws-sidebar-project-info">
            <div className="ws-sidebar-project-avatar">
              <img src={Logo} alt="Unprompted" />
            </div>
            <div>
              <div className="ws-sidebar-project-name">Unprompted</div>
              <div className="ws-sidebar-project-version">Gouvernance v2.4</div>
            </div>
          </div>
        </div>

        {/* File tree header */}
        <div className="ws-explorer-header">
          <span>Explorateur</span>
          <button title="Nouveau fichier">
            <FileText size={14} />
          </button>
        </div>

        {/* File tree */}
        <div className="ws-file-tree">
          {renderTree(fileTree)}
        </div>

        {/* Bottom section */}
        <div className="ws-sidebar-bottom-section">
          <button className="ws-invite-btn">
            <UserPlus size={16} />
            Invite Member
          </button>

          <button className="ws-sidebar-bottom-item">
            <HelpCircle size={16} />
            <span>Help</span>
          </button>
          <button className="ws-sidebar-bottom-item">
            <Settings size={16} />
            <span>Settings</span>
          </button>
        </div>
      </div>
    </div>
  );
};

export default WorkspaceSidebar;
