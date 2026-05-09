import React from 'react';
import Editor from '@monaco-editor/react';

interface MonacoEditorProps {
  language: string;
  value: string;
  onChange: (value: string | undefined) => void;
  fileName?: string;
}

export const MonacoEditor: React.FC<MonacoEditorProps> = ({ 
  language, 
  value, 
  onChange,
  fileName = "app.jsx" 
}) => {
  return (
    /* La div parent prend juste 100% de la hauteur disponible, sans forcer de minimum */
    <div className="w-full h-full">
      <Editor
        height="100%"
        theme="vs-dark"
        path={fileName}
        defaultLanguage={language}
        value={value}
        onChange={onChange}
        options={{
          minimap: { enabled: false },
          fontSize: 14,
          wordWrap: 'on',
          scrollBeyondLastLine: false,
          automaticLayout: true,
          padding: { top: 16 }
        }}
        loading={
          <div className="flex items-center justify-center h-full text-[#cccccc]">
            Chargement de l'éditeur...
          </div>
        }
      />
    </div>
  );
};