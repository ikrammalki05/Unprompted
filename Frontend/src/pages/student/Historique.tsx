import  { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Bell, Settings,  ArrowLeft,  BrainCircuit} from 'lucide-react';
import { api } from '../../services/api'; // Ajuste le chemin selon ton projet

// ─── TYPES ───────────────────────────────────────────────────────────────────
type ActivityType = 'ai' | 'git' | 'eval';

interface UnifiedActivity {
  id: string;
  type: ActivityType;
  dateStr: string;     // Chaîne brute pour l'affichage (ex: "le 4 mai à 10:30")
  timestamp: number;   // Timestamp pour le tri chronologique
  title: string;
  description?: string;
  project?: string;
  meta?: any;          // Lignes ajoutées, tokens, note, etc.
}

interface DashboardStats {
  totalIa: number;
  totalGit: number;
  iaDependance: number;
}

// Utilitaire pour formater la date proprement
const formatDate = (isoString: string) => {
  const date = new Date(isoString);
  return date.toLocaleDateString('fr-FR', {
    day: 'numeric',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

export const UnifiedHistoryPage = () => {
  const navigate = useNavigate();
  
  const [activities, setActivities] = useState<UnifiedActivity[]>([]);
  const [stats, setStats] = useState<DashboardStats>({ totalIa: 0, totalGit: 0, iaDependance: 0 });
  const [isLoading, setIsLoading] = useState(true);

  // ID de l'étudiant (à remplacer par l'ID réel via le contexte ou l'auth)
  const studentId = 1; 

  useEffect(() => {
    const fetchHistory = async () => {
      try {
        setIsLoading(true);
        // Appel à ton API C#
        const response = await api.get(`/Profil/etudiant/${studentId}/historique`);
        const data = response.data;

        const combinedActivities: UnifiedActivity[] = [];

        // 1. Mapper les contributions Git
        if (data.contributionsGit) {
          data.contributionsGit.forEach((git: any, index: number) => {
            combinedActivities.push({
              id: `git-${index}`,
              type: 'git',
              timestamp: new Date(git.dateCommit).getTime(),
              dateStr: formatDate(git.dateCommit),
              title: git.messageCommit,
              project: git.nomProjet,
              meta: { added: git.lignesAjoutees, removed: git.lignesSupprimees }
            });
          });
        }

        // 2. Mapper les interactions IA
        if (data.interactionsIa) {
          data.interactionsIa.forEach((ia: any, index: number) => {
            combinedActivities.push({
              id: `ia-${index}`,
              type: 'ai',
              timestamp: new Date(ia.dateQuestion).getTime(),
              dateStr: formatDate(ia.dateQuestion),
              title: `"${ia.question}"`,
              project: ia.nomProjet,
              meta: { tokens: ia.tokensConsommes }
            });
          });
        }

        // 3. Mapper les évaluations
        if (data.evaluations) {
          data.evaluations.forEach((evalItem: any, index: number) => {
            combinedActivities.push({
              id: `eval-${index}`,
              type: 'eval',
              timestamp: new Date(evalItem.dateEvaluation).getTime(),
              dateStr: formatDate(evalItem.dateEvaluation),
              title: `Évaluation de projet : ${evalItem.note}/20`,
              description: evalItem.commentaire,
              project: evalItem.nomProjet
            });
          });
        }

        // 4. Trier tout du plus récent au plus ancien
        combinedActivities.sort((a, b) => b.timestamp - a.timestamp);
        setActivities(combinedActivities);

        // 5. Calculer les statistiques pour les cartes du haut
        const totalIa = data.interactionsIa?.length || 0;
        const totalGit = data.contributionsGit?.length || 0;
        
        // Calcul basique du % de dépendance (Ratio IA / (IA + Git))
        const totalInteractions = totalIa + totalGit;
        const dependance = totalInteractions > 0 ? Math.round((totalIa / totalInteractions) * 100) : 0;

        setStats({
          totalIa: totalIa,
          totalGit: totalGit,
          iaDependance: dependance
        });

      } catch (error) {
        console.error("Erreur lors de la récupération de l'historique:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchHistory();
  }, [studentId]);

  // Fonction pour définir les couleurs selon le type d'événement
  const getEventStyle = (type: ActivityType) => {
    switch (type) {
      case 'ai': return { border: 'border-l-blue-500', bg: 'bg-blue-500', text: 'IA' };
      case 'eval': return { border: 'border-l-amber-500', bg: 'bg-amber-500', text: 'EVAL' };
      case 'git': default: return { border: 'border-l-gray-900', bg: 'bg-gray-900', text: 'GIT' };
    }
  };

  return (
    <div className="min-h-screen bg-[#fafafa] font-sans text-gray-900 pb-12">
      
      {/* Navbar */}
      <header className="flex items-center justify-between px-8 py-4 bg-white border-b border-gray-100">
        <div className="flex items-center gap-4">
          <button 
            onClick={() => navigate('/admin/code')}
            className="p-2 -ml-2 text-gray-400 hover:text-gray-900 hover:bg-gray-50 rounded-lg transition-colors"
          >
            <ArrowLeft size={20} />
          </button>
          <div className="text-xl font-bold tracking-tight">Unprompted</div>
        </div>
        <div className="flex items-center gap-4 text-gray-500">
          <button className="hover:text-gray-900"><Bell size={20} /></button>
          <button className="hover:text-gray-900"><Settings size={20} /></button>
        </div>
      </header>

      <main className="max-w-5xl mx-auto px-8 mt-10">
        
        <div className="mb-10">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">Historique unifié</h1>
          <p className="text-gray-500 font-medium">Session de l'étudiant • Vue détaillée</p>
        </div>

        {/* Cartes de statistiques dynamiques */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-12">
          <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-100">
            <h3 className="text-xs font-bold text-gray-400 uppercase tracking-wider mb-3">Total Requêtes IA</h3>
            <p className="text-4xl font-extrabold text-[#0a192f]">{stats.totalIa}</p>
          </div>

          <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-100">
            <h3 className="text-xs font-bold text-gray-400 uppercase tracking-wider mb-3">Commits Totaux</h3>
            <p className="text-4xl font-extrabold text-[#0a192f]">{stats.totalGit}</p>
          </div>

          <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-100">
            <h3 className="text-xs font-bold text-gray-400 uppercase tracking-wider mb-3">Dépendance IA (Est.)</h3>
            <div className="flex items-center gap-4">
              <p className="text-4xl font-extrabold text-[#0a192f]">{stats.iaDependance}%</p>
              <div className="flex-1 h-2.5 bg-gray-100 rounded-full overflow-hidden">
                <div className="h-full bg-blue-600 rounded-full transition-all duration-1000" style={{ width: `${stats.iaDependance}%` }}></div>
              </div>
            </div>
          </div>
        </div>

        {/* Chronologie (Timeline) */}
        <div>
          <h2 className="text-xs font-bold text-gray-400 uppercase tracking-wider mb-8">Chronologie des activités</h2>
          
          {isLoading ? (
            <div className="flex justify-center py-10">
              <span className="text-gray-500 font-medium">Chargement de l'historique...</span>
            </div>
          ) : activities.length === 0 ? (
            <div className="text-center py-10 bg-white rounded-xl border border-gray-100">
              <span className="text-gray-500">Aucune activité trouvée pour cet étudiant.</span>
            </div>
          ) : (
            <div className="relative">
              <div className="absolute left-[11px] top-4 bottom-0 w-[2px] bg-gray-100"></div>

              <div className="flex flex-col gap-8">
                {activities.map((activity) => {
                  const style = getEventStyle(activity.type);

                  return (
                    <div key={activity.id} className="relative pl-12">
                      <div className={`absolute left-0 top-6 w-[24px] h-[24px] rounded-full border-2 bg-white flex items-center justify-center z-10 border-${style.bg.split('-')[1]}-500`}></div>

                      <div className={`bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden flex flex-col border-l-4 ${style.border}`}>
                        <div className="p-5">
                          
                          {/* En-tête */}
                          <div className="flex items-center justify-between mb-4">
                            <div className="flex items-center gap-3">
                              <div className={`w-10 h-6 rounded flex items-center justify-center text-[10px] font-bold text-white ${style.bg}`}>
                                {style.text}
                              </div>
                              <span className="text-xs text-gray-500 font-medium">{activity.dateStr}</span>
                            </div>
                            
                            {/* Badge du nom du projet */}
                            {activity.project && (
                              <div className="flex items-center gap-1.5 px-2.5 py-1 bg-gray-50 text-gray-600 border border-gray-200 text-[11px] font-semibold rounded-md">
                                {activity.project}
                              </div>
                            )}
                          </div>

                          {/* Titre */}
                          <h4 className={`text-[15px] text-gray-900 ${activity.type === 'ai' ? 'italic font-bold' : 'font-bold'}`}>
                            {activity.title}
                          </h4>

                          {/* Description ou Commentaire */}
                          {activity.description && (
                            <div className="mt-4 p-4 bg-gray-50 rounded-lg text-sm text-gray-600 leading-relaxed border border-gray-100">
                              {activity.description}
                            </div>
                          )}

                          {/* Méta-données spécifiques (Lignes ajoutées, Tokens) */}
                          {activity.type === 'git' && activity.meta && (
                            <div className="mt-3 flex items-center gap-4 text-xs font-medium">
                              <span className="text-green-600 flex items-center gap-1">
                                +{activity.meta.added} lignes
                              </span>
                              <span className="text-red-500 flex items-center gap-1">
                                -{activity.meta.removed} lignes
                              </span>
                            </div>
                          )}

                          {activity.type === 'ai' && activity.meta && (
                            <div className="mt-3 flex items-center gap-4 text-xs font-medium text-gray-400">
                              <BrainCircuit size={14} />
                              {activity.meta.tokens} tokens consommés
                            </div>
                          )}

                        </div>
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          )}
        </div>

      </main>
    </div>
  );
};