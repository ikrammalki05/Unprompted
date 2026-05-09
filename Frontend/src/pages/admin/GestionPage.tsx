import { useState, useMemo } from 'react';
import { 
  StudentsSection, 
  TeachersSection, 
  ClassesSection, 
  AffectationPanel
} from '../../features/admin/gestion';

import type { Student, Teacher, ClassData } from '../../features/admin/gestion';

export const GestionPage = () => {
  // Optionnel : tu peux initialiser avec [] puisque tes composants enfants (StudentsSection, etc.)
  // vont faire eux-mêmes l'appel au backend pour remplir ces listes au chargement.
  const [students, setStudents] = useState<Student[]>([]);
  const [teachers, setTeachers] = useState<Teacher[]>([]);
  const [classes, setClasses] = useState<ClassData[]>([]); 
  
  const [search, setSearch] = useState('');

  const filteredStudents = useMemo(() => 
    students.filter(s => {
      // 1. On récupère le nom (soit nomComplet, soit nom) ou on met une chaîne vide
      const nomAFiltrer = s.nomComplet || s.nom || "";
      
      // 2. On sécurise aussi le code Apogée (correction du doublon)
      const codeAFiltrer = s.codeApogee || s.codeApogee || ""; 
      
      // 3. On sécurise la recherche
      const termeRecherche = search || "";

      // 4. On fait le filtre en toute sécurité
      return (
        nomAFiltrer.toLowerCase().includes(termeRecherche.toLowerCase()) || 
        codeAFiltrer.toLowerCase().includes(termeRecherche.toLowerCase())
      );
    }),
    [students, search]
  );

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
        <div className="flex flex-col gap-8">
          <StudentsSection students={filteredStudents} setStudents={setStudents} />
          <TeachersSection teachers={teachers} setTeachers={setTeachers} />
          <ClassesSection classes={classes} setClasses={setClasses} teachers={teachers} />
        </div>
        
        {/* Colonne Latérale : L'affectation (Sticky) */}
        <div className="min-[1280px]:sticky min-[1280px]:top-10">
          {/* L'AffectationPanel gère l'appel API tout seul, on lui passe juste les setters */}
          <AffectationPanel 
            teachers={teachers} 
            classes={classes} 
            setTeachers={setTeachers} 
            setClasses={setClasses} 
          />
        </div>
        
      </div>
    </div>
  );
};