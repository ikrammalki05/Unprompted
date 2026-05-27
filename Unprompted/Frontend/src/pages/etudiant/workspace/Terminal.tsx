import React, { useState } from 'react';
import { Plus, X } from 'lucide-react';

const terminalTabs = ['Terminal', 'Documentation', 'Debug Console'];

interface TerminalLine {
  type: 'prompt' | 'command' | 'info' | 'system' | 'result' | 'success' | 'text';
  content: string;
}

const terminalOutput: TerminalLine[] = [
  { type: 'prompt', content: 'unprompted@workspace:~/governance-v2$ ' },
  { type: 'command', content: 'python src/main.py' },
  { type: 'info', content: '[INFO] Loading evaluation engine...' },
  { type: 'info', content: '[INFO] Dataset loaded: governance_set_v2.json (240 entries)' },
  { type: 'system', content: '[SYSTEM] Executing audit sequence...' },
  { type: 'result', content: '[RESULT] Compliance Score: 0.94' },
  { type: 'success', content: 'Conformité validée.' },
  { type: 'prompt', content: 'unprompted@workspace:~/governance-v2$ ' },
  { type: 'text', content: '█' },
];

interface Props {
  onClose: () => void;
}

const Terminal: React.FC<Props> = ({ onClose }) => {
  const [activeTab, setActiveTab] = useState('Terminal');

  return (
    <div className="ws-terminal">
      {/* Header */}
      <div className="ws-terminal-header">
        <div className="ws-terminal-tabs">
          {terminalTabs.map((tab) => (
            <button
              key={tab}
              className={`ws-terminal-tab ${activeTab === tab ? 'active' : ''}`}
              onClick={() => setActiveTab(tab)}
            >
              {tab}
            </button>
          ))}
        </div>

        <div className="ws-terminal-actions">
          <button className="ws-terminal-action-btn" title="Nouveau terminal">
            <Plus size={15} />
          </button>
          <button className="ws-terminal-action-btn" title="Fermer" onClick={onClose}>
            <X size={15} />
          </button>
        </div>
      </div>

      {/* Terminal content */}
      <div className="ws-terminal-content">
        {terminalOutput.map((line, i) => {
          // Combine prompt + command on same visual line
          if (line.type === 'prompt' && i + 1 < terminalOutput.length && terminalOutput[i + 1].type === 'command') {
            return null; // rendered together with the command line
          }

          if (line.type === 'command' && i > 0 && terminalOutput[i - 1].type === 'prompt') {
            return (
              <div key={i}>
                <span className="ws-term-prompt">{terminalOutput[i - 1].content}</span>
                <span className="ws-term-command">{line.content}</span>
              </div>
            );
          }

          // Standalone prompt (at bottom with cursor)
          if (line.type === 'prompt') {
            return (
              <div key={i}>
                <span className="ws-term-prompt">{line.content}</span>
              </div>
            );
          }

          const classMap: Record<string, string> = {
            info: 'ws-term-info',
            system: 'ws-term-system',
            result: 'ws-term-result',
            success: 'ws-term-success',
            text: 'ws-term-text',
          };

          return (
            <div key={i}>
              <span className={classMap[line.type] || 'ws-term-text'}>{line.content}</span>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default Terminal;
