import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Cpu, ArrowRight, Monitor } from 'lucide-react';
import { api } from '../../services/api';
import { useCurrentUserDisplay } from '../../hooks/useCurrentUserDisplay';

interface Project {
  idProjet: number;
  titre: string;
  description: string;
  status: string;
  dateDebut: string;
  dateFin: string;
  duree: number;
  urlGit: string;
  progression: number;
  notesEnseignant: string | null;
  idEnseignant: number;
}

const statusMap: Record<string, { bg: string; text: string; label: string; iconClass: string }> = {
  'En cours': {
    bg: 'bg-emerald-50 border-emerald-200',
    text: 'text-emerald-700',
    label: 'PROJET ACTIF',
    iconClass: 'bg-emerald-50 text-emerald-600 border-emerald-100',
  },
  Terminé: {
    bg: 'bg-blue-50 border-blue-200',
    text: 'text-blue-700',
    label: 'COMPLÉTÉ',
    iconClass: 'bg-blue-50 text-blue-600 border-blue-100',
  },
  'En attente': {
    bg: 'bg-amber-50 border-amber-200',
    text: 'text-amber-700',
    label: 'EN ATTENTE',
    iconClass: 'bg-amber-50 text-amber-600 border-amber-100',
  },
  'A venir': {
    bg: 'bg-slate-50 border-slate-200',
    text: 'text-slate-600',
    label: 'À VENIR',
    iconClass: 'bg-slate-50 text-slate-500 border-slate-100',
  },
};

function normalizeProject(raw: any): Project {
  return {
    idProjet: raw.idProjet ?? raw.IdProjet ?? raw.iDProjet ?? 0,
    titre: raw.titre ?? raw.Titre ?? 'Sans titre',
    description: raw.description ?? raw.Description ?? '',
    status: raw.status ?? raw.Status ?? 'En attente',
    dateDebut: raw.dateDebut ?? raw.DateDebut ?? '',
    dateFin: raw.dateFin ?? raw.DateFin ?? '',
    duree: raw.duree ?? raw.Duree ?? 0,
    urlGit: raw.urlGit ?? raw.UrlGit ?? '',
    progression: raw.progression ?? raw.Progression ?? 0,
    notesEnseignant: raw.notesEnseignant ?? raw.NotesEnseignant ?? null,
    idEnseignant: raw.idEnseignant ?? raw.IdEnseignant ?? 0,
  };
}

const DashboardPage: React.FC = () => {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const user = useCurrentUserDisplay();
  const userName = user.firstName || user.fullName;

  useEffect(() => {
    const fetchPrjs = async () => {
      try {
        const res = await api.get('/projet/etudiant/me');
        const list = Array.isArray(res.data) ? res.data.map(normalizeProject) : [];
        setProjects(list);
      } catch (err: any) {
        console.error(err);
        setError(err?.response?.data?.message || 'Impossible de charger vos projets.');
        setProjects([]);
      } finally {
        setLoading(false);
      }
    };
    void fetchPrjs();
  }, []);

  return (
    <div className="p-8 max-w-4xl animate-fadeIn">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-1">Bonjour, {userName}</h1>
        <p className="text-gray-500 text-sm">Voici vos projets de recherche assignés.</p>
      </div>

      <div>
        <h2 className="text-lg font-bold text-gray-800 mb-5 flex items-center gap-2">
          <span className="bg-blue-100 p-1 rounded-lg">📋</span> Projets Assignés
        </h2>

        {loading ? (
          <div className="p-10 text-center text-gray-400">Chargement de vos projets...</div>
        ) : error ? (
          <div className="p-10 border border-red-100 rounded-2xl text-center text-red-500">{error}</div>
        ) : projects.length === 0 ? (
          <div className="p-10 border-2 border-dashed border-gray-100 rounded-2xl text-center text-gray-400">
            Aucun projet assigné pour le moment.
          </div>
        ) : (
          <div className="space-y-4">
            {projects.map((project) => {
              const config = statusMap[project.status] || statusMap['En attente'];
              return (
                <div
                  key={project.idProjet}
                  className="group bg-white rounded-2xl border border-gray-100 hover:border-blue-200 p-5 flex items-center gap-5 transition-all duration-300 hover:shadow-xl hover:shadow-blue-50/50"
                >
                  <div
                    className={`w-12 h-12 rounded-xl border flex-shrink-0 flex items-center justify-center ${config.iconClass}`}
                  >
                    <Cpu size={22} />
                  </div>
                  <div className="flex-1 min-w-0">
                    <div className="flex items-center gap-2 mb-1">
                      <h3 className="font-bold text-gray-900 truncate">{project.titre}</h3>
                      <span className={`text-[10px] font-bold px-2 py-0.5 rounded-full border ${config.bg} ${config.text}`}>
                        {config.label}
                      </span>
                    </div>
                    <p className="text-sm text-gray-500 line-clamp-2">{project.description}</p>
                    <div className="mt-2 text-xs text-gray-400">Progression: {project.progression ?? 0}%</div>
                  </div>
                  <div className="flex flex-col gap-2">
                    <button
                      onClick={() => navigate(`/etudiant/projet/${project.idProjet}/cahier-des-charges`)}
                      className="inline-flex items-center gap-1 text-sm text-blue-600 font-semibold"
                    >
                      Cahier <ArrowRight size={14} />
                    </button>
                    <button
                      onClick={() => navigate(`/etudiant/workspace/${project.idProjet}`)}
                      className="inline-flex items-center gap-1 text-sm text-indigo-600 font-semibold"
                    >
                      <Monitor size={14} /> Workspace
                    </button>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
};

export default DashboardPage;
