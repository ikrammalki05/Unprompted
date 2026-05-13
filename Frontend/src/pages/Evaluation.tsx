import React, { useState, useEffect } from 'react';
import {
  MessageSquare,
  Send,
  ChevronDown,
  User,
  Activity
} from 'lucide-react';

import ControlledSlider from '../components/ControlledSlider';
import evaluationService from '../services/evaluationService';
import userService from '../services/userService';

const Evaluation: React.FC = () => {
  const [technicalPerformance, setTechnicalPerformance] = useState(8.0);
  const [commentaire, setCommentaire] = useState('');
  const [etudiants, setEtudiants] = useState<any[]>([]);
  const [selectedEtudiantId, setSelectedEtudiantId] = useState<number | string>('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const data = await userService.getStudents();
        setEtudiants(data);
        if (data.length > 0) {
          setSelectedEtudiantId(data[0].id || data[0].Id);
        }
      } catch (error) {
        console.error("Erreur chargement étudiants", error);
      } finally {
        setLoading(false);
      }
    };
    fetchStudents();
  }, []);

  const selectedEtudiant = etudiants.find(e => (e.id || e.Id) === Number(selectedEtudiantId));

  const finalGrade = technicalPerformance * 2;

  const handleSubmitEvaluation = async () => {
    if (!selectedEtudiantId) {
      alert('Veuillez sélectionner un étudiant');
      return;
    }

    try {
      const evaluation = {
        etudiantId: Number(selectedEtudiantId),
        projetId: 1, // À dynamiser plus tard selon l'affectation réelle
        performanceTechnique: technicalPerformance,
        noteFinale: finalGrade,
        commentaire
      };

      await evaluationService.create(evaluation);
      alert('Évaluation envoyée avec succès');
      setCommentaire('');
    } catch (error) {
      console.error(error);
      alert('Erreur lors de l’envoi');
    }
  };

  return (
    <div className="p-8 space-y-8 max-w-7xl mx-auto animate-in fade-in duration-500">
      
      {/* Header Info */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Selection Box */}
        <div className="bg-white p-8 rounded-[32px] shadow-sm border border-slate-50 flex flex-col justify-between min-h-[160px]">
          <h3 className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-4 flex items-center gap-2">
            <User className="w-3 h-3" /> SÉLECTION DE L'ÉTUDIANT
          </h3>

          <div className="relative group">
            <select 
              value={selectedEtudiantId}
              onChange={(e) => setSelectedEtudiantId(e.target.value)}
              className="w-full appearance-none px-6 py-5 bg-slate-50 rounded-2xl text-slate-900 font-bold hover:bg-slate-100 transition-all border-none outline-none cursor-pointer pr-12 shadow-inner"
            >
              {loading ? (
                <option>Chargement...</option>
              ) : (
                etudiants.map(e => (
                  <option key={e.id || e.Id} value={e.id || e.Id}>
                    {e.nomComplet || e.NomComplet}
                  </option>
                ))
              )}
            </select>
            <ChevronDown className="absolute right-5 top-1/2 -translate-y-1/2 w-5 h-5 text-slate-400 pointer-events-none transition-transform group-hover:translate-y-[-40%]" />
          </div>
        </div>

        {/* Profile Details Box */}
        <div className="bg-white p-8 rounded-[32px] shadow-sm border border-slate-50 h-full relative overflow-hidden">
          <div className="absolute top-0 right-0 p-4 opacity-5">
            <Activity className="w-24 h-24" />
          </div>
          
          <h3 className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-6">
            DÉTAILS DU PROFIL
          </h3>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <p className="text-[10px] font-bold text-slate-400 uppercase mb-1">
                NOM COMPLET
              </p>
              <p className="text-xl font-black text-slate-900 leading-tight">
                {selectedEtudiant ? (selectedEtudiant.nomComplet || selectedEtudiant.NomComplet) : "---"}
              </p>
            </div>

            <div>
              <p className="text-[10px] font-bold text-slate-400 uppercase mb-1">
                GROUPE / FILIÈRE
              </p>
              <p className="text-xl font-black text-blue-600 leading-tight">
                {selectedEtudiant?.classeNom || "Non assigné"}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Form Box */}
      <div className="bg-white p-10 rounded-[32px] shadow-sm border border-slate-50 shadow-2xl shadow-slate-100/50">
        <div className="flex items-center gap-4 mb-10">
          <div className="p-4 bg-blue-50 text-blue-600 rounded-2xl">
            <MessageSquare className="w-6 h-6" />
          </div>

          <div>
            <h2 className="text-3xl font-black text-slate-900 tracking-tight">
              Formulaire d'Évaluation
            </h2>
            <p className="text-slate-400 text-sm font-medium">Notez les compétences et la progression de l'étudiant.</p>
          </div>
        </div>

        <div className="space-y-12">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-x-16 gap-y-10">
            <ControlledSlider
              label="Performance Technique (0-10)"
              value={technicalPerformance}
              onChange={setTechnicalPerformance}
              description="Évaluez la qualité du code et de l'implémentation."
            />

            <div className="space-y-4">
              <label className="block text-sm font-black text-slate-800">
                Note Finale (/20)
              </label>

              <div className="flex items-center gap-4 px-8 py-6 bg-slate-50 rounded-[28px] w-fit min-w-[180px] border border-slate-100 shadow-sm">
                <span className="text-4xl font-black text-blue-600">
                  {Math.round(finalGrade)}
                </span>

                <span className="text-slate-400 font-black text-2xl">
                  / 20
                </span>
              </div>
            </div>
          </div>

          <div className="space-y-4">
            <label className="block text-sm font-black text-slate-800">
              Commentaires de l'enseignant
            </label>

            <textarea
              placeholder="Ex: Excellent travail sur la partie API. Attention à la documentation..."
              className="w-full h-40 p-8 bg-slate-50/50 rounded-[32px] text-slate-800 font-medium placeholder:text-slate-300 focus:outline-none focus:ring-4 focus:ring-blue-50 focus:bg-white transition-all border border-slate-50"
              value={commentaire}
              onChange={(e) => setCommentaire(e.target.value)}
            ></textarea>
          </div>

          <div className="flex justify-end pt-4">
            <button
              onClick={handleSubmitEvaluation}
              className="flex items-center gap-4 px-12 py-6 bg-slate-900 text-white rounded-[28px] font-black text-lg hover:bg-slate-800 active:scale-95 transition-all shadow-2xl shadow-slate-900/20 group"
            >
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