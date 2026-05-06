import React, { useState } from 'react';
import { X, Info, Check } from 'lucide-react';

/* ── Constants ── */
const ROLES = ['Frontend', 'Backend', 'CI/CD', 'Test Qualité', 'Infrastructure'];

const STUDENTS = [
  'Marie Curie',
  'Jean Dupont',
  'Alice Martin',
  'Karim Benali',
  'Sofia Larbi',
];

/* ── Main Page ── */
const GestionProjets: React.FC = () => {
  const [titre, setTitre] = useState('');
  const [selectedRoles, setSelectedRoles] = useState<string[]>([]);
  const [autreRole, setAutreRole] = useState('');
  const [deadline, setDeadline] = useState('');
  const [semaines, setSemaines] = useState('');
  const [selectedStudents, setSelectedStudents] = useState<string[]>([]);
  const [dropdownOpen, setDropdownOpen] = useState(false);

  const toggleRole = (role: string) => {
    setSelectedRoles(prev =>
      prev.includes(role) ? prev.filter(r => r !== role) : [...prev, role]
    );
  };

  const toggleStudent = (s: string) => {
    setSelectedStudents(prev =>
      prev.includes(s) ? prev.filter(x => x !== s) : [...prev, s]
    );
  };

  return (
    <div className="min-h-screen flex items-start justify-center pt-10 pb-20 px-4 animate-in fade-in duration-500">
      {/* ── Form card ── */}
      <div className="w-full max-w-[540px] bg-white rounded-[28px] shadow-xl border border-slate-100 overflow-hidden">

        {/* ── Header ── */}
        <div className="px-8 pt-8 pb-6 border-b border-slate-100 flex items-start justify-between">
          <div>
            <h1 className="text-2xl font-black text-slate-900 tracking-tight mb-1">
              Nouveau Projet
            </h1>
            <p className="text-[13px] text-slate-400 font-medium">
              Créez une nouvelle opportunité académique pour vos étudiants.
            </p>
          </div>
          <button className="w-8 h-8 flex items-center justify-center rounded-xl bg-slate-100 hover:bg-slate-200 transition-colors text-slate-500 ml-4 flex-shrink-0">
            <X className="w-4 h-4" />
          </button>
        </div>

        {/* ── Body ── */}
        <div className="px-8 py-6 flex flex-col gap-6">

          {/* Titre du projet */}
          <div className="flex flex-col gap-2">
            <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest">
              Titre du projet
            </label>
            <input
              type="text"
              className="w-full px-4 py-3 bg-slate-50 border border-slate-100 rounded-2xl text-[13px] text-slate-800 font-semibold placeholder-slate-300 focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all"
              placeholder="Ex: Développement d'une interface de gestion prédictive..."
              value={titre}
              onChange={e => setTitre(e.target.value)}
            />
          </div>

          {/* Rôles requis */}
          <div className="flex flex-col gap-3">
            <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest">
              Rôles requis
            </label>
            <div className="grid grid-cols-2 gap-2">
              {ROLES.map(role => {
                const active = selectedRoles.includes(role);
                return (
                  <button
                    key={role}
                    onClick={() => toggleRole(role)}
                    className={`flex items-center gap-2.5 px-4 py-2.5 rounded-xl border text-[13px] font-semibold transition-all text-left ${
                      active
                        ? 'bg-blue-50 border-blue-200 text-blue-700'
                        : 'bg-slate-50 border-slate-100 text-slate-600 hover:border-slate-200'
                    }`}
                  >
                    <span
                      className={`w-4 h-4 rounded-full border-2 flex items-center justify-center flex-shrink-0 transition-colors ${
                        active ? 'border-blue-600 bg-blue-600' : 'border-slate-300'
                      }`}
                    >
                      {active && <span className="w-1.5 h-1.5 bg-white rounded-full" />}
                    </span>
                    {role}
                  </button>
                );
              })}
            </div>
            {/* Autre rôle */}
            <input
              type="text"
              className="w-full px-4 py-2.5 bg-slate-50 border border-slate-100 rounded-xl text-[13px] text-slate-700 font-medium placeholder-slate-300 focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all"
              placeholder="Autre rôle..."
              value={autreRole}
              onChange={e => setAutreRole(e.target.value)}
            />
          </div>

          {/* Durée / Échéance */}
          <div className="flex flex-col gap-2">
            <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest">
              Durée / Échéance
            </label>
            <div className="flex gap-3">
              <input
                type="date"
                className="flex-1 px-4 py-3 bg-slate-50 border border-slate-100 rounded-2xl text-[13px] text-slate-500 font-medium focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all"
                value={deadline}
                onChange={e => setDeadline(e.target.value)}
              />
              <div className="relative">
                <input
                  type="number"
                  min={1}
                  className="w-32 px-4 py-3 bg-slate-50 border border-slate-100 rounded-2xl text-[13px] text-slate-700 font-semibold focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all pr-16"
                  placeholder=""
                  value={semaines}
                  onChange={e => setSemaines(e.target.value)}
                />
                <span className="absolute right-4 top-1/2 -translate-y-1/2 text-[11px] text-slate-400 font-semibold pointer-events-none">
                  Semaines
                </span>
              </div>
            </div>
          </div>

          {/* Assignation des étudiants */}
          <div className="flex flex-col gap-2">
            <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest">
              Assignation des étudiants au groupe
            </label>

            {/* Custom dropdown */}
            <div className="relative">
              <button
                onClick={() => setDropdownOpen(v => !v)}
                className="w-full flex items-center justify-between px-4 py-3 bg-slate-50 border border-slate-100 rounded-2xl text-[13px] font-medium hover:bg-white hover:border-slate-200 transition-all focus:outline-none focus:ring-2 focus:ring-blue-100"
              >
                <span className={selectedStudents.length > 0 ? 'text-slate-800 font-semibold' : 'text-slate-400'}>
                  {selectedStudents.length > 0
                    ? selectedStudents.join(', ')
                    : 'Sélectionner des étudiants...'}
                </span>
                <svg
                  className={`w-4 h-4 text-slate-400 transition-transform ${dropdownOpen ? 'rotate-180' : ''}`}
                  fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}
                >
                  <path strokeLinecap="round" strokeLinejoin="round" d="M19 9l-7 7-7-7" />
                </svg>
              </button>

              {dropdownOpen && (
                <div className="absolute z-20 mt-2 w-full bg-white rounded-2xl border border-slate-100 shadow-xl overflow-hidden">
                  {STUDENTS.map(s => {
                    const sel = selectedStudents.includes(s);
                    return (
                      <button
                        key={s}
                        onClick={() => toggleStudent(s)}
                        className={`w-full flex items-center gap-3 px-4 py-3 text-[13px] font-semibold transition-colors ${
                          sel ? 'bg-blue-50 text-blue-700' : 'text-slate-700 hover:bg-slate-50'
                        }`}
                      >
                        <span
                          className={`w-4 h-4 rounded-[5px] border flex items-center justify-center flex-shrink-0 transition-colors ${
                            sel ? 'bg-blue-600 border-blue-600' : 'border-slate-300'
                          }`}
                        >
                          {sel && <Check className="w-2.5 h-2.5 text-white" strokeWidth={3} />}
                        </span>
                        {s}
                      </button>
                    );
                  })}
                </div>
              )}
            </div>

            {/* Info note */}
            <div className="flex gap-3 bg-slate-50 border border-slate-100 rounded-2xl p-4 mt-1">
              <Info className="w-4 h-4 text-slate-400 flex-shrink-0 mt-0.5" />
              <p className="text-[11px] text-slate-400 font-medium leading-relaxed">
                Une fois qu'un étudiant est assigné à un rôle spécifique, il est automatiquement
                retiré de la liste des étudiants disponibles pour d'autres projets afin d'éviter
                les surcharges de travail.
              </p>
            </div>
          </div>
        </div>

        {/* ── Footer ── */}
        <div className="px-8 pb-8 pt-2 flex flex-col gap-3">
          <button className="w-full py-4 bg-slate-900 hover:bg-slate-800 active:scale-[0.99] text-white text-[14px] font-black rounded-2xl transition-all shadow-lg shadow-slate-900/10 tracking-tight">
            Créer et Notifier le groupe
          </button>
          <button className="w-full py-2.5 text-[13px] font-semibold text-slate-400 hover:text-slate-600 transition-colors text-center">
            Enregistrer en tant que brouillon
          </button>
          <div className="flex items-center justify-center gap-2 pt-1">
            <span className="flex items-center gap-1.5 text-[10px] font-black text-[#16a34a] uppercase tracking-widest">
              <span className="w-3.5 h-3.5 rounded-full border-2 border-[#22c55e] flex items-center justify-center">
                <Check className="w-2 h-2 text-[#22c55e]" strokeWidth={3} />
              </span>
              Conforme au système de gouvernance Unprompted
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default GestionProjets;