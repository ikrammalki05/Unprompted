import React, { useState } from 'react';
import {
  FolderOpen,
  Users,
  ClipboardList,
  BarChart2,
  Bell,
  ArrowRight,
  Calendar,
  Check,
  X,
  ChevronRight,
} from 'lucide-react';

/* ─────────────── Data ─────────────── */
const stats = [
  {
    icon: FolderOpen,
    value: 12,
    label: 'Projets supervisés',
    badge: '+2 ce mois',
    iconBg: 'bg-blue-50',
    iconColor: 'text-blue-500',
    dark: false,
  },
  {
    icon: Users,
    value: 45,
    label: 'Étudiants actifs',
    badge: null,
    iconBg: 'bg-purple-50',
    iconColor: 'text-purple-500',
    dark: false,
  },
  {
    icon: ClipboardList,
    value: 8,
    label: 'Évaluations en attente',
    badge: null,
    iconBg: 'bg-orange-50',
    iconColor: 'text-orange-500',
    dark: false,
  },
  {
    icon: BarChart2,
    value: '+24',
    label: 'Activités récentes',
    badge: 'Dernières 24h',
    iconBg: 'bg-white/10',
    iconColor: 'text-white',
    dark: true,
  },
];

const projects = [
  {
    initials: 'IA',
    name: 'IA Éthique & Gouvernance',
    group: 'Groupe Master 2 – Architecture',
    pct: 82,
    color: '#3b82f6',
    avatarBg: '#dbeafe',
    avatarText: '#1d4ed8',
  },
  {
    initials: 'ML',
    name: 'Machine Learning Avancé',
    group: 'Doctorants – Lab 04',
    pct: 45,
    color: '#6366f1',
    avatarBg: '#ede9fe',
    avatarText: '#4f46e5',
  },
  {
    initials: 'DS',
    name: 'Data Science Appliquée',
    group: 'Licence 3 – TD B',
    pct: 15,
    color: '#ef4444',
    avatarBg: '#fee2e2',
    avatarText: '#dc2626',
  },
];

const approvals = [
  {
    type: 'ASSIGNATION ÉTUDIANT',
    typeColor: 'text-blue-600',
    title: 'Projet: Systèmes Autonomes',
    sub: null,
    avatars: ['#4f6ef7', '#22c55e'],
  },
  {
    type: 'VALIDATION SUJET',
    typeColor: 'text-indigo-600',
    title: 'Impact de l\'IA sur le Droit Civil',
    sub: 'Mme. Sophie Bernard',
    avatars: [],
  },
];

const deadlines = [
  {
    month: 'OCT',
    day: '24',
    title: 'Rendu Final: Mémoire M2',
    time: '14:00',
    where: 'Promotion 2024',
  },
  {
    month: 'OCT',
    day: '28',
    title: 'Soutenance: Lab AI',
    time: '09:30',
    where: 'Amphi C',
  },
  {
    month: 'NOV',
    day: '02',
    title: 'Examen mi-parcours',
    time: '11:00',
    where: 'En ligne',
  },
];

/* ─────────────── Sub-components ─────────────── */
const StatCard: React.FC<(typeof stats)[0]> = ({
  icon: Icon,
  value,
  label,
  badge,
  iconBg,
  iconColor,
  dark,
}) => (
  <div
    className={`flex-1 min-w-0 rounded-[24px] p-6 flex flex-col gap-3 ${
      dark ? 'bg-slate-900 text-white' : 'bg-white border border-slate-100'
    }`}
  >
    <div className="flex items-start justify-between gap-2">
      <div className={`w-10 h-10 rounded-xl flex items-center justify-center ${iconBg}`}>
        <Icon className={`w-5 h-5 ${iconColor}`} />
      </div>
      {badge && (
        <span
          className={`text-[10px] font-black px-2.5 py-1 rounded-full uppercase tracking-wide ${
            dark
              ? 'bg-white/10 text-white'
              : 'bg-[#dcfce7] text-[#16a34a]'
          }`}
        >
          {badge}
        </span>
      )}
    </div>
    <div>
      <p className={`text-3xl font-black leading-none mb-1 ${dark ? 'text-white' : 'text-slate-900'}`}>
        {value}
      </p>
      <p className={`text-[12px] font-semibold ${dark ? 'text-white/60' : 'text-slate-400'}`}>
        {label}
      </p>
    </div>
  </div>
);

/* ─────────────── Main Page ─────────────── */
const TableauDeBord: React.FC = () => {
  const [approved, setApproved] = useState<Record<number, boolean | null>>({});

  return (
    <div className="p-8 pb-20 max-w-7xl mx-auto animate-in fade-in duration-500">

      {/* ── Page header ── */}
      <div className="flex items-start justify-between mb-8">
        <div>
          <h1 className="text-3xl font-black text-slate-900 tracking-tight mb-1">
            Dashboard Enseignant
          </h1>
          <p className="text-[14px] text-slate-400 font-medium">Bienvenue Mr Ghailani</p>
        </div>
        <div className="flex items-center gap-4">
          <button className="relative w-9 h-9 flex items-center justify-center bg-white border border-slate-100 rounded-xl shadow-sm hover:bg-slate-50 transition-colors">
            <Bell className="w-4 h-4 text-slate-500" />
            <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-red-500 rounded-full border-2 border-white" />
          </button>
          <div className="flex items-center gap-3">
            <div className="text-right">
              <p className="text-[13px] font-black text-slate-900 leading-none mb-0.5">Ghailani</p>
              <p className="text-[10px] text-slate-400 font-semibold">Professeur Titulaire</p>
            </div>
            <div className="w-9 h-9 rounded-full bg-blue-100 flex items-center justify-center text-blue-600 font-black text-sm border-2 border-white shadow-sm">
              G
            </div>
          </div>
        </div>
      </div>

      {/* ── Stats row ── */}
      <div className="flex gap-4 mb-8">
        {stats.map((s, i) => (
          <StatCard key={i} {...s} />
        ))}
      </div>

      {/* ── Main grid ── */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">

        {/* ── Left: Aperçu du suivi ── */}
        <div className="lg:col-span-2 bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
          <div className="flex items-start justify-between mb-6">
            <div>
              <h2 className="text-[16px] font-black text-slate-900 mb-1">
                Aperçu du suivi de projet
              </h2>
              <p className="text-[12px] text-slate-400 font-medium">
                Progression moyenne par groupe de recherche
              </p>
            </div>
            <button className="flex items-center gap-1 text-[12px] font-bold text-blue-600 hover:text-blue-700 transition-colors group">
              Voir tout
              <ArrowRight className="w-3.5 h-3.5 group-hover:translate-x-0.5 transition-transform" />
            </button>
          </div>

          <div className="flex flex-col gap-4">
            {projects.map((p, i) => (
              <div
                key={i}
                className="flex items-center gap-4 p-4 bg-slate-50 rounded-2xl border border-slate-100/60 hover:bg-slate-100/50 transition-colors cursor-pointer group"
              >
                {/* Avatar */}
                <div
                  className="w-10 h-10 rounded-2xl flex items-center justify-center text-[11px] font-black flex-shrink-0"
                  style={{ background: p.avatarBg, color: p.avatarText }}
                >
                  {p.initials}
                </div>

                {/* Name + bar */}
                <div className="flex-1 min-w-0">
                  <div className="flex items-center justify-between mb-1.5">
                    <p className="text-[13px] font-black text-slate-800 truncate">{p.name}</p>
                    <span className="text-[12px] font-black text-slate-700 ml-3 flex-shrink-0">
                      {p.pct}%
                    </span>
                  </div>
                  <p className="text-[11px] text-slate-400 font-medium mb-2">{p.group}</p>
                  <div className="w-full h-1.5 bg-slate-200 rounded-full overflow-hidden">
                    <div
                      className="h-full rounded-full transition-all duration-700"
                      style={{ width: `${p.pct}%`, background: p.color }}
                    />
                  </div>
                </div>

                <ChevronRight className="w-4 h-4 text-slate-300 group-hover:text-slate-500 transition-colors flex-shrink-0" />
              </div>
            ))}
          </div>
        </div>

        {/* ── Right column ── */}
        <div className="flex flex-col gap-6">

          {/* Approbations en attente */}
          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-6">
            <div className="flex items-center gap-2 mb-5">
              <div className="w-5 h-5 rounded-full border-2 border-blue-500 flex items-center justify-center">
                <Check className="w-3 h-3 text-blue-500" strokeWidth={3} />
              </div>
              <h2 className="text-[14px] font-black text-slate-900">Approbations en attente</h2>
            </div>

            <div className="flex flex-col gap-3">
              {approvals.map((a, i) => {
                const decision = approved[i];
                return (
                  <div key={i} className="bg-slate-50 rounded-2xl border border-slate-100/60 p-4">
                    <p className={`text-[9px] font-black uppercase tracking-widest mb-2 ${a.typeColor}`}>
                      {a.type}
                    </p>
                    <p className="text-[13px] font-black text-slate-800 mb-2 leading-snug">
                      {a.title}
                    </p>
                    {a.sub && (
                      <p className="text-[11px] text-slate-400 font-medium mb-3">{a.sub}</p>
                    )}
                    {a.avatars.length > 0 && (
                      <div className="flex items-center gap-1 mb-3">
                        {a.avatars.map((color, j) => (
                          <div
                            key={j}
                            className="w-6 h-6 rounded-full border-2 border-white"
                            style={{ background: color, marginLeft: j > 0 ? -6 : 0 }}
                          />
                        ))}
                      </div>
                    )}
                    <div className="flex items-center gap-2">
                      <button
                        onClick={() => setApproved(prev => ({ ...prev, [i]: false }))}
                        className={`w-7 h-7 rounded-full border flex items-center justify-center transition-all ${
                          decision === false
                            ? 'bg-red-500 border-red-500 text-white'
                            : 'border-slate-200 text-slate-400 hover:border-red-300 hover:text-red-500'
                        }`}
                      >
                        <X className="w-3.5 h-3.5" strokeWidth={2.5} />
                      </button>
                      <button
                        onClick={() => setApproved(prev => ({ ...prev, [i]: true }))}
                        className={`w-7 h-7 rounded-full border flex items-center justify-center transition-all ${
                          decision === true
                            ? 'bg-green-500 border-green-500 text-white'
                            : 'border-slate-200 text-slate-400 hover:border-green-300 hover:text-green-500'
                        }`}
                      >
                        <Check className="w-3.5 h-3.5" strokeWidth={2.5} />
                      </button>
                      {decision !== undefined && decision !== null && (
                        <span className={`text-[10px] font-bold ml-1 ${decision ? 'text-green-600' : 'text-red-500'}`}>
                          {decision ? 'Approuvé' : 'Refusé'}
                        </span>
                      )}
                    </div>
                  </div>
                );
              })}
            </div>
          </div>

          {/* Échéances */}
          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-6">
            <div className="flex items-center justify-between mb-5">
              <h2 className="text-[14px] font-black text-slate-900">Échéances</h2>
              <Calendar className="w-4 h-4 text-slate-400" />
            </div>

            <div className="flex flex-col gap-3 mb-5">
              {deadlines.map((d, i) => (
                <div key={i} className="flex items-start gap-4">
                  {/* Date block */}
                  <div className="flex-shrink-0 w-11 text-center">
                    <p className="text-[9px] font-black text-slate-400 uppercase tracking-widest leading-none mb-0.5">
                      {d.month}
                    </p>
                    <p className="text-[20px] font-black text-slate-900 leading-none">{d.day}</p>
                  </div>
                  {/* Divider */}
                  <div className="w-px self-stretch bg-slate-100 flex-shrink-0 mx-1" />
                  {/* Info */}
                  <div className="flex-1 min-w-0">
                    <p className="text-[12px] font-black text-slate-800 leading-snug mb-0.5">
                      {d.title}
                    </p>
                    <p className="text-[11px] text-slate-400 font-medium">
                      {d.time} • {d.where}
                    </p>
                  </div>
                </div>
              ))}
            </div>

            <button className="w-full py-2.5 border border-slate-200 rounded-xl text-[12px] font-bold text-slate-600 hover:bg-slate-50 transition-colors">
              Voir le calendrier complet
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TableauDeBord;
