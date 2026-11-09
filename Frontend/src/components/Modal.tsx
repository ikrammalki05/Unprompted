import React from 'react';

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
}

export default function Modal({ isOpen, onClose, title, children }: ModalProps) {
  if (!isOpen) return null;

  return (
    <div 
      className="fixed inset-0 bg-black/40 backdrop-blur-[4px] flex items-center justify-center z-[1000] animate-[fadeIn_0.2s_ease]" 
      onClick={(e) => e.target === e.currentTarget && onClose()}
    >
      <div className="bg-white rounded-[14px] shadow-[0_20px_60px_rgba(0,0,0,0.2)] w-[90%] max-w-[480px] p-7 animate-[fadeIn_0.25s_ease]">
        <div className="flex items-center justify-between mb-5">
          <h2 className="text-[17px] font-semibold">{title}</h2>
          <button 
            className="p-[6px_8px] rounded-[6px] transition-[var(--transition)] hover:bg-[var(--bg)] hover:text-[var(--text-primary)] text-[var(--text-secondary)]" 
            onClick={onClose}
          >
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M18 6L6 18M6 6l12 12"/>
            </svg>
          </button>
        </div>
        {children}
      </div>
    </div>
  );
}
