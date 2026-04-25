import React, { useState } from 'react';
import { 
  MessageSquare, 
  Send, 
  CheckCircle2, 
  ChevronDown 
} from 'lucide-react';
import ControlledSlider from '../components/ControlledSlider';

const Evaluation: React.FC = () => {
  const [technicalPerformance, setTechnicalPerformance] = useState(8.0);
  
  // Final grade is calculated automatically in real-time (Value * 2)
  const finalGrade = technicalPerformance * 2;

  return (
    <div className="p-8 space-y-8 max-w-7xl mx-auto animate-in fade-in duration-500">
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Student Selection Card */}
        <div className="bg-white p-8 rounded-[32px] shadow-sm border border-slate-50 flex flex-col justify-between h-full min-h-[160px]">
          <h3 className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-4">SÉLECTION DE L'ÉTUDIANT</h3>
          <div className="relative group">
            <button className="w-full flex items-center justify-between px-6 py-4 bg-slate-50 rounded-2xl text-slate-900 font-bold hover:bg-slate-100 transition-colors">
              <span>Alice Lambert</span>
              <ChevronDown className="w-5 h-5 text-slate-400" />
            </button>
          </div>
        </div>

        {/* Profile Details Card */}
        <div className="bg-white p-8 rounded-[32px] shadow-sm border border-slate-50 h-full">
          <h3 className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-6">DÉTAILS DU PROFIL</h3>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <p className="text-[10px] font-bold text-slate-400 uppercase mb-1">NOM COMPLET</p>
              <p className="text-lg font-black text-slate-900 leading-tight">Alice Lambert</p>
            </div>
            <div>
              <p className="text-[10px] font-bold text-slate-400 uppercase mb-1">PROJET</p>
              <p className="text-lg font-black text-slate-900 leading-tight">Optimisation de LLM local</p>
            </div>
            <div className="mt-4">
              <p className="text-[10px] font-bold text-slate-400 uppercase mb-1">RÔLE</p>
              <span className="inline-flex items-center px-3 py-1 bg-blue-50 text-blue-600 text-[10px] font-black rounded-full uppercase tracking-tight">
                Responsable Backend
              </span>
            </div>
          </div>
        </div>
      </div>

      {/* AI Resources Card */}
      <div className="bg-white p-8 rounded-[32px] shadow-sm border border-slate-50">
        <div className="flex justify-between items-center mb-6">
          <h3 className="text-[10px] font-black text-slate-400 uppercase tracking-widest">UTILISATION DES RESSOURCES IA</h3>
          <span className="flex items-center gap-2 px-3 py-1.5 bg-green-50 text-green-600 text-[11px] font-bold rounded-lg border border-green-100">
            <CheckCircle2 className="w-4 h-4" />
            Quota respecté
          </span>
        </div>
        
        <div className="flex items-baseline gap-2 mb-4">
          <span className="text-4xl font-black text-slate-900">28</span>
          <span className="text-slate-400 font-bold">/ 50 prompts utilisés</span>
        </div>

        <div className="w-full h-3 bg-slate-100 rounded-full overflow-hidden">
          <div 
            className="h-full bg-blue-500 rounded-full transition-all duration-1000 ease-out"
            style={{ width: '56%' }}
          ></div>
        </div>
      </div>

      {/* Evaluation Form Card */}
      <div className="bg-white p-10 rounded-[32px] shadow-sm border border-slate-50">
        <div className="flex items-center gap-4 mb-10">
          <div className="p-3 bg-blue-50 text-blue-600 rounded-xl">
            <MessageSquare className="w-6 h-6" />
          </div>
          <h2 className="text-3xl font-black text-slate-900 tracking-tight">Formulaire d'Évaluation</h2>
        </div>

        <div className="space-y-12">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-x-16 gap-y-10">
            {/* Slider Section */}
            <ControlledSlider 
              label="Performance Technique (0-10)"
              value={technicalPerformance}
              onChange={setTechnicalPerformance}
              description="Évaluez la qualité du code et l'implémentation de l'architecture."
            />

            {/* Note Finale Section */}
            <div>
              <label className="block text-sm font-black text-slate-800 mb-6">Note Finale (/20)</label>
              <div className="flex items-center gap-4 px-6 py-4 bg-slate-50 rounded-2xl w-fit min-w-[140px] border border-slate-100 shadow-sm transition-all duration-300">
                <span className="text-2xl font-black text-blue-600 animate-in fade-in zoom-in duration-300">
                  {Math.round(finalGrade)}
                </span>
                <span className="text-slate-400 font-black text-xl">/ 20</span>
              </div>
            </div>
          </div>

          {/* Comments Section */}
          <div>
            <label className="block text-sm font-black text-slate-800 mb-4">Commentaires de l'enseignant</label>
            <textarea 
              placeholder="Saisissez vos observations sur le travail de l'étudiant et son implication dans le projet..."
              className="w-full h-40 p-6 bg-slate-50 rounded-2xl text-slate-800 font-medium placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-blue-100 transition-all border-none"
            ></textarea>
          </div>

          {/* Submit Button */}
          <div className="flex justify-end pt-4">
            <button className="flex items-center gap-3 px-10 py-5 bg-black text-white rounded-[24px] font-black text-lg hover:scale-105 active:scale-95 transition-all shadow-xl shadow-black/10 group">
              <span>Soumettre l'évaluation</span>
              <Send className="w-5 h-5 group-hover:translate-x-1 group-hover:-translate-y-1 transition-transform" />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Evaluation;
