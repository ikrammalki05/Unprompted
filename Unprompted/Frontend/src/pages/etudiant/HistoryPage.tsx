import React from 'react';
import { ChevronDown, MoreHorizontal, GitBranch, History, CheckCircle2 } from 'lucide-react';

const HistoryPage: React.FC = () => {
  return (
    <div className="p-8 max-w-5xl mx-auto animate-fadeIn">
      {/* Title Section */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Historique unifié</h1>
        <p className="text-gray-500 text-sm">Session de l'étudiant Alami • Semestre d'Automne 2024</p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-12">
        <div className="bg-white p-6 rounded-2xl border border-gray-100 shadow-sm">
          <p className="text-xs font-bold text-gray-500 tracking-wider mb-2 uppercase">Total Requêtes IA</p>
          <p className="text-4xl font-extrabold text-gray-900">1,284</p>
        </div>
        <div className="bg-white p-6 rounded-2xl border border-gray-100 shadow-sm">
          <p className="text-xs font-bold text-gray-500 tracking-wider mb-2 uppercase">Commits Totaux</p>
          <div className="flex items-center gap-3">
            <p className="text-4xl font-extrabold text-gray-900">42</p>
            <span className="bg-blue-50 text-blue-600 text-[10px] font-bold px-2.5 py-1 rounded-full">
              Git Main
            </span>
          </div>
        </div>
        <div className="bg-white p-6 rounded-2xl border border-gray-100 shadow-sm">
          <p className="text-xs font-bold text-gray-500 tracking-wider mb-2 uppercase">Niveau de dépendance IA</p>
          <div className="flex items-center gap-4">
            <p className="text-4xl font-extrabold text-gray-900">64%</p>
            <div className="flex-1 bg-gray-100 h-2.5 rounded-full overflow-hidden">
              <div className="bg-blue-600 h-full rounded-full" style={{ width: '64%' }}></div>
            </div>
          </div>
        </div>
      </div>

      {/* Timeline Section */}
      <div className="mb-8">
        <h2 className="text-xs font-bold text-gray-500 tracking-widest uppercase mb-6">Chronologie des activités</h2>
        
        <div className="relative pl-8">
          {/* Vertical line */}
          <div className="absolute left-[11px] top-2 bottom-2 w-[2px] bg-gray-100"></div>

          {/* Item 1: IA Request */}
          <div className="relative mb-8">
            <div className="absolute -left-8 top-1.5 w-6 h-6 bg-white border-2 border-blue-500 rounded-full z-10"></div>
            <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden flex">
              <div className="w-1.5 bg-blue-500 flex-shrink-0"></div>
              <div className="p-5 flex-1">
                <div className="flex items-center justify-between mb-4">
                  <div className="flex items-center gap-3">
                    <div className="bg-blue-600 text-white text-[10px] font-bold px-2 py-1 rounded">IA</div>
                    <span className="text-xs font-medium text-gray-500">Il y a 14 minutes</span>
                  </div>
                  <button className="text-gray-400 hover:text-gray-600 transition-colors">
                    <MoreHorizontal size={16} />
                  </button>
                </div>
                <p className="text-gray-900 font-medium italic mb-4 text-[15px]">"Peux-tu optimiser cette boucle récursive pour réduire la complexité spatiale ?"</p>
                <div className="bg-gray-50 rounded-xl p-4 text-sm text-gray-600">
                  L'IA a suggéré une approche itérative utilisant la mémoïsation. Réduction théorique de O(n) à O(1) en espace pour les cas de base.
                </div>
              </div>
            </div>
          </div>

          {/* Item 2: Git Commit */}
          <div className="relative mb-8">
            <div className="absolute -left-8 top-1.5 w-6 h-6 bg-white border-2 border-gray-900 rounded-full z-10"></div>
            <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden flex">
              <div className="w-1.5 bg-gray-900 flex-shrink-0"></div>
              <div className="p-5 flex-1">
                <div className="flex items-center justify-between mb-3">
                  <div className="flex items-center gap-3">
                    <div className="bg-gray-900 text-white text-[10px] font-bold px-2 py-1 rounded">GIT</div>
                    <span className="text-xs font-medium text-gray-500">Il y a 45 minutes</span>
                  </div>
                  <div className="bg-blue-50 text-blue-600 text-[10px] font-bold px-2.5 py-1.5 rounded flex items-center gap-1.5">
                    <GitBranch size={12} /> feature/optimization-v2
                  </div>
                </div>
                <p className="text-gray-900 font-bold mb-3 text-[15px]">refactor: implementation of memoized Fibonacci sequence</p>
                <div className="flex items-center gap-4 text-xs">
                  <span className="text-gray-500 flex items-center gap-1"><History size={12} /> hash: 7e2a91b</span>
                  <span className="text-emerald-600 flex items-center gap-1"><CheckCircle2 size={12} /> Verified Build</span>
                </div>
              </div>
            </div>
          </div>

          {/* Item 3: IA Request */}
          <div className="relative mb-8">
            <div className="absolute -left-8 top-1.5 w-6 h-6 bg-white border-2 border-blue-500 rounded-full z-10"></div>
            <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden flex">
              <div className="w-1.5 bg-blue-500 flex-shrink-0"></div>
              <div className="p-5 flex-1">
                <div className="flex items-center gap-3 mb-4">
                  <div className="bg-blue-600 text-white text-[10px] font-bold px-2 py-1 rounded">IA</div>
                  <span className="text-xs font-medium text-gray-500">Il y a 2 heures</span>
                </div>
                <p className="text-gray-900 font-medium italic mb-4 text-[15px]">"Explique-moi les avantages de l'architecture Hexagonale par rapport au Layered."</p>
                <div className="bg-gray-50 rounded-xl p-4 text-sm text-gray-600">
                  Résumé : L'architecture hexagonale permet une meilleure testabilité en isolant le domaine métier des infrastructures via des ports et adaptateurs.
                </div>
              </div>
            </div>
          </div>

          {/* Item 4: Git Commit */}
          <div className="relative mb-8">
            <div className="absolute -left-8 top-1.5 w-6 h-6 bg-white border-2 border-gray-900 rounded-full z-10"></div>
            <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden flex">
              <div className="w-1.5 bg-gray-900 flex-shrink-0"></div>
              <div className="p-5 flex-1">
                <div className="flex items-center justify-between mb-3">
                  <div className="flex items-center gap-3">
                    <div className="bg-gray-900 text-white text-[10px] font-bold px-2 py-1 rounded">GIT</div>
                    <span className="text-xs font-medium text-gray-500">Hier, 18:30</span>
                  </div>
                  <div className="bg-blue-50 text-blue-600 text-[10px] font-bold px-2.5 py-1.5 rounded flex items-center gap-1.5">
                    <GitBranch size={12} /> main
                  </div>
                </div>
                <p className="text-gray-900 font-bold mb-3 text-[15px]">docs: update architectural ADR for project core</p>
                <div className="flex items-center gap-4 text-xs">
                  <span className="text-gray-500 flex items-center gap-1"><History size={12} /> hash: a3f1c4d</span>
                </div>
              </div>
            </div>
          </div>

        </div>
      </div>

      {/* Load More Button */}
      <div className="flex justify-center mt-2 pb-10">
        <button className="flex items-center gap-2 px-6 py-2.5 bg-white border border-gray-200 text-sm font-medium text-gray-700 rounded-full hover:bg-gray-50 transition-colors shadow-sm">
          <ChevronDown size={16} /> Charger les activités précédentes
        </button>
      </div>
    </div>
  );
};

export default HistoryPage;
