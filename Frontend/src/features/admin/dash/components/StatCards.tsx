import { useState, useEffect } from "react";
import { api } from "../../../../services/api";


interface StatItem {
  id: number;
  title: string;
  value: string | number;
  badge: string;
  badgeColor: string;
  icon: string;
  iconBg: string;
}


interface BackendStatsResponse {
  totalEtudiants: number;
  totalEnseignants: number;
  totalClasses: number;
}

export const StatCards = () => {
  const [statsData, setStatsData] = useState<StatItem[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        setIsLoading(true);
        
        const response = await api.get<BackendStatsResponse>('/Admin/dashboard-stats');
        const data = response.data;

        
        const formattedStats: StatItem[] = [
          {
            id: 1,
            title: "Total étudiants",
            value: data.totalEtudiants, // 👈 La donnée vient du backend
            badge: "+4.2%", // Tu pourras rendre ça dynamique plus tard si le backend l'envoie
            badgeColor: "bg-green-100 text-green-700",
            icon: "🎓", 
            iconBg: "bg-blue-50",
          },
          {
            id: 2,
            title: "Total enseignants",
            value: data.totalEnseignants, // 👈 La donnée vient du backend
            badge: "Stable",
            badgeColor: "bg-gray-200 text-gray-700",
            icon: "👩‍🏫",
            iconBg: "bg-gray-100",
          },
          {
            id: 3,
            title: "Total classes",
            value: data.totalClasses, // 👈 La donnée vient du backend
            badge: "+12",
            badgeColor: "bg-green-100 text-green-700",
            icon: "🚪",
            iconBg: "bg-blue-50",
          },
        ];

       
        setStatsData(formattedStats);
        
      } catch (error) {
        console.error("Erreur lors de la récupération des statistiques :", error);
        
        // En cas d'erreur (serveur éteint), on peut mettre des valeurs par défaut pour ne pas casser l'UI
        setStatsData([
            { id: 1, title: "Total étudiants", value: "---", badge: "Erreur", badgeColor: "bg-red-100 text-red-700", icon: "🎓", iconBg: "bg-blue-50" },
            { id: 2, title: "Total enseignants", value: "---", badge: "Erreur", badgeColor: "bg-red-100 text-red-700", icon: "👩‍🏫", iconBg: "bg-gray-100" },
            { id: 3, title: "Total classes", value: "---", badge: "Erreur", badgeColor: "bg-red-100 text-red-700", icon: "🚪", iconBg: "bg-blue-50" },
        ]);
      } finally {
        setIsLoading(false);
      }
    };

    fetchStats();
  }, []);

  if (isLoading) {
    return (
      <div className="w-full h-32 flex items-center justify-center bg-white rounded-2xl border border-gray-50 mb-8">
        <span className="text-gray-500 font-medium">Chargement des statistiques...</span>
      </div>
    );
  }

  // Le HTML (JSX) reste exactement le même !
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      {statsData.map((stat) => (
        <div key={stat.id} className="bg-white rounded-2xl p-6 shadow-sm border border-gray-50">
          <div className="flex justify-between items-start mb-6">
            <div className={`w-12 h-12 rounded-xl flex items-center justify-center text-xl ${stat.iconBg}`}>
              {stat.icon}
            </div>
            <span className={`px-3 py-1 text-xs font-bold rounded-full ${stat.badgeColor}`}>
              {stat.badge}
            </span>
          </div>
          <div>
            <p className="text-sm font-semibold text-gray-500 mb-1">{stat.title}</p>
            <p className="text-3xl font-bold text-gray-900">{stat.value}</p>
          </div>
        </div>
      ))}
    </div>
  );
};