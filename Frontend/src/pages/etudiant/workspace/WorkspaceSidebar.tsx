import React, { useState } from 'react';
import { ChevronDown, ChevronRight, FileCode2, Folder, Plus } from 'lucide-react';
import type { TreeNode } from '../../../services/workspaceService';

interface Props {
  tree: TreeNode[];
  activeFileId: number | null;
  projectTitle?: string;
  onFileSelect: (node: TreeNode) => void;
  onNewFile: () => void;
}

const TreeItem: React.FC<{
  node: TreeNode;
  depth: number;
  activeFileId: number | null;
  onFileSelect: (node: TreeNode) => void;
}> = ({ node, depth, activeFileId, onFileSelect }) => {
  const [open, setOpen] = useState(true);
  const isFolder = node.type === 'folder';
  const active = !isFolder && node.id === activeFileId;

  return (
    <div>
      <button
        className={`ws-tree-item ${active ? 'active' : ''}`}
        style={{ paddingLeft: 8 + depth * 12 }}
        onClick={() => {
          if (isFolder) setOpen((v) => !v);
          else onFileSelect(node);
        }}
      >
        {isFolder ? (
          open ? <ChevronDown size={14} /> : <ChevronRight size={14} />
        ) : (
          <FileCode2 size={14} />
        )}
        {isFolder && <Folder size={14} />}
        <span>{node.name}</span>
      </button>
      {isFolder && open && node.children?.map((child) => (
        <TreeItem
          key={`${child.type}-${child.id}`}
          node={child}
          depth={depth + 1}
          activeFileId={activeFileId}
          onFileSelect={onFileSelect}
        />
      ))}
    </div>
  );
};

const WorkspaceSidebar: React.FC<Props> = ({
  tree,
  activeFileId,
  onFileSelect,
  onNewFile,
}) => {
  return (
    <aside className="ws-sidebar">
      <div className="ws-sidebar-header">
        <span className="ws-sidebar-title">Explorateur</span>
        <button type="button" className="ws-sidebar-add" onClick={onNewFile} title="Nouveau fichier">
          <Plus size={14} />
        </button>
      </div>
      <div className="ws-tree">
        {tree.length === 0 ? (
          <div className="ws-tree-empty">Aucun fichier. Créez-en un.</div>
        ) : (
          tree.map((node) => (
            <TreeItem
              key={`${node.type}-${node.id}`}
              node={node}
              depth={0}
              activeFileId={activeFileId}
              onFileSelect={onFileSelect}
            />
          ))
        )}
      </div>
    </aside>
  );
};

export default WorkspaceSidebar;
