import { useEffect, useState } from 'react';
import { api } from '../../../../services/api';

type Activity = {
  id: string;
  title: string;
  desc: string;
  time: string;
  icon: string;
  iconBg: string;
};

export const RecentActivity = () => {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      try {
        const [etudiants, enseignants, projets] = await Promise.all([
          api.get('/Etudiant').catch(() => ({ data: [] })),
          api.get('/Enseignant').catch(() => ({ data: [] })),
          api.get('/projet').catch(() => ({ data: [] })),
        ]);

        const items: Activity[] = [];
        const etList = Array.isArray(etudiants.data) ? etudiants.data : [];
        const enList = Array.isArray(enseignants.data) ? enseignants.data : [];
        const prList = Array.isArray(projets.data) ? projets.data : [];

        etList.slice(0, 3).forEach((e: any, i: number) => {
          items.push({
            id: `et-${e.id ?? e.idEtudiant ?? i}`,
            title: 'Étudiant enregistré',
            desc: `${e.prenom ?? ''} ${e.nom ?? e.nomComplet ?? 'Étudiant'}`.trim(),
            time: 'BASE',
            icon: '👤',
            iconBg: 'bg-blue-50 text-[#0066cc]',
          });
        });

        enList.slice(0, 2).forEach((e: any, i: number) => {
          items.push({
            id: `en-${e.id ?? e.idEnseignant ?? i}`,
            title: 'Enseignant enregistré',
            desc: `${e.prenom ?? ''} ${e.nom ?? ''}`.trim() || 'Enseignant',
            time: 'BASE',
            icon: '👩‍🏫',
            iconBg: 'bg-green-100 text-green-700',
          });
        });

        prList.slice(0, 2).forEach((p: any, i: number) => {
          items.push({
            id: `pr-${p.idProjet ?? p.IdProjet ?? i}`,
            title: 'Projet actif',
            desc: p.titre ?? p.Titre ?? 'Projet',
            time: p.status ?? p.Status ?? '',
            icon: '📁',
            iconBg: 'bg-indigo-50 text-indigo-700',
          });
        });

        setActivities(items);
      } catch {
        setActivities([]);
      } finally {
        setLoading(false);
      }
    };
    void load();
  }, []);

  return (
    <div className="w-full lg:w-2/3">
      <div className="flex justify-between items-end mb-4">
        <h2 className="text-xl font-bold text-gray-900">Activité récente</h2>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-gray-50 overflow-hidden">
        {loading ? (
          <div className="p-5 text-sm text-gray-400">Chargement…</div>
        ) : activities.length === 0 ? (
          <div className="p-5 text-sm text-gray-400">Aucune activité en base pour le moment.</div>
        ) : (
          activities.map((activity, index) => (
            <div
              key={activity.id}
              className={`flex items-center gap-4 p-5 hover:bg-gray-50 transition-colors ${
                index !== activities.length - 1 ? 'border-b border-gray-100' : ''
              }`}
            >
              <div className={`w-10 h-10 rounded-full flex flex-none items-center justify-center text-sm ${activity.iconBg}`}>
                {activity.icon}
              </div>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-bold text-gray-900 truncate">{activity.title}</p>
                <p className="text-xs text-gray-500 truncate mt-0.5">{activity.desc}</p>
              </div>
              <div className="text-[10px] font-bold text-gray-400 tracking-wider whitespace-nowrap">
                {activity.time}
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};
