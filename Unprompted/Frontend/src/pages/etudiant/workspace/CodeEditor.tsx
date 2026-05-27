import React from 'react';
import { X } from 'lucide-react';

// ── File contents with pre-tokenised syntax ────────────────────────
interface CodeLine {
  tokens: { text: string; cls: string }[];
}

const FILES: Record<string, CodeLine[]> = {
  'main.py': [
    {
      tokens: [
        { text: 'import', cls: 'syn-keyword' },
        { text: ' unprompted_sdk ', cls: 'syn-variable' },
        { text: 'as', cls: 'syn-keyword' },
        { text: ' sdk', cls: 'syn-variable' },
      ],
    },
    {
      tokens: [
        { text: 'from', cls: 'syn-keyword' },
        { text: ' governance ', cls: 'syn-variable' },
        { text: 'import', cls: 'syn-keyword' },
        { text: ' EvaluationEngine', cls: 'syn-class' },
      ],
    },
    { tokens: [] }, // blank line
    {
      tokens: [
        { text: '# Initialisation de l\'environnement de', cls: 'syn-comment' },
      ],
    },
    {
      tokens: [
        { text: '# recherche', cls: 'syn-comment' },
      ],
    },
    {
      tokens: [
        { text: 'engine', cls: 'syn-variable' },
        { text: ' = ', cls: 'syn-operator' },
        { text: 'EvaluationEngine', cls: 'syn-class' },
        { text: '(', cls: 'syn-paren' },
        { text: 'model', cls: 'syn-variable' },
        { text: '=', cls: 'syn-operator' },
        { text: '"gpt-4-', cls: 'syn-string' },
      ],
    },
    {
      tokens: [
        { text: 'academic"', cls: 'syn-string' },
        { text: ')', cls: 'syn-paren' },
      ],
    },
    { tokens: [] }, // blank line
    {
      tokens: [
        { text: 'def', cls: 'syn-keyword' },
        { text: ' run_audit', cls: 'syn-function' },
        { text: '(', cls: 'syn-paren' },
        { text: 'dataset_path', cls: 'syn-variable' },
        { text: ')', cls: 'syn-paren' },
        { text: ':', cls: 'syn-operator' },
      ],
    },
    {
      tokens: [
        { text: '    results', cls: 'syn-variable' },
        { text: ' =', cls: 'syn-operator' },
      ],
    },
    {
      tokens: [
        { text: '    engine', cls: 'syn-variable' },
        { text: '.', cls: 'syn-dot' },
        { text: 'evaluate', cls: 'syn-function' },
        { text: '(', cls: 'syn-paren' },
        { text: 'dataset_path', cls: 'syn-variable' },
        { text: ')', cls: 'syn-paren' },
      ],
    },
    {
      tokens: [
        { text: '    ', cls: 'syn-variable' },
        { text: 'if', cls: 'syn-keyword' },
        { text: ' results', cls: 'syn-variable' },
        { text: '.', cls: 'syn-dot' },
        { text: 'score', cls: 'syn-variable' },
        { text: ' > ', cls: 'syn-operator' },
        { text: '0.85', cls: 'syn-number' },
        { text: ':', cls: 'syn-operator' },
      ],
    },
    {
      tokens: [
        { text: '        ', cls: 'syn-variable' },
        { text: 'print', cls: 'syn-builtin' },
        { text: '(', cls: 'syn-paren' },
        { text: '"Conformité validée."', cls: 'syn-string' },
        { text: ')', cls: 'syn-paren' },
      ],
    },
    {
      tokens: [
        { text: '    ', cls: 'syn-variable' },
        { text: 'else', cls: 'syn-keyword' },
        { text: ':', cls: 'syn-operator' },
      ],
    },
    {
      tokens: [
        { text: '        ', cls: 'syn-variable' },
        { text: 'print', cls: 'syn-builtin' },
        { text: '(', cls: 'syn-paren' },
        { text: '"Révision nécessaire."', cls: 'syn-string' },
        { text: ')', cls: 'syn-paren' },
      ],
    },
    { tokens: [] }, // blank line
    {
      tokens: [
        { text: 'run_audit', cls: 'syn-function' },
        { text: '(', cls: 'syn-paren' },
        { text: '"./data/governance_set_v2.json"', cls: 'syn-string' },
        { text: ')', cls: 'syn-paren' },
      ],
    },
  ],
  'utils.py': [
    {
      tokens: [
        { text: '# Utility functions for governance auditing', cls: 'syn-comment' },
      ],
    },
    { tokens: [] },
    {
      tokens: [
        { text: 'import', cls: 'syn-keyword' },
        { text: ' json', cls: 'syn-variable' },
      ],
    },
    {
      tokens: [
        { text: 'import', cls: 'syn-keyword' },
        { text: ' os', cls: 'syn-variable' },
      ],
    },
    { tokens: [] },
    {
      tokens: [
        { text: 'def', cls: 'syn-keyword' },
        { text: ' load_dataset', cls: 'syn-function' },
        { text: '(', cls: 'syn-paren' },
        { text: 'path', cls: 'syn-variable' },
        { text: ':', cls: 'syn-operator' },
        { text: ' str', cls: 'syn-builtin' },
        { text: ')', cls: 'syn-paren' },
        { text: ':', cls: 'syn-operator' },
      ],
    },
    {
      tokens: [
        { text: '    ', cls: 'syn-variable' },
        { text: '"""Load a JSON dataset from disk."""', cls: 'syn-string' },
      ],
    },
    {
      tokens: [
        { text: '    ', cls: 'syn-variable' },
        { text: 'with', cls: 'syn-keyword' },
        { text: ' ', cls: 'syn-variable' },
        { text: 'open', cls: 'syn-builtin' },
        { text: '(', cls: 'syn-paren' },
        { text: 'path', cls: 'syn-variable' },
        { text: ', ', cls: 'syn-variable' },
        { text: '"r"', cls: 'syn-string' },
        { text: ')', cls: 'syn-paren' },
        { text: ' ', cls: 'syn-variable' },
        { text: 'as', cls: 'syn-keyword' },
        { text: ' f:', cls: 'syn-variable' },
      ],
    },
    {
      tokens: [
        { text: '        ', cls: 'syn-variable' },
        { text: 'return', cls: 'syn-keyword' },
        { text: ' json', cls: 'syn-variable' },
        { text: '.', cls: 'syn-dot' },
        { text: 'load', cls: 'syn-function' },
        { text: '(', cls: 'syn-paren' },
        { text: 'f', cls: 'syn-variable' },
        { text: ')', cls: 'syn-paren' },
      ],
    },
  ],
  'README.md': [
    {
      tokens: [
        { text: '# Gouvernance IA — Unprompted', cls: 'syn-keyword' },
      ],
    },
    { tokens: [] },
    {
      tokens: [
        { text: 'Plateforme de gouvernance académique de l\'IA.', cls: 'syn-variable' },
      ],
    },
    { tokens: [] },
    {
      tokens: [
        { text: '## Installation', cls: 'syn-keyword' },
      ],
    },
    { tokens: [] },
    {
      tokens: [
        { text: '```bash', cls: 'syn-comment' },
      ],
    },
    {
      tokens: [
        { text: 'pip install -r requirements.txt', cls: 'syn-string' },
      ],
    },
    {
      tokens: [
        { text: 'python src/main.py', cls: 'syn-string' },
      ],
    },
    {
      tokens: [
        { text: '```', cls: 'syn-comment' },
      ],
    },
  ],
};

interface Props {
  activeFile: string;
}

const CodeEditor: React.FC<Props> = ({ activeFile }) => {
  const lines = FILES[activeFile] || FILES['main.py'];

  const getFileIcon = (name: string) => {
    if (name.endsWith('.py')) return '🐍';
    if (name.endsWith('.md')) return '📄';
    return '📁';
  };

  return (
    <div className="ws-editor">
      {/* Tab bar */}
      <div className="ws-editor-tabs">
        <button className="ws-editor-tab active">
          <span>{getFileIcon(activeFile)}</span>
          <span>{activeFile}</span>
          <span className="ws-editor-tab-close">
            <X size={14} />
          </span>
        </button>
      </div>

      {/* Code content */}
      <div className="ws-editor-content">
        {lines.map((line, i) => (
          <div key={i} className="ws-code-line">
            <span className="ws-line-number">{i + 1}</span>
            <span className="ws-line-content">
              {line.tokens.length === 0 ? '\u00A0' : line.tokens.map((token, j) => (
                <span key={j} className={token.cls}>{token.text}</span>
              ))}
            </span>
          </div>
        ))}
      </div>
    </div>
  );
};

export default CodeEditor;
