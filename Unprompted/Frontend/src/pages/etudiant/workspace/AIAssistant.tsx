import React, { useState } from 'react';
import { Bot, Send } from 'lucide-react';

const AIAssistant: React.FC = () => {
  const [inputValue, setInputValue] = useState('');

  return (
    <div className="ws-assistant">
      {/* Header */}
      <div className="ws-assistant-header">
        <Bot size={18} />
        <span>Assistant IA</span>
      </div>

      {/* Messages */}
      <div className="ws-assistant-messages">
        {/* AI message */}
        <div className="ws-msg-ai">
          J'ai analysé votre script <strong>main.py</strong>. Voulez-vous que j'ajoute un middleware de journalisation pour les audits ?
        </div>

        {/* User response */}
        <div className="ws-msg-user">
          Oui, s'il te plaît. Montre-moi un exemple.
        </div>

        {/* Suggestion block */}
        <div className="ws-msg-ai">
          <div className="ws-suggestion-label">Suggestion</div>
          <div className="ws-suggestion-code">
            sdk.log_event("audit_start")
          </div>
        </div>
      </div>

      {/* Input */}
      <div className="ws-assistant-input">
        <input
          type="text"
          placeholder="Posez une question..."
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              e.preventDefault();
              setInputValue('');
            }
          }}
        />
        <button
          className="ws-assistant-send"
          onClick={() => setInputValue('')}
          title="Envoyer"
        >
          <Send size={16} />
        </button>
      </div>
    </div>
  );
};

export default AIAssistant;
