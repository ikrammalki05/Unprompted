import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Cpu, ArrowRight } from 'lucide-react';
import { api } from '../../services/api';
import { keycloak } from '../../services/keycloak';

const studentId = keycloak.subject;

interface Project {
  idProjet: number;
  titre: string;
  description: string;
  status: string; // Changed to string to match your Postman output
  dateDebut: string;
  dateFin: string;
  duree: number;
  urlGit: string;
  progression: number;
  notesEnseignant: number | null;
  idEnseignant: number;
}

// Map your .NET strings to your Tailwind styles
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
};

const DashboardPage: React.FC = () => {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<Project[] | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPrjs = async () => {
      try {
        const res = await api.get<Project[]>(`/projet/etudiant/${studentId}`);
        setProjects(res.data);
      } catch (err) {
        console.error("Erreur lors de la récupération:", err);
      } finally {
        setLoading(false);
      }
    };
    if (studentId) fetchPrjs();
  }, []);

  return (
    <div className="p-8 max-w-4xl animate-fadeIn">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-1">Bonjour, Alami</h1>
        <p className="text-gray-500 text-sm">Voici vos projets de recherche assignés.</p>
      </div>

      <div>
        <h2 className="text-lg font-bold text-gray-800 mb-5 flex items-center gap-2">
          <span>📋</span> Projets Assignés
        </h2>

        {loading ? (
          <div className="p-10 text-center text-gray-400">Chargement des projets...</div>
        ) : !projects || projects.length === 0 ? (
          <div className="p-10 border-2 border-dashed border-gray-100 rounded-2xl text-center text-gray-400">
            Aucun projet assigné pour le moment.
          </div>
        ) : (
          <div className="space-y-4">
            {projects.map((project) => {
              // Fallback to "En attente" if the status string doesn't match our map
              const config = statusMap[project.status] || statusMap["En attente"];

              return (
                <div
                  key={project.idProjet}
                  onClick={() => navigate(`/projet/${project.idProjet}/cahier-des-charges`)}
                  className="group bg-white rounded-2xl border border-gray-100 hover:border-gray-200 p-5 flex items-center gap-5 transition-all duration-300 hover:shadow-lg hover:shadow-gray-100/80 cursor-pointer"
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
                      Initialisé le {new Date(project.dateDebut).toLocaleDateString()}
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