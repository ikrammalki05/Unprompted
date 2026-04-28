const activities = [
  {
    id: 1,
    title: "Nouvelle inscription d'étudiant",
    desc: "Jean Dupont a rejoint la classe de Terminale S1",
    time: "IL Y A 10 MIN",
    icon: "👤+",
    iconBg: "bg-blue-50 text-[#0066cc]",
  },
  {
    id: 2,
    title: "Mise à jour de classe",
    desc: "Emploi du temps modifié pour le Groupe B - Informatique",
    time: "IL Y A 2 HEURES",
    icon: "🕒",
    iconBg: "bg-green-100 text-green-700",
  },
  {
    id: 3,
    title: "Nouvel enseignant affecté",
    desc: "Mme. Sophie Martin affectée au département de Mathématiques",
    time: "IL Y A 5 HEURES",
    icon: "👩‍🏫",
    iconBg: "bg-blue-50 text-[#0066cc]",
  },
];

export const RecentActivity = () => {
  return (
    <div className="w-full lg:w-2/3"> {/* Prend 2/3 de l'espace sur grand écran */}
      <div className="flex justify-between items-end mb-4">
        <h2 className="text-xl font-bold text-gray-900">Activité récente</h2>
        <a href="#" className="text-sm font-semibold text-[#0066cc] hover:underline">
          Voir tout
        </a>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-gray-50 overflow-hidden">
        {activities.map((activity, index) => (
          <div 
            key={activity.id} 
            // Ajoute une bordure en bas sauf pour le dernier élément
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
        ))}
      </div>
    </div>
  );
};