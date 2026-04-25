import React, { useState } from 'react';
import { 
  FileUp, 
  Infinity, 
  Hourglass, 
  ShieldAlert, 
  Filter, 
  Download, 
  CheckCircle2 
} from 'lucide-react';

interface AccessCardProps {
  id: string;
  icon: React.ElementType;
  title: string;
  description: string;
  active: boolean;
  onClick: () => void;
}

const AccessCard: React.FC<AccessCardProps> = ({ icon: Icon, title, description, active, onClick }) => (
  <div 
    onClick={onClick}
    className={`p-6 rounded-2xl border-2 cursor-pointer transition-all duration-300 relative group flex flex-col h-full bg-white ${
      active 
        ? 'border-blue-500 shadow-lg shadow-blue-50' 
        : 'border-slate-100 hover:border-slate-200 hover:shadow-md'
    }`}
  >
    {active && (
      <div className="absolute top-4 right-4">
        <CheckCircle2 className="w-6 h-6 text-blue-500" fill="currentColor" fillOpacity={0.1} />
      </div>
    )}
    <div className={`w-12 h-12 rounded-xl flex items-center justify-center mb-6 ${
      active ? 'bg-blue-100 text-blue-600' : 'bg-slate-50 text-slate-400 group-hover:bg-slate-100'
    }`}>
      <Icon className="w-6 h-6" />
    </div>
    <h3 className="text-xl font-bold text-slate-800 mb-3">{title}</h3>
    <p className="text-sm text-slate-500 leading-relaxed line-clamp-3">
      {description}
    </p>
  </div>
);

const ConfigurationIA: React.FC = () => {
  const [selectedAccess, setSelectedAccess] = useState('unlimited');

  return (
    <div className="p-8 space-y-12 max-w-7xl mx-auto">
      {/* Project Selection Section */}
      <section>
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-slate-800 tracking-tight">Sélectionner le projet</h2>
          <p className="text-slate-500 mt-1">Choisissez le projet académique pour lequel vous souhaitez configurer les accès IA.</p>
        </div>
        
        <div className="bg-slate-50/50 border-2 border-dashed border-slate-200 rounded-3xl p-12 text-center transition-colors hover:border-blue-300 group">
          <div className="w-16 h-16 bg-blue-50 text-blue-600 rounded-2xl flex items-center justify-center mx-auto mb-4 group-hover:scale-110 transition-transform">
            <FileUp className="w-8 h-8" />
          </div>
          <h3 className="text-lg font-bold text-slate-800 mb-4 tracking-tight">Importer le cahier des charges (PDF)</h3>
          <button className="px-8 py-2.5 bg-slate-200 text-slate-700 rounded-xl font-bold hover:bg-slate-300 transition-colors shadow-sm">
            Choisir un fichier
          </button>
        </div>
      </section>

      {/* Access Levels Section */}
      <section>
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-slate-800 tracking-tight">Niveaux d'accès IA</h2>
          <p className="text-slate-500 mt-1">Définissez les privilèges d'utilisation de l'intelligence artificielle pour cette session académique.</p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <AccessCard 
            id="unlimited"
            icon={Infinity}
            title="Accès illimité"
            description="Utilisation libre pour activités exploratoires. Recommandé pour les phases de recherche."
            active={selectedAccess === 'unlimited'}
            onClick={() => setSelectedAccess('unlimited')}
          />
          <AccessCard 
            id="limited"
            icon={Hourglass}
            title="Accès limité"
            description="Quota de tokens par période. Idéal pour l'apprentissage de l'efficience des prompts."
            active={selectedAccess === 'limited'}
            onClick={() => setSelectedAccess('limited')}
          />
          <AccessCard 
            id="restricted"
            icon={ShieldAlert}
            title="Accès restreint"
            description="Fonctionnalités filtrées. Utilisé pour les évaluations sous haute surveillance."
            active={selectedAccess === 'restricted'}
            onClick={() => setSelectedAccess('restricted')}
          />
        </div>
      </section>

      {/* Pedagogical Analysis Section */}
      <section>
        <div className="flex flex-col md:flex-row md:items-end justify-between gap-4 mb-8">
          <div>
            <h2 className="text-2xl font-bold text-slate-800 tracking-tight">Analyse pédagogique</h2>
            <p className="text-slate-500 mt-1">Surveillance en temps réel de l'interaction étudiant-IA.</p>
          </div>
          
          <div className="flex gap-3">
            <button className="flex items-center gap-2 px-5 py-2.5 bg-slate-100 text-slate-600 rounded-xl font-bold hover:bg-slate-200 transition-all border border-slate-200/50">
              <Filter className="w-4 h-4" />
              <span>Filtrer</span>
            </button>
            <button className="flex items-center gap-2 px-5 py-2.5 bg-slate-100 text-slate-600 rounded-xl font-bold hover:bg-slate-200 transition-all border border-slate-200/50">
              <Download className="w-4 h-4" />
              <span>Exporter CSV</span>
            </button>
          </div>
        </div>

        <div className="bg-white rounded-3xl border border-slate-100 overflow-hidden shadow-sm">
          <div className="overflow-x-auto">
            <table className="w-full text-left">
              <thead>
                <tr className="bg-slate-50/50 border-b border-slate-100">
                  <th className="px-6 py-4 text-[10px] font-black text-slate-400 uppercase tracking-widest">Étudiant</th>
                  <th className="px-6 py-4 text-[10px] font-black text-slate-400 uppercase tracking-widest">Projet</th>
                  <th className="px-6 py-4 text-[10px] font-black text-slate-400 uppercase tracking-widest">Commits</th>
                  <th className="px-6 py-4 text-[10px] font-black text-slate-400 uppercase tracking-widest">Git</th>
                  <th className="px-6 py-4 text-[10px] font-black text-slate-400 uppercase tracking-widest">Derniers Prompts</th>
                  <th className="px-6 py-4 text-[10px] font-black text-slate-400 uppercase tracking-widest">Tokens</th>
                  <th className="px-6 py-4 text-[10px] font-black text-slate-400 uppercase tracking-widest">Dépendance</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                <tr className="hover:bg-slate-50/50 transition-colors">
                  <td className="px-6 py-5 col-span-7 text-center text-slate-400 text-sm font-medium italic">
                    Aucune donnée disponible pour le moment
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </section>
    </div>
  );
};

export default ConfigurationIA;
