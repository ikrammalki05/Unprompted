export const DashboardHeader = () => {
  return (
    <div className="flex items-end justify-between mb-8">
      <div>
        <h1 className="text-3xl font-bold text-gray-900 flex items-center gap-2">
          Bonjour, Alami <span className="text-2xl">👋</span>
        </h1>
        <p className="text-sm text-gray-500 mt-1">
          Voici l'état d'avancement de vos recherches académiques.
        </p>
      </div>
      <div className="flex gap-3">
        <button className="px-4 py-2 bg-gray-100 text-gray-700 text-sm font-semibold rounded-lg hover:bg-gray-200 transition-colors flex items-center gap-2">
          <span>📥</span> Exporter le Rapport
        </button>
        <button className="px-4 py-2 bg-[#021124] text-white text-sm font-semibold rounded-lg hover:bg-gray-800 transition-colors flex items-center gap-2">
          <span>+</span> Nouveau Projet
        </button>
      </div>
    </div>
  );
};