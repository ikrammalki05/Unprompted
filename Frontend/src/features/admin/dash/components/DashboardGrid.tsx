export const DashboardGrid = () => {
  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
      
      {/* LIGNE 1 */}
      <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-50 lg:col-span-1">
        <p className="text-gray-400 text-sm">Widget : Projet Assigné</p>
      </div>
      <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-50 lg:col-span-1">
        <p className="text-gray-400 text-sm">Widget : Progression Globale</p>
      </div>
      <div className="bg-[#021124] rounded-2xl p-6 shadow-sm lg:col-span-1 text-white">
        <p className="text-gray-400 text-sm">Widget : Échéance Proche</p>
      </div>

      {/* COLONNE GAUCHE (2/3) */}
      <div className="lg:col-span-2 space-y-6">
        <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-50 min-h-[300px]">
           <p className="text-gray-400 text-sm">Widget : Activité Git & Commits</p>
        </div>
        <div className="bg-gradient-to-br from-[#021124] to-[#1a5b9c] rounded-2xl p-6 shadow-sm min-h-[150px] text-white">
           <p className="text-blue-200 text-sm">Widget : Assistant IA Unprompted</p>
        </div>
      </div>

      {/* COLONNE DROITE (1/3) */}
      <div className="lg:col-span-1 space-y-6">
        <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-50 min-h-[250px]">
           <p className="text-gray-400 text-sm">Widget : Jalons et Tâches</p>
        </div>
        <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-50 min-h-[200px]">
           <p className="text-gray-400 text-sm">Widget : Membres de l'équipe</p>
        </div>
        <div className="bg-[#f8f9fa] rounded-2xl p-6 border border-gray-100 min-h-[100px]">
           <p className="text-gray-400 text-sm">Widget : Statistiques IA</p>
        </div>
      </div>

    </div>
  );
};