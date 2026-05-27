import React, { useState } from 'react';
import WorkspaceTopBar from './WorkspaceTopBar';
import WorkspaceSidebar from './WorkspaceSidebar';
import CodeEditor from './CodeEditor';
import AIAssistant from './AIAssistant';
import Terminal from './Terminal';
import './workspace.css';

const WorkspacePage: React.FC = () => {
  const [activeFile, setActiveFile] = useState('main.py');
  const [terminalOpen, setTerminalOpen] = useState(true);

  return (
    <div className={`ws-layout ${terminalOpen ? 'ws-terminal-open' : ''}`}>
      <WorkspaceTopBar />
      <WorkspaceSidebar activeFile={activeFile} onFileSelect={setActiveFile} />
      <CodeEditor activeFile={activeFile} />
      <AIAssistant />
      {terminalOpen && <Terminal onClose={() => setTerminalOpen(false)} />}
    </div>
  );
};

export default WorkspacePage;
