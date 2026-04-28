import { useState, useMemo } from 'react';
import { 
  StudentsSection, 
  TeachersSection, 
  ClassesSection, 
  AffectationPanel,
  initialStudents, 
  initialTeachers, 
  initialClasses,
  
} from '../features/admin/gestion';

import type {Student,Teacher,ClassData} from '../features/admin/gestion'

export const GestionPage = () => {
  const [students, setStudents] = useState<Student[]>(initialStudents);
  const [teachers, setTeachers] = useState<Teacher[]>(initialTeachers);
  const [classes, setClasses] = useState<ClassData[]>(initialClasses);
  const [search, setSearch] = useState('');

  const filteredStudents = useMemo(() => 
    students.filter(s => s.nom.toLowerCase().includes(search.toLowerCase()) || s.code.includes(search)),
  [students, search]);

  const handleAssign = (teacherNom: string, classNom: string) => {
    setClasses(prev => prev.map(c => 
      c.nom === classNom ? { ...c, enseignant: teacherNom } : c
    ));
    setTeachers(prev => prev.map(t => 
      t.nom === teacherNom ? { ...t, classes: Array.from(new Set([...t.classes, classNom])) } : t
    ));
    alert(`${teacherNom} a été assigné à la classe ${classNom} !`);
  };

  return (
    <div className="flex-1 p-[40px] overflow-y-auto max-w-full bg-[#f8fafc]">
      
      {/* En-tête de la page */}
      <div className="flex flex-col mb-8 gap-1.5">
        <h1 className="text-[28px] font-extrabold text-[#1e293b] tracking-tight">
          Gestion des utilisateurs et des classes
        </h1>
        <p className="text-[14.5px] text-[#64748b] font-medium">
          Gérez les étudiants, les enseignants, les classes et les affectations.
        </p>
      </div>

      {/* Barre de recherche */}
      <div className="flex items-center gap-3 bg-[#f1f5f9]/60 rounded-xl p-2.5 w-[380px] mb-10 border border-[#f1f5f9] focus-within:bg-white focus-within:border-[#2563eb] focus-within:shadow-sm transition-all">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" className="text-[#94a3b8] ml-2">
          <circle cx="11" cy="11" r="8" /><path d="m21 21-4.35-4.35" />
        </svg>
        <input 
          placeholder="Rechercher un étudiant..." 
          className="bg-transparent border-none outline-none text-sm w-full font-medium" 
          value={search}
          onChange={e => setSearch(e.target.value)}
        />
      </div>

      {/* Grille principale */}
      <div className="grid grid-cols-1 min-[1280px]:grid-cols-[1fr_320px] gap-10 items-start">
        
        {/* Colonne Principale : Les listes */}
        <div className="flex flex-col">
          <StudentsSection students={filteredStudents} setStudents={setStudents} />
          <TeachersSection teachers={teachers} setTeachers={setTeachers} />
          <ClassesSection classes={classes} setClasses={setClasses} teachers={teachers} />
        </div>
        
        {/* Colonne Latérale : L'affectation (Sticky) */}
        <div className="min-[1280px]:sticky min-[1280px]:top-10">
          <AffectationPanel teachers={teachers} classes={classes} onAssign={handleAssign} />
        </div>
        
      </div>
    </div>
  );
};