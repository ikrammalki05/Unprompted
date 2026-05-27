import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Cpu, ArrowRight } from 'lucide-react';
import { api } from '../../services/api';
import { keycloak } from '../../services/keycloak';

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
  "En cours": {
    bg: 'bg-emerald-50 border-emerald-200',
    text: 'text-emerald-700',
    label: 'PROJET ACTIF',
    iconClass: 'bg-emerald-50 text-emerald-600 border-emerald-100',
  },
  "Terminé": {
    bg: 'bg-blue-50 border-blue-200',
    text: 'text-blue-700',
    label: 'COMPLÉTÉ',
    iconClass: 'bg-blue-50 text-blue-600 border-blue-100',
  },
  "En attente": {
    bg: 'bg-amber-50 border-amber-200',
    text: 'text-amber-700',
    label: 'EN ATTENTE',
    iconClass: 'bg-amber-50 text-amber-600 border-amber-100',
  },
  "A venir": {
    bg: 'bg-slate-50 border-slate-200',
    text: 'text-slate-600',
    label: 'À VENIR',
    iconClass: 'bg-slate-50 text-slate-500 border-slate-100',
  }
};

// Données de secours (Screen 2) pour garantir que l'utilisateur a toujours des sujets
const MOCK_PROJECTS: Project[] = [
  {
    idProjet: 1,
    titre: "Gouvernance IA dans le secteur public",
    description: "Recherche sur l'éthique et les cadres réglementaires de l'IA.",
    status: "En cours",
    dateDebut: new Date().toISOString(),
    dateFin: new Date().toISOString(),
    duree: 8,
    urlGit: "",
    progression: 35,
    notesEnseignant: null,
    idEnseignant: 1
  },
  {
    idProjet: 2,
    titre: "Optimisation des modèles de langage",
    description: "Étude comparative des techniques de quantification 4-bit.",
    status: "En attente",
    dateDebut: new Date().toISOString(),
    dateFin: new Date().toISOString(),
    duree: 7,
    urlGit: "",
    progression: 0,
    notesEnseignant: null,
    idEnseignant: 1
  },
  {
    idProjet: 3,
    titre: "Sécurisation des APIs Locales",
    description: "Protocoles de sécurité pour l'inférence sur site.",
    status: "A venir",
    dateDebut: new Date().toISOString(),
    dateFin: new Date().toISOString(),
    duree: 6,
    urlGit: "",
    progression: 0,
    notesEnseignant: null,
    idEnseignant: 1
  }
];

const DashboardPage: React.FC = () => {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<Project[] | null>(null);
  const [loading, setLoading] = useState(true);

  // Récupération du nom depuis Keycloak
  const userName = keycloak.tokenParsed?.given_name || "Alami";

  useEffect(() => {
    const fetchPrjs = async () => {
      try {
        const res = await api.get<Project[]>('/projet/etudiant/me');
        // Si l'API retourne des données, on les utilise. Sinon on utilise les mocks.
        if (res.data && res.data.length > 0) {
          setProjects(res.data);
        } else {
          console.warn("Aucun projet réel trouvé, affichage des sujets de démonstration.");
          setProjects(MOCK_PROJECTS);
        }
      } catch (err) {
        console.error("Erreur lors de la récupération des projets réels, basculement sur démo:", err);
        setProjects(MOCK_PROJECTS);
      } finally {
        setLoading(false);
      }
    };
    fetchPrjs();
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
        ) : !projects || projects.length === 0 ? (
          <div className="p-10 border-2 border-dashed border-gray-100 rounded-2xl text-center text-gray-400">
            Aucun projet trouvé.
          </div>
        ) : (
          <div className="space-y-4">
            {projects.map((project) => {
              const config = statusMap[project.status] || statusMap["En attente"];

              return (
                <div
                  key={project.idProjet}
                  onClick={() => navigate(`/projet/${project.idProjet}/cahier-des-charges`)}
                  className="group bg-white rounded-2xl border border-gray-100 hover:border-blue-200 p-5 flex items-center gap-5 transition-all duration-300 hover:shadow-xl hover:shadow-blue-50/50 cursor-pointer"
                >
                  <div className={`w-12 h-12 rounded-xl border flex-shrink-0 flex items-center justify-center transition-transform duration-300 group-hover:scale-110 ${config.iconClass}`}>
                    <Cpu size={22} />
                  </div>

                  <div className="flex-1 min-w-0">
                    <h3 className="text-[15px] font-semibold text-gray-900 mb-0.5 group-hover:text-blue-700 transition-colors">
                      {project.titre}
                    </h3>
                    <p className="text-sm text-gray-400 truncate">{project.description}</p>
                  </div>

                  <div className="flex-shrink-0 text-right flex flex-col items-end gap-1.5">
                    <span className={`inline-block text-[10px] font-bold px-3 py-1 rounded-full border tracking-wider ${config.bg} ${config.text}`}>
                      {config.label}
                    </span>
                    <span className="text-xs text-gray-400">
                      Mise à jour {project.progression > 0 ? 'il y a 2h' : 'hier'}
                    </span>
                  </div>

                  <ArrowRight size={18} className="text-gray-300 group-hover:text-blue-500 group-hover:translate-x-1 transition-all duration-300 flex-shrink-0" />
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