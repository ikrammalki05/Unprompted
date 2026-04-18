import { useState } from 'react';

// ── Admin data ──
interface AdminData {
  nom: string;
  email: string;
  telephone: string;
  role: string;
  institution: string;
  localisation: string;
}

const adminData: AdminData = {
  nom: 'Administrateur',
  email: 'admin@unprompted.edu',
  telephone: '00000000',
  role: 'Administration Centrale',
  institution: 'École Nationale des Sciences Appliquées',
  localisation: 'Tanger, Maroc',
};

// ── Icons ──
const IconBell = () => (
  <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9" />
    <path d="M13.73 21a2 2 0 0 1-3.46 0" />
  </svg>
);

const IconEdit = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" />
    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" />
  </svg>
);

const IconUser = () => (
  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
    <circle cx="12" cy="7" r="4" />
  </svg>
);

const IconBuilding = () => (
  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <rect x="3" y="9" width="18" height="13" rx="1" />
    <path d="M8 9V5a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v4" />
    <line x1="12" y1="13" x2="12" y2="17" />
    <line x1="10" y1="15" x2="14" y2="15" />
  </svg>
);

const IconPin = () => (
  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
    <circle cx="12" cy="10" r="3" />
  </svg>
);

const IconSave = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
    <path d="M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2z" />
    <polyline points="17 21 17 13 7 13 7 21" />
    <polyline points="7 3 7 8 15 8" />
  </svg>
);

const IconClose = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <line x1="18" y1="6" x2="6" y2="18" />
    <line x1="6" y1="6" x2="18" y2="18" />
  </svg>
);

// ── Field component ──
interface InfoFieldProps {
  label: string;
  value: string;
  editable: boolean;
  editValue: string;
  onChange: (value: string) => void;
}

function InfoField({ label, value, editable, editValue, onChange }: InfoFieldProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <span className="text-[10px] font-bold text-[#94a3b8] uppercase tracking-[0.05em]">{label}</span>
      {editable ? (
        <input
          className="w-full p-[8px_12px] border border-[#e2e8f0] rounded-lg text-sm text-[#1e293b] outline-none transition-all focus:border-[var(--accent)] focus:ring-1 focus:ring-[var(--accent)]"
          value={editValue}
          onChange={(e) => onChange(e.target.value)}
        />
      ) : (
        <span className="text-sm font-medium text-[#1e293b]">{value}</span>
      )}
    </div>
  );
}

// ── Main page ──
export default function ProfilePage() {
  const [editing, setEditing] = useState(false);
  const [data, setData] = useState<AdminData>(adminData);
  const [draft, setDraft] = useState<AdminData>(adminData);
  const [saved, setSaved] = useState(false);
  const [notifOpen, setNotifOpen] = useState(false);

  const handleEdit = () => {
    setDraft({ ...data });
    setEditing(true);
  };

  const handleSave = () => {
    setData({ ...draft });
    setEditing(false);
    setSaved(true);
    setTimeout(() => setSaved(false), 2500);
  };

  const handleCancel = () => {
    setEditing(false);
    setDraft({ ...data });
  };

  return (
    <div className="flex-1 p-[28px_40px] overflow-y-auto w-full flex flex-col gap-6">

      {/* ── Top bar ── */}
      <header className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <h1 className="text-[20px] font-bold text-[#1e293b] tracking-tight">Profil Utilisateur</h1>
          <span className="bg-[#e2e8f0] text-[#64748b] text-[10px] font-extrabold p-[4px_12px] rounded-[6px] tracking-widest uppercase">COMPTE ADMINISTRATEUR</span>
        </div>
        <div className="flex items-center gap-6">
          <button
            className={`relative bg-transparent border-none cursor-pointer flex items-center justify-center p-2 rounded-full transition-all hover:bg-[#f1f5f9] hover:text-[var(--accent)] ${notifOpen ? 'bg-[#f1f5f9] text-[var(--accent)]' : 'text-[#64748b]'}`}
            onClick={() => setNotifOpen((o) => !o)}
          >
            <IconBell />
            <span className="absolute top-[6px] right-[6px] w-[9px] h-[9px] bg-[#ef4444] border-[2px] border-white rounded-full shadow-[0_0_0_1px_rgba(255,255,255,0.5)]" />
          </button>
          <div className="text-right flex flex-col justify-center">
            <span className="text-[13.5px] font-bold text-[#1e293b] leading-tight">{data.nom}</span>
            <span className="text-[11px] font-medium text-[#94a3b8]">{data.email}</span>
          </div>
        </div>
      </header>

      {/* ── Welcome banner ── */}
      <div className="w-full bg-[#021a32] rounded-[14px] p-[48px_40px] flex items-center justify-between mb-2 text-white relative overflow-hidden shadow-[0_20px_50px_rgba(2,26,50,0.15)]">
        {/* Subtle decorative elements could go here */}
        <div className="relative z-10 flex flex-col gap-1.5">
          <h2 className="text-[32px] font-bold tracking-tight">Bienvenue, {data.nom}</h2>
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
              <button className="flex items-center gap-2 p-[10px_22px] rounded-lg text-sm font-bold cursor-pointer transition-all bg-white/10 text-white border border-white/20 hover:bg-white/20" onClick={handleCancel}>
                <IconClose /> Annuler
              </button>
              <button className="flex items-center gap-2 p-[10px_22px] rounded-lg text-sm font-bold cursor-pointer transition-all bg-[#2563eb] text-white border-none hover:bg-[#1d4ed8] shadow-[0_4px_12px_rgba(37,99,235,0.4)]" onClick={handleSave}>
                <IconSave /> Enregistrer
              </button>
            </>
          ) : (
            <button className="flex items-center gap-2.5 p-[11px_24px] rounded-lg text-[14px] font-bold cursor-pointer transition-all bg-[#2563eb] text-white border-none hover:bg-[#1d4ed8] shadow-[0_10px_20px_rgba(37,99,235,0.3)]" onClick={handleEdit}>
              <IconEdit /> Modifier le profil
            </button>
          )}
        </div>
      </div>

      {/* ── Cards grid ── */}
      <div className="grid grid-cols-1 min-[1100px]:grid-cols-2 gap-6 pb-10">

        {/* Informations personnelles */}
        <div className="bg-white border border-[#f1f5f9] rounded-xl p-[32px] shadow-sm flex flex-col gap-6">
          <div className="flex items-center gap-4">
            <div className="w-10 h-10 bg-[#eff6ff] rounded-lg flex items-center justify-center text-[#2563eb]">
              <IconUser />
            </div>
            <h3 className="text-[17px] font-bold text-[#1e293b]">Informations personnelles</h3>
          </div>
          
          <div className="grid grid-cols-2 gap-y-7 gap-x-12">
            <InfoField
              label="NOM COMPLET"
              value={data.nom}
              editable={editing}
              editValue={draft.nom}
              onChange={(v) => setDraft({ ...draft, nom: v })}
            />
            <InfoField
              label="ADRESSE E-MAIL"
              value={data.email}
              editable={editing}
              editValue={draft.email}
              onChange={(v) => setDraft({ ...draft, email: v })}
            />
            <InfoField
              label="TÉLÉPHONE"
              value={data.telephone}
              editable={editing}
              editValue={draft.telephone}
              onChange={(v) => setDraft({ ...draft, telephone: v })}
            />
            <div className="flex flex-col gap-1.5">
              <span className="text-[10px] font-bold text-[#94a3b8] uppercase tracking-[0.05em]">RÔLE AU SEIN DU SYSTÈME</span>
              <div className="flex items-center gap-2.5">
                <span className="w-2 h-2 bg-[#22c55e] rounded-full" />
                {editing ? (
                  <input
                    className="w-full p-[8px_12px] border border-[#e2e8f0] rounded-lg text-sm text-[#1e293b] outline-none transition-all focus:border-[var(--accent)]"
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

        {/* Détails de l'établissement */}
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

      </div>

      {/* Notification dropdown */}
      {notifOpen && (
        <div className="absolute top-[75px] right-10 w-80 bg-white rounded-2xl shadow-[0_20px_50px_rgba(0,0,0,0.12)] border border-[#f1f5f9] z-[100] overflow-hidden animate-[fadeIn_0.2s_ease-out]">
          <div className="p-[18px_24px] font-bold border-b border-[#f1f5f9] text-[14px]">Notifications</div>
          <div className="max-h-[300px] overflow-y-auto">
            <div className="p-[18px_24px] flex gap-3 border-b border-[#f1f5f9] cursor-pointer transition-colors bg-[#f0f7ff] hover:bg-[#f8fafc]">
              <div className="w-1.5 h-1.5 bg-[#2563eb] rounded-full mt-[7px] shrink-0" />
              <div>
                <p className="text-[13px] font-medium mb-1 text-[#1e293b] leading-relaxed">Nouvel étudiant ajouté à Master IABD – G1</p>
                <span className="text-[11px] font-medium text-[#94a3b8]">Il y a 5 min</span>
              </div>
            </div>
            <div className="p-[18px_24px] flex gap-3 border-b border-[#f1f5f9] cursor-pointer transition-colors hover:bg-[#f8fafc]">
              <div className="w-1.5 h-1.5 bg-[#2563eb] rounded-full mt-[7px] shrink-0 opacity-0" />
              <div>
                <p className="text-[13px] font-medium mb-1 text-[#1e293b] leading-relaxed">Affectation mise à jour pour Dr. Alami</p>
                <span className="text-[11px] font-medium text-[#94a3b8]">Il y a 1h</span>
              </div>
            </div>
            <div className="p-[18px_24px] flex gap-3 cursor-pointer transition-colors hover:bg-[#f8fafc]">
              <div className="w-1.5 h-1.5 bg-[#2563eb] rounded-full mt-[7px] shrink-0 opacity-0" />
              <div>
                <p className="text-[13px] font-medium mb-1 text-[#1e293b] leading-relaxed">Rapport mensuel disponible</p>
                <span className="text-[11px] font-medium text-[#94a3b8]">Hier</span>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
