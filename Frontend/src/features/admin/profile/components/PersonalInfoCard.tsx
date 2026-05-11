import { IconUser } from './Icons';
import { InfoField } from './InfoField';
import type { AdminData } from '../types';

interface Props {
  data: AdminData;
  draft: AdminData;
  editing: boolean;
  setDraft: React.Dispatch<React.SetStateAction<AdminData>>;
}

export function PersonalInfoCard({ data, draft, editing, setDraft }: Props) {
  return (
    <div className="bg-white border border-[#f1f5f9] rounded-xl p-[32px] shadow-sm flex flex-col gap-6">
      <div className="flex items-center gap-4">
        <div className="w-10 h-10 bg-[#eff6ff] rounded-lg flex items-center justify-center text-[#2563eb]">
          <IconUser />
        </div>
        <h3 className="text-[17px] font-bold text-[#1e293b]">Informations personnelles</h3>
      </div>
      
      <div className="grid grid-cols-2 gap-y-7 gap-x-12">
        <InfoField label="NOM COMPLET" value={data.nom} editable={editing} editValue={draft.nom} onChange={(v) => setDraft({ ...draft, nom: v })} />
        <InfoField label="ADRESSE E-MAIL" value={data.email} editable={editing} editValue={draft.email} onChange={(v) => setDraft({ ...draft, email: v })} />
        <InfoField label="TÉLÉPHONE" value={data.telephone} editable={editing} editValue={draft.telephone} onChange={(v) => setDraft({ ...draft, telephone: v })} />
        
        <div className="flex flex-col gap-1.5">
          <span className="text-[10px] font-bold text-[#94a3b8] uppercase tracking-[0.05em]">RÔLE AU SEIN DU SYSTÈME</span>
          <div className="flex items-center gap-2.5">
            <span className="w-2 h-2 bg-[#22c55e] rounded-full" />
            {editing ? (
              <input
                className="w-full p-[8px_12px] border border-[#e2e8f0] rounded-lg text-sm text-[#1e293b] outline-none transition-all focus:border-[#2563eb]"
                value={draft.role}
                onChange={(e) => setDraft({ ...draft, role: e.target.value })}
              />
            ) : (
              <span className="text-sm font-bold text-[#1e293b]">{data.role}</span>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}