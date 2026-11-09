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
    <div className="w-full bg-gradient-to-r from-[#05162b] via-[#0b2a4a] to-[#1d4ed8] rounded-3xl p-8 md:p-10 flex flex-col md:flex-row md:items-center justify-between gap-6 text-white relative overflow-hidden shadow-sm">
      <div className="relative z-10 flex flex-col gap-2">
        <p className="text-[11px] font-bold uppercase tracking-[0.14em] text-blue-200/80">Profil administrateur</p>
        <h2 className="text-3xl md:text-[34px] font-black tracking-tight">Bienvenue, {nom}</h2>
        <p className="text-sm md:text-[15px] text-slate-300 font-medium max-w-xl">
          Gérez vos paramètres personnels et les configurations de l’établissement.
        </p>
      </div>
      <div className="relative z-10 flex flex-wrap items-center gap-3">
        {saved && (
          <div className="flex items-center gap-2 bg-emerald-50 text-emerald-700 px-4 py-2.5 rounded-xl text-sm font-bold border border-emerald-100">
            <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3">
              <polyline points="20 6 9 17 4 12" />
            </svg>
            Enregistré
          </div>
        )}
        {editing ? (
          <>
            <button
              className="flex items-center gap-2 px-5 py-2.5 rounded-xl text-sm font-bold bg-white/10 text-white border border-white/20 hover:bg-white/20 transition-all"
              onClick={onCancel}
            >
              <IconClose /> Annuler
            </button>
            <button
              className="flex items-center gap-2 px-5 py-2.5 rounded-xl text-sm font-bold bg-white text-[#05162b] hover:bg-slate-100 transition-all"
              onClick={onSave}
            >
              <IconSave /> Enregistrer
            </button>
          </>
        ) : (
          <button
            className="flex items-center gap-2.5 px-5 py-2.5 rounded-xl text-sm font-bold bg-white text-[#05162b] hover:bg-slate-100 transition-all"
            onClick={onEdit}
          >
            <IconEdit /> Modifier le profil
          </button>
        )}
      </div>
    </div>
  );
}
