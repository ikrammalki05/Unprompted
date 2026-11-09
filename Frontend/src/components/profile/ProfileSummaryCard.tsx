import React from 'react';
import { Mail, ShieldCheck } from 'lucide-react';

interface ProfileSummaryCardProps {
  fullName: string;
  email: string;
  roleLabel: string;
  statusLabel?: string;
  initials?: string;
  meta?: Array<{ label: string; value: string }>;
}

export const ProfileSummaryCard: React.FC<ProfileSummaryCardProps> = ({
  fullName,
  email,
  roleLabel,
  statusLabel = 'Actif',
  initials,
  meta = [],
}) => {
  const avatar =
    initials ||
    fullName
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map((p) => p[0]?.toUpperCase())
      .join('') ||
    'UP';

  return (
    <div className="bg-white rounded-3xl border border-slate-100 shadow-sm overflow-hidden">
      <div className="h-24 bg-gradient-to-r from-[#05162b] via-[#0b2a4a] to-[#1d4ed8]" />
      <div className="px-7 pb-8 -mt-12">
        <div className="w-24 h-24 rounded-full bg-white p-1.5 shadow-lg mx-auto mb-4">
          <div className="w-full h-full rounded-full bg-slate-100 flex items-center justify-center text-[#05162b] text-2xl font-black">
            {avatar}
          </div>
        </div>

        <div className="text-center mb-6">
          <h3 className="text-2xl font-black text-slate-900 tracking-tight">{fullName}</h3>
          <p className="mt-1 text-[11px] font-bold uppercase tracking-[0.14em] text-blue-600">
            {roleLabel}
          </p>
        </div>

        <div className="space-y-3">
          <div className="flex items-center gap-3 p-3.5 rounded-2xl bg-slate-50 border border-slate-100">
            <div className="w-9 h-9 rounded-xl bg-white border border-slate-100 flex items-center justify-center text-slate-400">
              <Mail className="w-4 h-4" />
            </div>
            <div className="min-w-0">
              <p className="text-[10px] font-bold uppercase tracking-wider text-slate-400">Email</p>
              <p className="text-sm font-semibold text-slate-700 truncate">{email}</p>
            </div>
          </div>

          <div className="flex items-center justify-between gap-3 p-3.5 rounded-2xl bg-slate-50 border border-slate-100">
            <div className="flex items-center gap-3 min-w-0">
              <div className="w-9 h-9 rounded-xl bg-white border border-slate-100 flex items-center justify-center text-slate-400">
                <ShieldCheck className="w-4 h-4" />
              </div>
              <div>
                <p className="text-[10px] font-bold uppercase tracking-wider text-slate-400">Statut</p>
                <p className="text-sm font-semibold text-slate-700">{statusLabel}</p>
              </div>
            </div>
            <span className="shrink-0 px-2.5 py-1 rounded-lg bg-emerald-50 text-emerald-700 text-[10px] font-black uppercase tracking-wide">
              Conforme
            </span>
          </div>

          {meta.map((item) => (
            <div
              key={item.label}
              className="flex items-center justify-between px-1 py-1 text-sm"
            >
              <span className="text-[10px] font-bold uppercase tracking-wider text-slate-400">
                {item.label}
              </span>
              <span className="font-semibold text-slate-800">{item.value}</span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ProfileSummaryCard;
