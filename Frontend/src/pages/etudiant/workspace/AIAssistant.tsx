import React, { useCallback, useEffect, useRef, useState } from 'react';
import { Send } from 'lucide-react';
import workspaceService from '../../../services/workspaceService';

interface Message {
  role: 'user' | 'ai';
  content: string;
  meta?: string;
}

interface Props {
  projectId: number;
  width: number;
  onWidthChange: (width: number) => void;
  onResizeStart?: () => void;
  onResizeEnd?: () => void;
}

const MIN_WIDTH = 280;
const MAX_WIDTH_RATIO = 0.7;

const AIAssistant: React.FC<Props> = ({
  projectId,
  width,
  onWidthChange,
  onResizeStart,
  onResizeEnd,
}) => {
  const [messages, setMessages] = useState<Message[]>([
    {
      role: 'ai',
      content:
        "Assistant RAG prêt. Posez une question sur votre projet. Le niveau d'accès est celui défini par votre enseignant.",
    },
  ]);
  const [inputValue, setInputValue] = useState('');
  const [loading, setLoading] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const draggingRef = useRef(false);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages, loading]);

  const clampWidth = useCallback((value: number) => {
    const max = Math.floor(window.innerWidth * MAX_WIDTH_RATIO);
    return Math.min(max, Math.max(MIN_WIDTH, Math.round(value)));
  }, []);

  const onResizePointerDown = (e: React.PointerEvent<HTMLButtonElement>) => {
    e.preventDefault();
    const handle = e.currentTarget;
    draggingRef.current = true;
    onResizeStart?.();
    handle.setPointerCapture(e.pointerId);

    const onMove = (ev: PointerEvent) => {
      if (!draggingRef.current) return;
      onWidthChange(clampWidth(window.innerWidth - ev.clientX));
    };

    const onUp = (ev: PointerEvent) => {
      draggingRef.current = false;
      onResizeEnd?.();
      window.removeEventListener('pointermove', onMove);
      window.removeEventListener('pointerup', onUp);
      if (handle.hasPointerCapture(ev.pointerId)) {
        handle.releasePointerCapture(ev.pointerId);
      }
    };

    window.addEventListener('pointermove', onMove);
    window.addEventListener('pointerup', onUp);
  };

  const send = async () => {
    const text = inputValue.trim();
    if (!text || loading || !projectId) return;

    setInputValue('');
    setMessages((prev) => [...prev, { role: 'user', content: text }]);
    setLoading(true);
    try {
      const res = await workspaceService.askAi(projectId, text);
      setMessages((prev) => [
        ...prev,
        {
          role: 'ai',
          content: res.response,
          meta: `niveau: ${res.accessLevel} · statut: ${res.status}`,
        },
      ]);
    } catch (e: any) {
      setMessages((prev) => [
        ...prev,
        {
          role: 'ai',
          content: e?.response?.data?.message || 'Impossible de contacter le service IA.',
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <aside className="ws-assistant" style={{ width }}>
      <button
        type="button"
        className="ws-assistant-resize"
        aria-label="Redimensionner le chat"
        title="Glisser pour agrandir ou réduire"
        onPointerDown={onResizePointerDown}
      />
      <div className="ws-assistant-header">Assistant IA (RAG)</div>
      <div className="ws-assistant-messages">
        {messages.map((m, i) => (
          <div key={i} className={m.role === 'ai' ? 'ws-msg-ai' : 'ws-msg-user'}>
            <div>{m.content}</div>
            {m.meta && (
              <div style={{ marginTop: 6, fontSize: 11, opacity: 0.7 }}>{m.meta}</div>
            )}
          </div>
        ))}
        {loading && <div className="ws-msg-ai">Réflexion…</div>}
        <div ref={messagesEndRef} />
      </div>
      <div className="ws-assistant-input">
        <input
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              e.preventDefault();
              void send();
            }
          }}
          placeholder="Posez votre question…"
        />
        <button onClick={() => void send()} disabled={loading || !inputValue.trim()}>
          <Send size={16} />
        </button>
      </div>
    </aside>
  );
};

export default AIAssistant;
