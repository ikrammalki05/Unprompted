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
    <div className="bg-white border border-slate-100 rounded-3xl p-7 md:p-8 shadow-sm flex flex-col gap-6">
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-blue-50 rounded-xl flex items-center justify-center text-blue-600">
          <IconBuilding />
        </div>
        <h3 className="text-base font-bold text-slate-900">Détails de l’établissement</h3>
      </div>

      <div className="grid grid-cols-1 gap-4">
        <div className="bg-slate-50 rounded-2xl p-5 border border-slate-100 flex flex-col gap-2.5">
          <span className="text-[11px] font-bold text-slate-400 uppercase tracking-[0.08em]">
            Institution
          </span>
          {editing ? (
            <input
              className="w-full px-4 py-3 border border-slate-200 rounded-xl text-sm font-semibold text-slate-900 outline-none transition-all bg-white focus:border-blue-400 focus:ring-4 focus:ring-blue-50"
              value={draft.institution}
              onChange={(e) => setDraft({ ...draft, institution: e.target.value })}
            />
          ) : (
            <span className="text-lg font-extrabold text-slate-900 leading-tight">
              {data.institution}
            </span>
          )}
        </div>

        <div className="bg-slate-50 rounded-2xl p-5 border border-slate-100 flex flex-col gap-2.5">
          <span className="text-[11px] font-bold text-slate-400 uppercase tracking-[0.08em]">
            Localisation
          </span>
          <div className="flex items-center gap-2 text-blue-600 font-semibold text-sm">
            <IconPin />
            {editing ? (
              <input
                className="w-full px-4 py-3 border border-slate-200 rounded-xl text-sm font-semibold text-slate-900 outline-none transition-all bg-white focus:border-blue-400 focus:ring-4 focus:ring-blue-50"
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
