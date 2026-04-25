import React, { useState } from 'react';
import {
  ArrowLeft,
  Calendar,
  CheckCircle2,
  Circle,
  FileText,
  GitCommit,
  Mail,
  Clock,
} from 'lucide-react';

/* ────────────── Types ────────────── */
interface Student { initials: string; name: string; color: string }
interface Step    { id: number; label: string; status: 'done' | 'in-progress' | 'todo' }
interface Activity{ avatar: string; name: string; action: string; file?: string; time: string }
interface Commit  { message: string; author: string; date: string; branch: string; branchColor: string }

/* ────────────── Data ────────────── */
const students: Student[] = [
  { initials: 'MC', name: 'Marie Curie',  color: '#4f6ef7' },
  { initials: 'JD', name: 'Jean Dupont',  color: '#22c55e' },
];

const steps: Step[] = [
  { id: 1, label: 'Preprocessing',  status: 'done' },
  { id: 2, label: 'Model Setup',    status: 'done' },
  { id: 3, label: 'Training',       status: 'in-progress' },
  { id: 4, label: 'Evaluation',     status: 'todo' },
];

const activities: Activity[] = [
  { avatar: 'MC', name: 'Marie Curie',  action: 'a mis à jour', file: 'main.py',              time: 'il y a 2h' },
  { avatar: 'JD', name: 'Jean Dupont',  action: 'a ajouté un document', file: '"Rapport_Etape_1.pdf"', time: 'il y a 5h' },
];

const commits: Commit[] = [
  { message: 'Feat: Add quantization layer', author: 'Marie Curie',  date: '10/05/2024', branch: 'main', branchColor: '#6366f1' },
  { message: 'Fix: Model inference bug',     author: 'Jean Dupont',  date: '09/05/2024', branch: 'dev',  branchColor: '#f59e0b' },
];

const progress = 65;

/* ────────────── Sub-components ────────────── */
const StepBadge: React.FC<{ status: Step['status'] }> = ({ status }) => {
  const map = {
    done:        { label: 'TERMINÉ',   cls: 'bg-[#dcfce7] text-[#16a34a]' },
    'in-progress':{ label: 'EN COURS', cls: 'bg-[#dbeafe] text-[#2563eb]' },
    todo:        { label: 'À FAIRE',   cls: 'bg-slate-100 text-slate-400' },
  } as const;
  const { label, cls } = map[status];
  return <span className={`text-[10px] font-black px-2.5 py-1 rounded-full uppercase tracking-wider ${cls}`}>{label}</span>;
};

const StepIcon: React.FC<{ status: Step['status'] }> = ({ status }) => {
  if (status === 'done')        return <CheckCircle2 className="w-5 h-5 text-[#22c55e]" fill="#dcfce7" />;
  if (status === 'in-progress') return <Circle       className="w-5 h-5 text-[#3b82f6]" fill="#dbeafe" />;
  return                               <Circle       className="w-5 h-5 text-slate-300" />;
};

/* ────────────── Main page ────────────── */
const ProjetDetails: React.FC = () => {
  const [note, setNote] = useState('');

  return (
    <div className="p-8 pb-20 max-w-7xl mx-auto animate-in fade-in duration-500">

      {/* ── Breadcrumb ── */}
      <div className="flex items-center gap-2 text-[12px] font-semibold text-slate-400 mb-4 uppercase tracking-widest">
        <span className="cursor-pointer hover:text-blue-600 transition-colors">PROJETS</span>
        <span>/</span>
        <span className="text-slate-600">OPTIMISATION DE LLM LOCAL</span>
      </div>

      {/* ── Header row ── */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-8">
        <div className="flex items-center gap-4">
          <button className="w-9 h-9 flex items-center justify-center bg-white rounded-xl border border-slate-100 shadow-sm hover:bg-slate-50 transition-colors">
            <ArrowLeft className="w-4 h-4 text-slate-500" />
          </button>
          <h1 className="text-3xl font-black text-slate-900 tracking-tight">Optimisation de LLM local</h1>
        </div>
        <div className="flex items-center gap-3">
          <span className="flex items-center gap-1.5 px-3 py-1.5 bg-[#dcfce7] text-[#16a34a] text-[11px] font-black rounded-full uppercase tracking-wide">
            <span className="w-1.5 h-1.5 rounded-full bg-[#22c55e] inline-block" />
            En cours
          </span>
          <span className="flex items-center gap-1.5 px-3 py-1.5 bg-white border border-slate-100 rounded-full text-[11px] font-semibold text-slate-500 shadow-sm">
            <Calendar className="w-3.5 h-3.5" />
            Deadline: 15 Oct 2024
          </span>
        </div>
      </div>

      {/* ── Student avatars ── */}
      <div className="flex items-center gap-2 mb-8">
        {students.map(s => (
          <div key={s.initials} className="flex items-center gap-2 px-3 py-1.5 bg-white border border-slate-100 rounded-full shadow-sm">
            <div
              className="w-6 h-6 rounded-full flex items-center justify-center text-white text-[10px] font-black"
              style={{ background: s.color }}
            >
              {s.initials}
            </div>
          </div>
        ))}
        <span className="text-[13px] font-semibold text-slate-600 ml-1">
          Étudiants :&nbsp;
          <span className="font-black text-slate-800">{students.map(s => s.name).join(', ')}</span>
        </span>
      </div>

      {/* ── Tab row (static) ── */}
      <div className="flex gap-6 border-b border-slate-100 mb-8">
        {['Vue d\'ensemble', 'Activité', 'Historique des commits', 'Documents', 'Progression'].map((tab, i) => (
          <button
            key={tab}
            className={`pb-3 text-[13px] font-bold tracking-tight transition-colors border-b-2 ${
              i === 0
                ? 'border-blue-600 text-blue-600'
                : 'border-transparent text-slate-400 hover:text-slate-600'
            }`}
          >
            {tab}
          </button>
        ))}
      </div>

      {/* ── Main content grid ── */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">

        {/* ── Left / main column ── */}
        <div className="lg:col-span-2 flex flex-col gap-6">

          {/* Progression card */}
          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-[15px] font-black text-slate-900">Progression</h2>
              <span className="text-[13px] font-black text-blue-600">{progress}% complété</span>
            </div>

            {/* Bar */}
            <div className="w-full h-2.5 bg-slate-100 rounded-full mb-6 overflow-hidden">
              <div
                className="h-full bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full transition-all duration-700"
                style={{ width: `${progress}%` }}
              />
            </div>

            {/* Steps grid */}
            <div className="grid grid-cols-2 gap-3">
              {steps.map(step => (
                <div
                  key={step.id}
                  className="flex items-center gap-3 p-3 bg-slate-50 rounded-2xl border border-slate-100/60"
                >
                  <StepIcon status={step.status} />
                  <span className="flex-1 text-[13px] font-semibold text-slate-700">{step.label}</span>
                  <StepBadge status={step.status} />
                </div>
              ))}
            </div>
          </div>

          {/* Activité récente */}
          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
            <h2 className="text-[15px] font-black text-slate-900 mb-5">Activité récente</h2>
            <div className="space-y-4">
              {activities.map((a, i) => {
                const student = students.find(s => s.initials === a.avatar);
                return (
                  <div key={i} className="flex items-start gap-3 group">
                    <div
                      className="w-8 h-8 rounded-full flex items-center justify-center text-white text-[10px] font-black flex-shrink-0 mt-0.5"
                      style={{ background: student?.color ?? '#94a3b8' }}
                    >
                      {a.avatar}
                    </div>
                    <div className="flex-1 flex items-start justify-between gap-2">
                      <p className="text-[13px] text-slate-600 leading-snug">
                        <span className="font-black text-slate-900">{a.name}</span>
                        {' '}{a.action}{' '}
                        {a.file && (
                          <code className="bg-slate-100 text-blue-700 px-1.5 py-0.5 rounded-lg text-[11px] font-mono font-bold">
                            {a.file}
                          </code>
                        )}
                      </p>
                      <span className="flex items-center gap-1 text-[11px] text-slate-400 font-semibold whitespace-nowrap flex-shrink-0">
                        <Clock className="w-3 h-3" />
                        {a.time}
                      </span>
                    </div>
                  </div>
                );
              })}
            </div>
          </div>

          {/* Historique des commits */}
          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
            <h2 className="text-[15px] font-black text-slate-900 mb-5">Historique des commits</h2>
            <div className="overflow-x-auto">
              <table className="w-full text-[12px]">
                <thead>
                  <tr className="border-b border-slate-100">
                    {['MESSAGE', 'AUTEUR', 'DATE', 'BRANCHE'].map(h => (
                      <th key={h} className="pb-3 text-left font-black text-slate-400 uppercase tracking-widest pr-4 last:pr-0">{h}</th>
                    ))}
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-50">
                  {commits.map((c, i) => (
                    <tr key={i} className="group hover:bg-slate-50/60 transition-colors">
                      <td className="py-3.5 pr-4 font-semibold text-slate-700 flex items-center gap-2">
                        <GitCommit className="w-3.5 h-3.5 text-slate-300 flex-shrink-0" />
                        {c.message}
                      </td>
                      <td className="py-3.5 pr-4 text-slate-600 font-medium">{c.author}</td>
                      <td className="py-3.5 pr-4 text-slate-500">{c.date}</td>
                      <td className="py-3.5">
                        <span
                          className="px-2.5 py-1 rounded-lg text-white text-[10px] font-black"
                          style={{ background: c.branchColor }}
                        >
                          {c.branch}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>

        {/* ── Right column ── */}
        <div className="flex flex-col gap-6">

          {/* Notes de l'enseignant */}
          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
            <div className="flex items-center gap-2 mb-5">
              <FileText className="w-4 h-4 text-slate-400" />
              <h2 className="text-[15px] font-black text-slate-900">Notes de l'enseignant</h2>
            </div>
            <textarea
              className="w-full h-36 resize-none bg-slate-50 border border-slate-100 rounded-2xl p-4 text-[13px] text-slate-700 placeholder-slate-300 font-medium focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all"
              placeholder="Ajouter une observation..."
              value={note}
              onChange={e => setNote(e.target.value)}
            />
            <button className="mt-4 w-full py-3 bg-slate-900 hover:bg-slate-800 text-white text-[13px] font-black rounded-2xl transition-all shadow-md shadow-slate-900/10 active:scale-[0.98]">
              Enregistrer
            </button>
          </div>

          {/* Contact étudiants */}
          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
            <h2 className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-5">Contact étudiants</h2>
            <div className="space-y-3">
              {students.map(s => (
                <div key={s.initials} className="flex items-center justify-between p-3 bg-slate-50 rounded-2xl border border-slate-100/60 hover:bg-blue-50/40 transition-colors group cursor-pointer">
                  <div className="flex items-center gap-3">
                    <div
                      className="w-8 h-8 rounded-full flex items-center justify-center text-white text-[10px] font-black"
                      style={{ background: s.color }}
                    >
                      {s.initials}
                    </div>
                    <span className="text-[13px] font-bold text-slate-800">{s.name}</span>
                  </div>
                  <Mail className="w-4 h-4 text-slate-300 group-hover:text-blue-500 transition-colors" />
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProjetDetails;
