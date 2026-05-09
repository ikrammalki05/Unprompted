import { IconBuilding, IconPin } from './Icons';
import type { AdminData } from '../types';

interface Props {
  data: AdminData;
  draft: AdminData;
  editing: boolean;
  setDraft: React.Dispatch<React.SetStateAction<AdminData>>;
}

export function InstitutionCard({ data, draft, editing, setDraft }: Props) {
  return (
    <div className="bg-white border border-[#f1f5f9] rounded-xl p-[32px] shadow-sm flex flex-col gap-6">
      <div className="flex items-center gap-4">
        <div className="w-10 h-10 bg-[#eff6ff] rounded-lg flex items-center justify-center text-[#2563eb]">
          <IconBuilding />
        </div>
        <h3 className="text-[17px] font-bold text-[#1e293b]">Détails de l'établissement</h3>
      </div>
      
      <div className="grid grid-cols-1 gap-4">
        <div className="bg-[#f8fafc] rounded-xl p-5 border border-[#f1f5f9] flex flex-col gap-2.5">
          <span className="text-[10px] font-extrabold text-[#94a3b8] uppercase tracking-[0.05em]">INSTITUTION</span>
          {editing ? (
            <input
              className="w-full p-[8px_12px] border border-[#e2e8f0] rounded-lg text-sm text-[#1e293b] outline-none transition-all bg-white"
              value={draft.institution}
              onChange={(e) => setDraft({ ...draft, institution: e.target.value })}
            />
          ) : (
            <span className="text-[18px] font-extrabold text-[#1e293b] leading-tight">{data.institution}</span>
          )}
        </div>
        
        <div className="bg-[#f8fafc] rounded-xl p-5 border border-[#f1f5f9] flex flex-col gap-2.5">
          <span className="text-[10px] font-extrabold text-[#94a3b8] uppercase tracking-[0.05em]">LOCALISATION</span>
          <div className="flex items-center gap-2 text-[#2563eb] font-semibold text-[13.5px]">
            <IconPin />
            {editing ? (
              <input
                className="w-full p-[8px_12px] border border-[#e2e8f0] rounded-lg text-sm text-[#1e293b] outline-none transition-all bg-white"
                value={draft.localisation}
                onChange={(e) => setDraft({ ...draft, localisation: e.target.value })}
              />
            ) : (
              <span>{data.localisation}</span>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}