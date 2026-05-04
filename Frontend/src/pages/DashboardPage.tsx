import React from 'react';
import { useNavigate } from 'react-router-dom';
import { BookOpen, Cpu, Shield, ArrowRight } from 'lucide-react';

interface ProjectStatus {
  label: string;
  color: 'active' | 'pending' | 'upcoming';
}

interface Project {
  id: number;
  title: string;
  description: string;
  icon: React.ReactNode;
  status: ProjectStatus;
  meta: string;
}

const statusStyles: Record<string, { bg: string; text: string; label: string }> = {
  active: {
    bg: 'bg-emerald-50 border-emerald-200',
    text: 'text-emerald-700',
    label: 'PROJET ACTIF',
  },
  pending: {
    bg: 'bg-amber-50 border-amber-200',
    text: 'text-amber-700',
    label: 'EN ATTENTE',
  },
  upcoming: {
    bg: 'bg-blue-50 border-blue-200',
    text: 'text-blue-700',
    label: 'À VENIR',
  },
};

const iconWrapperStyles: Record<string, string> = {
  active: 'bg-emerald-50 text-emerald-600 border-emerald-100',
  pending: 'bg-blue-50 text-blue-600 border-blue-100',
  upcoming: 'bg-violet-50 text-violet-600 border-violet-100',
};

const projects: Project[] = [
  {
    id: 1,
    title: 'Gouvernance IA dans le secteur public',
    description: "Recherche sur l'éthique et les cadres réglementaires de l'IA.",
    icon: <BookOpen size={22} />,
    status: { label: 'PROJET ACTIF', color: 'active' },
    meta: 'Mis à jour il y a 2h',
  },
  {
    id: 2,
    title: 'Optimisation des modèles de langage',
    description: 'Étude comparative des techniques de quantification 4-bit.',
    icon: <Cpu size={22} />,
    status: { label: 'EN ATTENTE', color: 'pending' },
    meta: 'Mis à jour hier',
  },
  {
    id: 3,
    title: 'Sécurisation des APIs Locales',
    description: "Protocoles de sécurité pour l'inférence sur site.",
    icon: <Shield size={22} />,
    status: { label: 'À VENIR', color: 'upcoming' },
    meta: 'Début la semaine prochaine',
  },
];

const DashboardPage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="p-8 max-w-4xl animate-fadeIn">
      {/* Greeting */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-1">
          Bonjour, Alami
        </h1>
        <p className="text-gray-500 text-sm">
          Voici vos projets de recherche assignés.
        </p>
      </div>

      {/* Projects Section */}
      <div>
        <h2 className="text-lg font-bold text-gray-800 mb-5 flex items-center gap-2">
          <span>📋</span> Projets Assignés
        </h2>

        <div className="space-y-4">
          {projects.map((project) => {
            const style = statusStyles[project.status.color];
            const iconStyle = iconWrapperStyles[project.status.color];

            return (
              <div
                key={project.id}
                onClick={() => navigate(`/projet/${project.id}/cahier-des-charges`)}
                className="group bg-white rounded-2xl border border-gray-100 hover:border-gray-200 p-5 flex items-center gap-5 transition-all duration-300 hover:shadow-lg hover:shadow-gray-100/80 cursor-pointer"
              >
                {/* Icon */}
                <div className={`w-12 h-12 rounded-xl border flex-shrink-0 flex items-center justify-center transition-transform duration-300 group-hover:scale-110 ${iconStyle}`}>
                  {project.icon}
                </div>

                {/* Content */}
                <div className="flex-1 min-w-0">
                  <h3 className="text-[15px] font-semibold text-gray-900 mb-0.5 group-hover:text-blue-700 transition-colors">
                    {project.title}
                  </h3>
                  <p className="text-sm text-gray-400 truncate">
                    {project.description}
                  </p>
                </div>

                {/* Status & Meta */}
                <div className="flex-shrink-0 text-right flex flex-col items-end gap-1.5">
                  <span className={`inline-block text-[10px] font-bold px-3 py-1 rounded-full border tracking-wider ${style.bg} ${style.text}`}>
                    {project.status.label}
                  </span>
                  <span className="text-xs text-gray-400">
                    {project.meta}
                  </span>
                </div>

                {/* Arrow */}
                <ArrowRight size={18} className="text-gray-300 group-hover:text-blue-500 group-hover:translate-x-1 transition-all duration-300 flex-shrink-0" />
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
