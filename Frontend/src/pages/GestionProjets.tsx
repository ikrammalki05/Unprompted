import React, { useState, useEffect } from 'react';
import { 
  AlertTriangle, 
  X,
  Check,
  Search,
  UserPlus,
  Calendar,
  Briefcase
} from 'lucide-react';
import projetService from '../services/projetService';
import userService from '../services/userService';

const rolesPredefinis = [
  "Frontend", "Backend", "CI/CD", "Test Qualité", "Infrastructure"
];

const GestionProjets: React.FC = () => {
  // Form State
  const [titre, setTitre] = useState('');
  const [deadline, setDeadline] = useState('');
  const [semaines, setSemaines] = useState('');
  const [rolesSelectionnes, setRolesSelectionnes] = useState<string[]>([]);
  const [autreRole, setAutreRole] = useState('');
  const [etudiantsSelectionnes, setEtudiantsSelectionnes] = useState<number[]>([]);
  const [searchEtudiant, setSearchEtudiant] = useState('');
  
  // Data State
  const [etudiantsDisponibles, setEtudiantsDisponibles] = useState<any[]>([]);
  const [enseignantId, setEnseignantId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const initData = async () => {
      try {
        const profile = await userService.getCurrentUser();
        setEnseignantId(profile.id);

        const students = await userService.getStudents();
        setEtudiantsDisponibles(students);
      } catch (error) {
        console.error("Erreur initialisation", error);
      }
    };
    initData();
  }, []);

  const toggleRole = (role: string) => {
    setRolesSelectionnes(prev => 
      prev.includes(role) ? prev.filter(r => r !== role) : [...prev, role]
    );
  };

  const toggleEtudiant = (id: number) => {
    setEtudiantsSelectionnes(prev => 
      prev.includes(id) ? prev.filter(eid => eid !== id) : [...prev, id]
    );
  };

  const filteredEtudiants = etudiantsDisponibles.filter(e => 
    (e.nomComplet || e.NomComplet || "").toLowerCase().includes(searchEtudiant.toLowerCase())
  );

  const handleCreateProjet = async () => {
    if (!enseignantId) {
        alert("Session non identifiée.");
        return;
    }
    if (!titre || !deadline) {
        alert("Veuillez remplir au moins le titre et la deadline.");
        return;
    }

    try {
      setLoading(true);
      const projetData = {
        titre,
        dateFin: deadline,
        duree: Number(semaines),
        description: `Rôles: ${[...rolesSelectionnes, autreRole].filter(Boolean).join(', ')}`
      };

      await projetService.create(projetData, enseignantId);
      alert('Projet créé avec succès !');
      
      setTitre('');
      setDeadline('');
      setSemaines('');
      setRolesSelectionnes([]);
      setEtudiantsSelectionnes([]);
    } catch (error) {
      console.error('Erreur création projet :', error);
      alert('Erreur lors de la création du projet');
    } finally {
      setLoading(false);
    }
  };

  const getInitials = (name: string) => {
    return name.split(' ').map(n => n[0]).join('').toUpperCase().substring(0, 2);
  };

  return (
    <div className="min-h-screen bg-[#f8fafc] py-12 px-4 animate-in fade-in duration-700">
      <div className="w-full max-w-[800px] mx-auto bg-white rounded-[40px] shadow-[0_32px_64px_-16px_rgba(0,0,0,0.08)] border border-slate-100 overflow-hidden relative">
        
        {/* Header Section */}
        <div className="px-12 pt-12 pb-10 bg-gradient-to-br from-white to-slate-50/50">
          <div className="flex items-center gap-3 mb-4">
            <div className="w-10 h-10 bg-blue-600 rounded-2xl flex items-center justify-center shadow-lg shadow-blue-200">
              <Briefcase className="w-5 h-5 text-white" />
            </div>
            <span className="text-[11px] font-black text-blue-600 uppercase tracking-[0.2em]">Configuration de Projet</span>
          </div>
          <h1 className="text-4xl font-black text-slate-900 tracking-tight mb-3">
            Nouveau Projet
          </h1>
          <p className="text-slate-400 font-medium text-lg">
            Créez une nouvelle opportunité académique pour vos étudiants.
          </p>
        </div>

        <div className="px-12 pb-12 flex flex-col gap-10">
          
          {/* Titre Input */}
          <div className="space-y-4">
            <label className="text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] flex items-center gap-2">
              Titre du projet <span className="w-1 h-1 rounded-full bg-blue-400"></span>
            </label>
            <input
              type="text"
              placeholder="Ex: Développement d'une interface de gestion prédictive..."
              value={titre}
              onChange={(e) => setTitre(e.target.value)}
              className="w-full px-6 py-5 bg-slate-50/50 border border-slate-100 rounded-3xl text-[15px] text-slate-800 font-bold focus:bg-white focus:ring-4 focus:ring-blue-50 focus:border-blue-200 outline-none transition-all placeholder:text-slate-300 shadow-sm"
            />
          </div>

          {/* Rôles Grid */}
          <div className="space-y-5">
            <label className="text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] flex items-center gap-2">
              Rôles Requis <span className="w-1 h-1 rounded-full bg-blue-400"></span>
            </label>
            <div className="grid grid-cols-2 md:grid-cols-3 gap-3">
              {rolesPredefinis.map(role => (
                <button
                  key={role}
                  onClick={() => toggleRole(role)}
                  className={`flex items-center gap-3 p-4 rounded-2xl border transition-all duration-300 ${
                    rolesSelectionnes.includes(role)
                      ? 'bg-blue-600 border-blue-600 text-white shadow-lg shadow-blue-100 scale-[1.02]'
                      : 'bg-white border-slate-100 text-slate-600 hover:border-slate-300 hover:bg-slate-50'
                  }`}
                >
                  <div className={`w-5 h-5 rounded-full flex items-center justify-center transition-colors ${
                    rolesSelectionnes.includes(role) ? 'bg-white/20' : 'bg-slate-100'
                  }`}>
                    {rolesSelectionnes.includes(role) ? <Check className="w-3 h-3 text-white" strokeWidth={4} /> : <div className="w-1.5 h-1.5 rounded-full bg-slate-300" />}
                  </div>
                  <span className="text-[13px] font-bold">{role}</span>
                </button>
              ))}
            </div>
            <div className="relative group">
              <input
                type="text"
                placeholder="Autre rôle spécifique..."
                value={autreRole}
                onChange={(e) => setAutreRole(e.target.value)}
                className="w-full px-6 py-4 bg-slate-50/50 border border-slate-100 rounded-2xl text-[14px] text-slate-800 font-bold focus:bg-white outline-none transition-all group-hover:border-slate-200"
              />
              <Plus className="absolute right-5 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-300" />
            </div>
          </div>

          {/* Date & Duration Row */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
            <div className="space-y-4">
              <label className="text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] flex items-center gap-2">
                Échéance <span className="w-1 h-1 rounded-full bg-blue-400"></span>
              </label>
              <div className="relative">
                <input
                  type="date"
                  value={deadline}
                  onChange={(e) => setDeadline(e.target.value)}
                  className="w-full px-6 py-5 bg-slate-50/50 border border-slate-100 rounded-3xl text-[15px] text-slate-700 font-bold outline-none focus:bg-white transition-all"
                />
                <Calendar className="absolute right-6 top-1/2 -translate-y-1/2 w-5 h-5 text-slate-300 pointer-events-none" />
              </div>
            </div>
            <div className="space-y-4">
              <label className="text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] flex items-center gap-2">
                Durée <span className="w-1 h-1 rounded-full bg-blue-400"></span>
              </label>
              <div className="relative">
                <input
                  type="number"
                  placeholder="Ex: 12"
                  value={semaines}
                  onChange={(e) => setSemaines(e.target.value)}
                  className="w-full px-6 py-5 bg-slate-50/50 border border-slate-100 rounded-3xl text-[15px] text-slate-700 font-bold outline-none focus:bg-white transition-all pr-24"
                />
                <div className="absolute right-6 top-1/2 -translate-y-1/2 flex items-center gap-2 pointer-events-none">
                  <div className="w-px h-4 bg-slate-200"></div>
                  <span className="text-[11px] font-black text-slate-400 uppercase">Semaines</span>
                </div>
              </div>
            </div>
          </div>

          {/* Student Assignment - LUXE VERSION */}
          <div className="space-y-5">
            <label className="text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] flex items-center gap-2">
              Assignation des étudiants <span className="w-1 h-1 rounded-full bg-blue-400"></span>
            </label>
            
            {/* Chips Container */}
            <div className="flex flex-wrap gap-2 mb-4 min-h-[48px] p-2 bg-slate-50/30 rounded-2xl border border-dashed border-slate-200">
              {etudiantsSelectionnes.length === 0 ? (
                <span className="text-slate-300 text-[13px] font-medium italic p-2 flex items-center gap-2">
                  <UserPlus className="w-4 h-4" /> Aucun étudiant sélectionné pour le moment
                </span>
              ) : (
                etudiantsSelectionnes.map(id => {
                  const student = etudiantsDisponibles.find(e => (e.id || e.Id) === id);
                  const name = student?.nomComplet || student?.NomComplet || "Étudiant";
                  return (
                    <div key={id} className="flex items-center gap-2 bg-white border border-slate-100 shadow-sm px-3 py-1.5 rounded-xl animate-in zoom-in duration-300">
                      <div className="w-5 h-5 rounded-full bg-blue-600 text-[9px] text-white flex items-center justify-center font-bold">
                        {getInitials(name)}
                      </div>
                      <span className="text-[12px] font-bold text-slate-700">{name}</span>
                      <button onClick={() => toggleEtudiant(id)} className="p-0.5 hover:bg-slate-100 rounded-full text-slate-400 hover:text-red-500 transition-colors">
                        <X className="w-3.5 h-3.5" />
                      </button>
                    </div>
                  );
                })
              )}
            </div>

            {/* Selection Area */}
            <div className="bg-slate-50 rounded-[32px] border border-slate-100 overflow-hidden shadow-inner">
              <div className="p-4 bg-white border-b border-slate-50 flex items-center gap-3">
                <Search className="w-4 h-4 text-slate-400" />
                <input 
                  type="text" 
                  placeholder="Rechercher un étudiant par nom..." 
                  value={searchEtudiant}
                  onChange={(e) => setSearchEtudiant(e.target.value)}
                  className="flex-1 bg-transparent border-none outline-none text-[13px] font-bold text-slate-700 placeholder:text-slate-300"
                />
              </div>
              <div className="max-h-[220px] overflow-y-auto p-2 grid grid-cols-1 sm:grid-cols-2 gap-2">
                {filteredEtudiants.map(etud => {
                  const id = etud.id || etud.Id;
                  const isSelected = etudiantsSelectionnes.includes(id);
                  const name = etud.nomComplet || etud.NomComplet || "Sans nom";
                  return (
                    <button
                      key={id}
                      onClick={() => toggleEtudiant(id)}
                      className={`flex items-center gap-3 p-3 rounded-2xl transition-all ${
                        isSelected ? 'bg-blue-600 text-white shadow-md' : 'bg-white hover:bg-slate-100 text-slate-600 border border-transparent'
                      }`}
                    >
                      <div className={`w-8 h-8 rounded-full flex items-center justify-center text-[10px] font-black ${
                        isSelected ? 'bg-white/20' : 'bg-slate-100 text-slate-400'
                      }`}>
                        {getInitials(name)}
                      </div>
                      <div className="text-left flex-1 min-w-0">
                        <p className={`text-[13px] font-bold truncate ${isSelected ? 'text-white' : 'text-slate-700'}`}>
                          {name}
                        </p>
                        <p className={`text-[10px] font-semibold opacity-70 ${isSelected ? 'text-white' : 'text-slate-400'}`}>
                          {etud.classeNom || "Étudiant"}
                        </p>
                      </div>
                      {isSelected && <Check className="w-4 h-4" strokeWidth={3} />}
                    </button>
                  );
                })}
              </div>
            </div>
          </div>

          {/* Information Section */}
          <div className="flex gap-5 p-6 bg-blue-50/40 rounded-[32px] border border-blue-100/50 shadow-sm">
            <div className="w-10 h-10 bg-blue-100 rounded-2xl flex items-center justify-center flex-shrink-0">
              <AlertTriangle className="w-5 h-5 text-blue-600" />
            </div>
            <div className="space-y-1">
              <p className="text-[13px] font-black text-blue-900">Information importante</p>
              <p className="text-[12px] text-blue-700/70 leading-relaxed font-semibold">
                Une fois qu'un étudiant est assigné à un rôle spécifique, il est automatiquement retiré de la liste des étudiants disponibles pour d'autres projets afin d'éviter les surcharges de travail.
              </p>
            </div>
          </div>

          {/* Final Action Buttons */}
          <div className="space-y-4 pt-6 border-t border-slate-50">
            <button
              onClick={handleCreateProjet}
              disabled={loading}
              className="group w-full py-6 bg-slate-900 hover:bg-slate-800 text-white text-[16px] font-black rounded-3xl transition-all shadow-2xl shadow-slate-900/20 active:scale-[0.98] disabled:opacity-50 flex items-center justify-center gap-3"
            >
              {loading ? (
                <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></div>
              ) : (
                <>
                  Créer et Notifier le groupe
                  <Check className="w-5 h-5 group-hover:translate-x-1 transition-transform" />
                </>
              )}
            </button>
            <button className="w-full text-center py-2 text-[14px] font-bold text-slate-400 hover:text-slate-900 transition-colors tracking-tight">
              Enregistrer en tant que brouillon
            </button>
          </div>

        </div>
      </div>
    </div>
  );
};

// Simple Plus icon fallback
const Plus = ({className}: {className?: string}) => (
  <svg className={className} fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={3} d="M12 4v16m8-8H4" />
  </svg>
);

export default GestionProjets;