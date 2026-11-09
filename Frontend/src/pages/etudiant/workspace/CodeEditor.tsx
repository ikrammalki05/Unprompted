import React from 'react';

interface Props {
  fileName: string;
  content: string;
  onChange: (value: string) => void;
}

const CodeEditor: React.FC<Props> = ({ fileName, content, onChange }) => {
  const lines = Math.max(1, content.split('\n').length);

  return (
    <section className="ws-editor">
      <div className="ws-editor-tabs">
        <div className="ws-editor-tab active">{fileName || 'untitled'}</div>
      </div>
      <div className="ws-editor-body" style={{ display: 'flex', height: '100%' }}>
        <div
          className="ws-line-numbers"
          style={{
            padding: '12px 8px',
            color: '#64748b',
            textAlign: 'right',
            userSelect: 'none',
            fontFamily: 'ui-monospace, SFMono-Regular, Menlo, monospace',
            fontSize: 13,
            lineHeight: '20px',
            borderRight: '1px solid #1e293b',
            minWidth: 40,
          }}
        >
          {Array.from({ length: lines }, (_, i) => (
            <div key={i}>{i + 1}</div>
          ))}
        </div>
        <textarea
          value={content}
          onChange={(e) => onChange(e.target.value)}
          spellCheck={false}
          style={{
            flex: 1,
            width: '100%',
            height: '100%',
            resize: 'none',
            border: 'none',
            outline: 'none',
            background: '#0b1220',
            color: '#e2e8f0',
            padding: '12px',
            fontFamily: 'ui-monospace, SFMono-Regular, Menlo, monospace',
            fontSize: 13,
            lineHeight: '20px',
            whiteSpace: 'pre',
            overflow: 'auto',
          }}
          placeholder="Écrivez votre code ici…"
        />
      </div>
    </section>
  );
};

export default CodeEditor;
