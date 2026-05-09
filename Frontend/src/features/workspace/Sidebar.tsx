import React, { useState } from 'react';

// ─── Icônes SVG inline style VS Code Seti ────────────────────────────────────
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

// ─── Données ──────────────────────────────────────────────────────────────────
const initialData = [
  {
    id: 'folder-devcontainer', name: '.devcontainer', isFolder: true,
    children: [],
  },
  {
    id: 'folder-vscode', name: '.vscode', isFolder: true,
    children: [],
  },
  {
    id: 'folder-node', name: 'node_modules', isFolder: true, dimmed: true,
    children: [],
  },
  {
    id: 'folder-public', name: 'public', isFolder: true,
    children: [],
  },
  {
    id: 'folder-src', name: 'src', isFolder: true, defaultOpen: true,
    children: [
      { id: 'App.css',           name: 'App.css' },
      { id: 'App.jsx',           name: 'App.jsx', selected: true },
      { id: 'App.test.jsx',      name: 'App.test.jsx' },
      { id: 'index.css',         name: 'index.css' },
      { id: 'index.jsx',         name: 'index.jsx' },
      { id: 'logo.svg',          name: 'logo.svg' },
      { id: 'reportWebVitals.js',name: 'reportWebVitals.js' },
      { id: 'setupTests.js',     name: 'setupTests.js' },
    ],
  },
  { id: '.gitignore',        name: '.gitignore' },
  { id: 'index.html',        name: 'index.html' },
  { id: 'jsconfig.json',     name: 'jsconfig.json', badge: '1' },
  { id: 'LICENSE',           name: 'LICENSE' },
  { id: 'package-lock.json', name: 'package-lock.json' },
  { id: 'package.json',      name: 'package.json' },
  { id: 'README.md',         name: 'README.md' },
];

// ─── Sidebar ─────────────────────────────────────────────────────────────────
interface TreeNode {
  id: string; name: string; isFolder?: boolean; dimmed?: boolean;
  defaultOpen?: boolean; selected?: boolean; badge?: string;
  children?: TreeNode[];
}

interface SidebarProps { onFileSelect?: (id: string) => void; }

export const Sidebar: React.FC<SidebarProps> = ({ onFileSelect }) => {
  const [openFolders, setOpenFolders] = useState<Set<string>>(
    () => new Set(initialData.filter(n => (n as any).defaultOpen).map(n => n.id))
  );
  const [selected, setSelected] = useState<string>('App.jsx');
  const [collapsed, setCollapsed] = useState<Record<string, boolean>>({});

  const toggleFolder = (id: string) => {
    setOpenFolders(prev => {
      const next = new Set(prev);
      next.has(id) ? next.delete(id) : next.add(id);
      return next;
    });
  };

  const handleSelect = (id: string) => {
    setSelected(id);
    onFileSelect?.(id);
  };

  const renderNode = (node: TreeNode, depth = 0) => {
    const isOpen = openFolders.has(node.id);
    const isSelected = selected === node.id;
    const indent = depth * 16 + 8;

    return (
      <div key={node.id}>
        <div
          onClick={() => node.isFolder ? toggleFolder(node.id) : handleSelect(node.id)}
          style={{
            display: 'flex', alignItems: 'center',
            paddingLeft: indent, paddingRight: 8,
            height: 24, cursor: 'pointer', fontSize: 13,
            fontFamily: "'Segoe UI', system-ui, sans-serif",
            color: node.dimmed ? '#666' : isSelected ? '#ffffff' : '#cccccc',
            background: isSelected ? '#37373d' : 'transparent',
            userSelect: 'none', position: 'relative',
          }}
          onMouseEnter={e => { if (!isSelected) (e.currentTarget as HTMLElement).style.background = '#2a2d2e'; }}
          onMouseLeave={e => { if (!isSelected) (e.currentTarget as HTMLElement).style.background = 'transparent'; }}
        >
          {/* Arrow */}
          <span style={{
            width: 16, display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
            fontSize: 10, color: '#cccccc', flexShrink: 0,
          }}>
            {node.isFolder && (
              <span style={{
                display: 'inline-block',
                transform: isOpen ? 'rotate(90deg)' : 'rotate(0deg)',
                transition: 'transform 0.1s', fontSize: 10,
              }}>▶</span>
            )}
          </span>

          {/* Icon */}
          <FileIcon name={node.name} isFolder={!!node.isFolder} isOpen={isOpen} />

          {/* Name */}
          <span style={{ flex: 1, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
            {node.name}
          </span>

          {/* Badge */}
          {node.badge && (
            <span style={{
              background: 'transparent', color: '#cccccc',
              fontSize: 12, marginLeft: 4,
            }}>{node.badge}</span>
          )}
        </div>

        {/* Children */}
        {node.isFolder && isOpen && node.children?.map(child => renderNode(child, depth + 1))}
      </div>
    );
  };

  const sections = ['STRUCTURE', 'CHRONOLOGIE'];

  return (
    <div style={{
      width: 260, height: '100vh', background: '#252526',
      display: 'flex', flexDirection: 'column', overflow: 'hidden',
      fontFamily: "'Segoe UI', system-ui, sans-serif",
    }}>
      {/* Header */}
      <div style={{
        display: 'flex', alignItems: 'center', justifyContent: 'space-between',
        padding: '0 14px', height: 35,
        fontSize: 11, fontWeight: 700, letterSpacing: '0.08em',
        color: '#bbbbbb', textTransform: 'uppercase', flexShrink: 0,
      }}>
        <span>Explorateur</span>
        <span style={{ fontSize: 16, cursor: 'pointer', opacity: 0.6 }}>···</span>
      </div>

      {/* Project section header */}
      <div style={{
        display: 'flex', alignItems: 'center',
        padding: '4px 8px', fontSize: 11, fontWeight: 700,
        color: '#cccccc', textTransform: 'uppercase', letterSpacing: '0.05em',
        flexShrink: 0,
      }}>
        <span style={{ marginRight: 4, fontSize: 10 }}>▼</span>
        CODESPACES-REACT [CODESPACES: URBAN SPORK]
      </div>

      {/* Tree */}
      <div style={{ flex: 1, overflowY: 'auto', overflowX: 'hidden' }}
        className="vscode-scrollbar">
        {initialData.map(node => renderNode(node as TreeNode, 0))}
      </div>

      {/* Bottom collapsible sections */}
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
    </div>
  );
};

export default Sidebar;