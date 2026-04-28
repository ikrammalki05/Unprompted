import  { useState } from 'react';
import type { Teacher, ClassData } from '../types';

interface Props {
  teachers: Teacher[];
  classes: ClassData[];
  onAssign: (t: string, c: string) => void;
}

export function AffectationPanel({ teachers, classes, onAssign }: Props) {
  const [selectedTeacher, setSelectedTeacher] = useState(teachers[0]?.nom || '');
  const [selectedClass, setSelectedClass] = useState(classes[0]?.nom || '');

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
            className="w-full p-2.5 bg-[#f8fafc] border border-[#f1f5f9] rounded-lg text-sm text-[#1e293b] font-medium outline-none appearance-none cursor-pointer" 
            value={selectedTeacher} 
            onChange={e => setSelectedTeacher(e.target.value)}
          >
            {teachers.map(t => <option key={t.id} value={t.nom}>{t.nom}</option>)}
          </select>
        </div>
        <div>
          <label className="block text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider mb-2">Sélectionner la classe</label>
          <select 
            className="w-full p-2.5 bg-[#f8fafc] border border-[#f1f5f9] rounded-lg text-sm text-[#1e293b] font-medium outline-none appearance-none cursor-pointer"
            value={selectedClass}
            onChange={e => setSelectedClass(e.target.value)}
          >
            {classes.map(c => <option key={c.id} value={c.nom}>{c.nom}</option>)}
          </select>
        </div>
        <button 
          className="w-full bg-[#2563eb] text-white p-3 rounded-lg text-[13.5px] font-bold shadow-[0_4px_12px_rgba(37,99,235,0.2)] hover:bg-[#1d4ed8] mt-2 flex items-center justify-center gap-2 transition-all active:scale-95"
          onClick={() => onAssign(selectedTeacher, selectedClass)}
        >
           <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"><polyline points="20 6 9 17 4 12" /></svg>
           Assigner
        </button>
      </div>
    </div>
  );
}