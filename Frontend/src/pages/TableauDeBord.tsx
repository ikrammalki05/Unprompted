import React, { useState, useEffect } from 'react';
import {
  FolderOpen,
  Users,
  ClipboardList,
  BarChart2,
  Bell,
  Calendar,
  Check,
  X,
  ChevronRight,
} from 'lucide-react';
import userService from '../services/userService';
import projetService from '../services/projetService';

interface StatItem {
  icon: React.ElementType;
  value: string | number;
  label: string;
  badge?: string | null;
  iconBg: string;
  iconColor: string;
  dark?: boolean;
}

/* ─────────────── Sub-components ─────────────── */
const StatCard: React.FC<StatItem> = ({
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
interface TableauDeBordProps {
  onProjectClick?: (id: number) => void;
}

const TableauDeBord: React.FC<TableauDeBordProps> = ({ onProjectClick }) => {
  const [loading, setLoading] = useState(true);
  const [profile, setProfile] = useState<any>(null);
  const [realStats, setRealStats] = useState<any>(null);
  const [realProjects, setRealProjects] = useState<any[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        // 1. Get Profile
        const userProfile = await userService.getCurrentUser();
        setProfile(userProfile);

        // 2. Get Stats
        const statsData = await userService.getTeacherStats(userProfile.id);
        setRealStats(statsData);

        // 3. Get Projects
        const projectsData = await projetService.getByEnseignant(userProfile.id);
        setRealProjects(projectsData);
      } catch (error) {
        console.error("Erreur lors du chargement des données", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (!profile) {
    return (
      <div className="p-8 text-center">
        <p className="text-slate-500">Erreur lors de la récupération du profil. Veuillez vous reconnecter.</p>
        <button onClick={() => window.location.reload()} className="mt-4 px-4 py-2 bg-blue-600 text-white rounded-lg">Réessayer</button>
      </div>
    );
  }

  const dynamicStats = [
    {
      icon: FolderOpen,
      value: realStats?.projetsSupervisés || 0,
      label: 'Projets supervisés',
      badge: null,
      iconBg: 'bg-blue-50',
      iconColor: 'text-blue-500',
      dark: false,
    },
    {
      icon: Users,
      value: realStats?.étudiantsActifs || 0,
      label: 'Étudiants actifs',
      badge: null,
      iconBg: 'bg-purple-50',
      iconColor: 'text-purple-500',
      dark: false,
    },
    {
      icon: ClipboardList,
      value: realStats?.évaluationsEnAttente || 0,
      label: 'Évaluations en attente',
      badge: null,
      iconBg: 'bg-orange-50',
      iconColor: 'text-orange-500',
      dark: false,
    },
    {
      icon: BarChart2,
      value: realStats?.activitésRécentes || 0,
      label: 'Activités récentes',
      badge: 'Dernières 24h',
      iconBg: 'bg-white/10',
      iconColor: 'text-white',
      dark: true,
    },
  ];

  return (
    <div className="p-8 pb-20 max-w-7xl mx-auto animate-in fade-in duration-500">

      {/* ── Page header ── */}
      <div className="flex items-start justify-between mb-8">
        <div>
          <h1 className="text-3xl font-black text-slate-900 tracking-tight mb-1">
            Dashboard Enseignant
          </h1>
          <p className="text-[14px] text-slate-400 font-medium">Bienvenue {profile?.nomComplet || '...'}</p>
        </div>
        <div className="flex items-center gap-4">
          <button className="relative w-9 h-9 flex items-center justify-center bg-white border border-slate-100 rounded-xl shadow-sm hover:bg-slate-50 transition-colors">
            <Bell className="w-4 h-4 text-slate-500" />
            <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-red-500 rounded-full border-2 border-white" />
          </button>
          <div className="flex items-center gap-3">
            <div className="text-right">
              <p className="text-[13px] font-black text-slate-900 leading-none mb-0.5">{profile?.nomComplet?.split(' ')[1] || '...'}</p>
              <p className="text-[10px] text-slate-400 font-semibold">{profile?.specialite || 'Enseignant'}</p>
            </div>
            <div className="w-9 h-9 rounded-full bg-blue-100 flex items-center justify-center text-blue-600 font-black text-sm border-2 border-white shadow-sm">
              {profile?.nomComplet?.[0] || 'U'}
            </div>
          </div>
        </div>
      </div>

      {/* ── Stats row ── */}
      <div className="flex gap-4 mb-8">
        {dynamicStats.map((s, i) => (
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
                Progression réelle de vos projets
              </p>
            </div>
          </div>

          <div className="flex flex-col gap-4">
            {realProjects.length > 0 ? realProjects.map((p, i) => (
              <div
                key={i}
                onClick={p.id ? () => onProjectClick?.(p.id) : undefined}
                className="flex items-center gap-4 p-4 bg-slate-50 rounded-2xl border border-slate-100/60 hover:bg-slate-100/50 transition-colors cursor-pointer group"
              >
                {/* Avatar */}
                <div
                  className="w-10 h-10 rounded-2xl flex items-center justify-center text-[11px] font-black flex-shrink-0 bg-blue-100 text-blue-600"
                >
                  {p.titre?.[0] || 'P'}
                </div>

                {/* Name + bar */}
                <div className="flex-1 min-w-0">
                  <div className="flex items-center justify-between mb-1.5">
                    <p className="text-[13px] font-black text-slate-800 truncate">{p.titre}</p>
                    <span className="text-[12px] font-black text-slate-700 ml-3 flex-shrink-0">
                      {p.progression || 0}%
                    </span>
                  </div>
                  <p className="text-[11px] text-slate-400 font-medium mb-2">{p.statut}</p>
                  <div className="w-full h-1.5 bg-slate-200 rounded-full overflow-hidden">
                    <div
                      className="h-full bg-blue-500 rounded-full transition-all duration-700"
                      style={{ width: `${p.progression || 0}%` }}
                    />
                  </div>
                </div>

                <ChevronRight className="w-4 h-4 text-slate-300 group-hover:text-slate-500 transition-colors flex-shrink-0" />
              </div>
            )) : (
                <div className="text-center py-10 text-slate-400 font-medium">
                    Aucun projet trouvé pour cet enseignant.
                </div>
            )}
          </div>
        </div>

        {/* ── Right column ── */}
        <div className="flex flex-col gap-6">

          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-6">
            <div className="flex items-center gap-2 mb-5">
              <div className="w-5 h-5 rounded-full border-2 border-blue-500 flex items-center justify-center">
                <Check className="w-3 h-3 text-blue-500" strokeWidth={3} />
              </div>
              <h2 className="text-[14px] font-black text-slate-900">Projets à suivre</h2>
            </div>

            <div className="flex flex-col gap-3">
              {realProjects.length === 0 ? (
                <p className="text-sm text-slate-400">Aucun projet pour le moment.</p>
              ) : (
                realProjects.slice(0, 4).map((p: any) => (
                  <button
                    key={p.idProjet ?? p.IdProjet}
                    onClick={() => onProjectClick?.(p.idProjet ?? p.IdProjet)}
                    className="text-left bg-slate-50 rounded-2xl border border-slate-100/60 p-4 hover:border-blue-200"
                  >
                    <p className="text-[9px] font-black uppercase tracking-widest mb-2 text-blue-600">
                      {p.status ?? p.Status ?? 'Projet'}
                    </p>
                    <p className="text-[13px] font-black text-slate-800 mb-1 leading-snug">
                      {p.titre ?? p.Titre}
                    </p>
                    <p className="text-[11px] text-slate-400 font-medium">
                      Progression {p.progression ?? p.Progression ?? 0}%
                    </p>
                  </button>
                ))
              )}
            </div>
          </div>

          <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-6">
            <div className="flex items-center justify-between mb-5">
              <h2 className="text-[14px] font-black text-slate-900">Échéances projets</h2>
              <Calendar className="w-4 h-4 text-slate-400" />
            </div>

            <div className="flex flex-col gap-3">
              {realProjects
                .filter((p: any) => p.dateFin || p.DateFin)
                .slice(0, 5)
                .map((p: any) => {
                  const d = new Date(p.dateFin ?? p.DateFin);
                  return (
                    <div key={`due-${p.idProjet ?? p.IdProjet}`} className="flex items-start gap-4">
                      <div className="flex-shrink-0 w-11 text-center">
                        <p className="text-[9px] font-black text-slate-400 uppercase tracking-widest leading-none mb-0.5">
                          {d.toLocaleString('fr-FR', { month: 'short' })}
                        </p>
                        <p className="text-[20px] font-black text-slate-900 leading-none">{d.getDate()}</p>
                      </div>
                      <div className="w-px self-stretch bg-slate-100 flex-shrink-0 mx-1" />
                      <div className="flex-1 min-w-0">
                        <p className="text-[12px] font-black text-slate-800 leading-snug mb-0.5">
                          {p.titre ?? p.Titre}
                        </p>
                        <p className="text-[11px] text-slate-400 font-medium">
                          Fin prévue · {d.toLocaleDateString('fr-FR')}
                        </p>
                      </div>
                    </div>
                  );
                })}
              {realProjects.filter((p: any) => p.dateFin || p.DateFin).length === 0 && (
                <p className="text-sm text-slate-400">Aucune échéance renseignée.</p>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TableauDeBord;
