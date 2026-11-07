import React, { useState } from 'react';
import { X } from 'lucide-react';

interface TerminalLine {
  type: string;
  content: string;
}

interface Props {
  lines: TerminalLine[];
  onClose: () => void;
  onCommand: (command: string) => Promise<void> | void;
}

const Terminal: React.FC<Props> = ({ lines, onClose, onCommand }) => {
  const [input, setInput] = useState('');
  const [busy, setBusy] = useState(false);

  const submit = async () => {
    const cmd = input.trim();
    if (!cmd || busy) return;
    setBusy(true);
    setInput('');
    try {
      await onCommand(cmd);
    } finally {
      setBusy(false);
    }
  };

  return (
    <section className="ws-terminal">
      <div className="ws-terminal-header">
        <span>Terminal</span>
        <button className="ws-terminal-action-btn" onClick={onClose} title="Fermer">
          <X size={15} />
        </button>
      </div>
      <div className="ws-terminal-body">
        {lines.map((line, idx) => (
          <div key={idx} className={`ws-terminal-line ws-terminal-${line.type}`}>
            {line.type === 'command' ? `$ ${line.content}` : line.content}
          </div>
        ))}
      </div>
      <div className="ws-terminal-input-row" style={{ display: 'flex', gap: 8, padding: '8px 12px' }}>
        <span style={{ color: '#4ade80' }}>$</span>
        <input
          value={input}
          disabled={busy}
          onChange={(e) => setInput(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') void submit();
          }}
          placeholder={busy ? 'Exécution…' : 'Entrez une commande'}
          style={{
            flex: 1,
            background: 'transparent',
            border: 'none',
            outline: 'none',
            color: '#e2e8f0',
            fontFamily: 'ui-monospace, SFMono-Regular, Menlo, monospace',
          }}
        />
      </div>
    </section>
  );
};

export default Terminal;
