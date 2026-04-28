import React, { useState, useEffect } from 'react';
import type { Teacher, ClassData } from '../types';
import { api } from '../../../../services/api';

interface Props {
  teachers: Teacher[];
  classes: ClassData[];
  setTeachers: React.Dispatch<React.SetStateAction<Teacher[]>>;
  setClasses: React.Dispatch<React.SetStateAction<ClassData[]>>;
}

export function AffectationPanel({ teachers, classes, setTeachers, setClasses }: Props) {
  const [selectedTeacher, setSelectedTeacher] = useState('');
  const [selectedClass, setSelectedClass] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (teachers.length > 0 && !selectedTeacher) {
      setSelectedTeacher(teachers[0].nom || teachers[0].nomComplet || '');
    }
  }, [teachers, selectedTeacher]);

  useEffect(() => {
    if (classes.length > 0 && !selectedClass) {
      setSelectedClass(classes[0].nomClasse || '');
    }
  }, [classes, selectedClass]);

  const handleAssignClick = async () => {
    if (!selectedTeacher || !selectedClass) return;

    // 1. On cherche les vrais objets pour récupérer leurs IDs
    const teacher = teachers.find(t => t.nom === selectedTeacher || t.nomComplet === selectedTeacher);
    const classe = classes.find(c => c.nomClasse === selectedClass);

    if (!teacher || !classe) {
      alert("Erreur : Enseignant ou classe introuvable.");
      return;
    }

    setIsSubmitting(true);
    
    try {
      // 2. Appel API avec la bonne route et le bon body
      await api.post('/Admin/affecter-enseignant', {
        idEnseignant: teacher.id,
        idClasse: classe.id
      });

      // 3. Mise à jour de l'affichage dans React pour que l'utilisateur voie le changement
      const updatedClassesPourEnseignant = Array.from(new Set([...(teacher.classes || []), selectedClass]));
      
      setClasses(prev => prev.map(c => 
        c.id === classe.id ? { ...c, enseignantReferent: selectedTeacher } : c
      ));

      setTeachers(prev => prev.map(t => 
        t.id === teacher.id ? { ...t, classes: updatedClassesPourEnseignant } : t
      ));
      
      alert(`${selectedTeacher} a été assigné à la classe ${selectedClass} avec succès !`);
      
    } catch (error) {
      console.error("Erreur lors de l'affectation sur le serveur :", error);
      alert("Une erreur est survenue lors de l'affectation côté serveur.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="bg-white border border-[#f1f5f9] rounded-2xl p-7 shadow-[0_4px_12px_rgba(0,0,0,0.02)] flex flex-col gap-6">
      <h2 className="flex items-center gap-3 text-[15px] font-bold text-[#1e293b]">
        <span className="w-1 h-5 bg-[#2563eb] rounded-full shrink-0" />
        Affectation
      </h2>
      <div className="flex flex-col gap-1.5">
        <h4 className="text-[14px] font-bold text-[#1e293b]">Assigner un enseignant</h4>
        <p className="text-[12px] text-[#94a3b8] leading-relaxed">Liez un membre du corps professoral à une classe spécifique pour l'année académique.</p>
      </div>

      <div className="flex flex-col gap-4">
        <div>
          <label className="block text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider mb-2">Sélectionner l'enseignant</label>
          <select 
            className="w-full p-2.5 bg-[#f8fafc] border border-[#f1f5f9] rounded-lg text-sm text-[#1e293b] font-medium outline-none appearance-none cursor-pointer disabled:opacity-50" 
            value={selectedTeacher} 
            onChange={e => setSelectedTeacher(e.target.value)}
            disabled={isSubmitting}
          >
            {teachers.map(t => (
              <option key={t.id} value={t.nom || t.nomComplet}>
                {t.nomComplet || t.nom}
              </option>
            ))}
          </select>
        </div>
        <div>
          <label className="block text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider mb-2">Sélectionner la classe</label>
          <select 
            className="w-full p-2.5 bg-[#f8fafc] border border-[#f1f5f9] rounded-lg text-sm text-[#1e293b] font-medium outline-none appearance-none cursor-pointer disabled:opacity-50"
            value={selectedClass}
            onChange={e => setSelectedClass(e.target.value)}
            disabled={isSubmitting}
          >
            {classes.map(c => (
              <option key={c.id} value={c.nomClasse}>{c.nomClasse}</option>
            ))}
          </select>
        </div>
        <button 
          className="w-full bg-[#2563eb] text-white p-3 rounded-lg text-[13.5px] font-bold shadow-[0_4px_12px_rgba(37,99,235,0.2)] hover:bg-[#1d4ed8] mt-2 flex items-center justify-center gap-2 transition-all active:scale-95 disabled:opacity-70 disabled:cursor-not-allowed"
          onClick={handleAssignClick}
          disabled={isSubmitting || teachers.length === 0 || classes.length === 0}
        >
          {isSubmitting ? (
             <span className="animate-pulse">Affectation en cours...</span>
          ) : (
            <>
               <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"><polyline points="20 6 9 17 4 12" /></svg>
               Assigner
            </>
          )}
        </button>
      </div>
    </div>
  );
}