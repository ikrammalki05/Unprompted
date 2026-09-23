import { IconClose, IconSave, IconEdit } from './Icons';

interface Props {
  nom: string;
  saved: boolean;
  editing: boolean;
  onEdit: () => void;
  onSave: () => void;
  onCancel: () => void;
}

export function ProfileBanner({ nom, saved, editing, onEdit, onSave, onCancel }: Props) {
  return (
    <div className="w-full bg-[#021a32] rounded-[14px] p-[48px_40px] flex items-center justify-between mb-2 text-white relative overflow-hidden shadow-[0_20px_50px_rgba(2,26,50,0.15)]">
      <div className="relative z-10 flex flex-col gap-1.5">
        <h2 className="text-[32px] font-bold tracking-tight">Bienvenue, {nom}</h2>
        <p className="text-[15px] text-[#94a3b8] font-medium">Gérez vos paramètres personnels et les configurations de l'établissement.</p>
      </div>
      <div className="relative z-10 flex items-center gap-4">
        {saved && (
          <div className="flex items-center gap-2 bg-[#dcfce7] text-[#15803d] p-[10px_18px] rounded-lg text-[13.5px] font-bold border border-[#bbf7d0] shadow-sm animate-[slideInRight_0.3s_ease-out]">
            <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3"><polyline points="20 6 9 17 4 12" /></svg>
            Enregistré
          </div>
        )}
        {editing ? (
          <>
            <button className="flex items-center gap-2 p-[10px_22px] rounded-lg text-sm font-bold cursor-pointer transition-all bg-white/10 text-white border border-white/20 hover:bg-white/20" onClick={onCancel}>
              <IconClose /> Annuler
            </button>
            <button className="flex items-center gap-2 p-[10px_22px] rounded-lg text-sm font-bold cursor-pointer transition-all bg-[#2563eb] text-white border-none hover:bg-[#1d4ed8] shadow-[0_4px_12px_rgba(37,99,235,0.4)]" onClick={onSave}>
              <IconSave /> Enregistrer
            </button>
          </>
        ) : (
          <button className="flex items-center gap-2.5 p-[11px_24px] rounded-lg text-[14px] font-bold cursor-pointer transition-all bg-[#2563eb] text-white border-none hover:bg-[#1d4ed8] shadow-[0_10px_20px_rgba(37,99,235,0.3)]" onClick={onEdit}>
            <IconEdit /> Modifier le profil
          </button>
        )}
      </div>
    </div>
  );
}