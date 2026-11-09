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
    <div className="bg-white border border-slate-100 rounded-3xl p-7 md:p-8 shadow-sm flex flex-col gap-6">
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-blue-50 rounded-xl flex items-center justify-center text-blue-600">
          <IconUser />
        </div>
        <h3 className="text-base font-bold text-slate-900">Informations personnelles</h3>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 gap-y-6 gap-x-8">
        <InfoField
          label="Nom complet"
          value={data.nom}
          editable={editing}
          editValue={draft.nom}
          onChange={(v) => setDraft({ ...draft, nom: v })}
        />
        <InfoField
          label="Adresse e-mail"
          value={data.email}
          editable={editing}
          editValue={draft.email}
          onChange={(v) => setDraft({ ...draft, email: v })}
        />
        <InfoField
          label="Téléphone"
          value={data.telephone}
          editable={editing}
          editValue={draft.telephone}
          onChange={(v) => setDraft({ ...draft, telephone: v })}
        />

        <div className="flex flex-col gap-2">
          <span className="text-[11px] font-bold text-slate-400 uppercase tracking-[0.08em]">
            Rôle système
          </span>
          <div className="flex items-center gap-2.5 min-h-[46px]">
            <span className="w-2 h-2 bg-emerald-500 rounded-full" />
            {editing ? (
              <input
                className="w-full px-4 py-3 border border-slate-200 rounded-xl text-sm font-semibold text-slate-900 outline-none transition-all focus:border-blue-400 focus:ring-4 focus:ring-blue-50"
                value={draft.role}
                onChange={(e) => setDraft({ ...draft, role: e.target.value })}
              />
            ) : (
              <span className="text-sm font-semibold text-slate-900">{data.role}</span>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
