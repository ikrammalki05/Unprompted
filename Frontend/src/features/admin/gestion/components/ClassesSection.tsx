import React, { useState, useEffect } from 'react';
import type { ClassData, Teacher } from '../types';
import Modal from '../../../../components/Modal';
import { SectionHeader } from '../../../../components/SectionHeader';
import { IconEdit, IconTrash, IconEye } from '../../../../components/Icons';
import { api } from '../../../../services/api'; 

interface Props {
  classes: ClassData[];
  setClasses: React.Dispatch<React.SetStateAction<ClassData[]>>;
  teachers: Teacher[];
}

export function ClassesSection({ classes, setClasses, teachers }: Props) {
  const [showModal, setShowModal] = useState(false);
  const [editClass, setEditClass] = useState<ClassData | null>(null);
  const [viewClass, setViewClass] = useState<ClassData | null>(null);
  
  // 1. Mise à jour du state initial du formulaire avec les NOUVEAUX champs
  const initialFormState = { 
    nomClasse: '', 
    desc: '', 
    anneeAcademique: '2023 – 2024', 
    effectifActuel: '', 
    effectifMax: '', 
    enseignantReferent: '' 
  };
  const [form, setForm] = useState(initialFormState);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchClasses = async () => {
      try {
        setIsLoading(true);
        const response = await api.get<ClassData[]>('/Classe'); 
        setClasses(response.data);
      } catch (error) {
        console.error("Erreur lors de la récupération des classes:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchClasses();
  }, [setClasses]);

  const openAdd = () => {
    setEditClass(null);
    setForm(initialFormState);
    setShowModal(true);
  };

  const openEdit = (c: ClassData) => {
    setEditClass(c);
    // 2. Utilisation des nouveaux champs pour pré-remplir le formulaire
    setForm({ 
      ...c, 
      effectifActuel: String(c.effectifActuel), 
      effectifMax: String(c.effectifMax) 
    });
    setShowModal(true);
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Supprimer cette classe ?')) {
      try {
        await api.delete(`/Classe/${id}`);
        setClasses(prev => prev.filter(c => c.id !== id));
      } catch (error) {
        console.error("Erreur lors de la suppression:", error);
        alert("Impossible de supprimer la classe.");
      }
    }
  };

  const handleSave = async () => {
    // 3. Vérification sur le nouveau champ "nomClasse"
    if (!form.nomClasse) return;
    
    // 4. Formatage du payload avec les nouveaux champs
    const payload = { 
      ...form, 
      effectifActuel: Number(form.effectifActuel) || 0, 
      effectifMax: Number(form.effectifMax) || 0 
    };

    try {
      if (editClass) {
        const response = await api.put<ClassData>(`/Classe/${editClass.id}`, payload);
        setClasses(prev => prev.map(c => c.id === editClass.id ? response.data : c));
      } else {
        const response = await api.post<ClassData>('/Classe', payload);
        setClasses(prev => [...prev, response.data]);
      }
      setShowModal(false);
    } catch (error) {
      console.error("Erreur lors de l'enregistrement:", error);
      alert("Une erreur est survenue lors de la sauvegarde.");
    }
  };

  return (
    <div className="flex flex-col w-full">
      <SectionHeader title="Gestion des Classes" onAdd={openAdd} addLabel="Créer une classe" accentColor="bg-[#10b981]" />
      
      <div className="bg-white border border-[#f1f5f9] rounded-xl overflow-hidden shadow-[0_1px_2px_rgba(0,0,0,0.03)] focus-within:shadow-[0_4px_12px_rgba(0,0,0,0.05)] transition-shadow">
        <table className="w-full text-left">
          <thead className="bg-[#fcfcfd] border-b border-[#f1f5f9]">
            <tr>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Nom de la Classe</th>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Année Académique</th>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Effectif</th>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Enseignant(s) Référent(s)</th>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider text-right">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#f1f5f9]">
            {isLoading ? (
              <tr>
                <td colSpan={5} className="text-center p-8 text-slate-400">Chargement des classes...</td>
              </tr>
            ) : classes.length === 0 ? (
              <tr>
                <td colSpan={5} className="text-center p-8 text-slate-400">Aucune classe trouvée.</td>
              </tr>
            ) : (
              classes.map(c => {
                 // 5. Calcul du pourcentage avec les nouveaux champs
                 const pct = c.effectifMax ? Math.round((c.effectifActuel / c.effectifMax) * 100) : 0;
                 return (
                  <tr key={c.id} className="hover:bg-[#fcfcfd]/80 transition-colors">
                    <td className="p-[16px_24px]">
                      <div className="flex flex-col">
                        <span className="text-[14px] font-extrabold text-[#1e293b]">{c.nomClasse}</span>
                        <span className="text-[11px] text-[#94a3b8] font-medium">{c.desc}</span>
                      </div>
                    </td>
                    <td className="p-[16px_24px] text-[13px] text-[#1e293b] font-semibold">{c.anneeAcademique}</td>
                    <td className="p-[16px_24px]">
                      <div className="flex items-center gap-3">
                        <div className="h-[5px] w-20 bg-[#f1f5f9] rounded-full overflow-hidden">
                          <div className="h-full bg-[#10b981] rounded-full transition-all duration-500" style={{ width: `${pct}%` }} />
                        </div>
                        <span className="text-[12px] font-extrabold text-[#1e293b]">{c.effectifActuel}/{c.effectifMax}</span>
                      </div>
                    </td>
                    <td className="p-[16px_24px] text-[13px] text-[#1e293b] font-medium">{c.enseignantReferent}</td>
                    <td className="p-[16px_24px] text-right">
                      <div className="flex gap-2 justify-end scale-90">
                        <button className="text-[#64748b] hover:text-[#1e293b] p-1 transition-colors" onClick={() => setViewClass(c)}><IconEye /></button>
                        <button className="text-[#64748b] hover:text-[#1e293b] p-1 transition-colors" onClick={() => openEdit(c)}><IconEdit /></button>
                        <button className="text-[#64748b] hover:text-[#ef4444] p-1 transition-colors" onClick={() => handleDelete(c.id)}><IconTrash /></button>
                      </div>
                    </td>
                  </tr>
                 )
              })
            )}
          </tbody>
        </table>
      </div>

      <Modal isOpen={!!viewClass} onClose={() => setViewClass(null)} title="Détails de la Classe">
        {viewClass && (
          <div className="flex flex-col gap-4 text-sm">
            {/* 6. Mise à jour de l'affichage de la modale de détails */}
            <div className="flex flex-col gap-1"><span className="text-[10px] font-bold text-slate-400 uppercase">Nom</span><span className="font-bold text-slate-800">{viewClass.nomClasse}</span></div>
            <div className="flex flex-col gap-1"><span className="text-[10px] font-bold text-slate-400 uppercase">Année</span><span className="font-medium text-slate-700">{viewClass.anneeAcademique}</span></div>
            <div className="flex flex-col gap-1"><span className="text-[10px] font-bold text-slate-400 uppercase">Effectif</span><span className="font-medium text-slate-700">{viewClass.effectifActuel} / {viewClass.effectifMax}</span></div>
            <div className="flex flex-col gap-1"><span className="text-[10px] font-bold text-slate-400 uppercase">Enseignant</span><span className="font-bold text-blue-600">{viewClass.enseignantReferent}</span></div>
            <div className="flex justify-end mt-4"><button className="bg-[#2563eb] text-white p-[8px_20px] rounded-lg text-sm font-bold" onClick={() => setViewClass(null)}>Fermer</button></div>
          </div>
        )}
      </Modal>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editClass ? 'Modifier la classe' : 'Créer une classe'}>
        <div className="flex flex-col gap-4">
          {/* 7. Mise à jour des inputs du formulaire */}
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Nom de la Classe</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.nomClasse} onChange={e => setForm({ ...form, nomClasse: e.target.value })} />
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Description</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.desc} onChange={e => setForm({ ...form, desc: e.target.value })} />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Effectif Actuel</label>
              <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" type="number" value={form.effectifActuel} onChange={e => setForm({ ...form, effectifActuel: e.target.value })} />
            </div>
            <div>
              <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Capacité Max</label>
              <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" type="number" value={form.effectifMax} onChange={e => setForm({ ...form, effectifMax: e.target.value })} />
            </div>
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Enseignant Référent</label>
            <select className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.enseignantReferent} onChange={e => setForm({ ...form, enseignantReferent: e.target.value })}>
              <option value="">Sélectionner un enseignant</option>
              {teachers.map(t => <option key={t.id} value={t.nom}>{t.nom}</option>)}
            </select>
          </div>
          <div className="flex justify-end gap-3 mt-4">
            <button className="bg-white border border-[#e2e8f0] p-[8px_20px] rounded-lg text-sm font-bold text-[#64748b] hover:bg-slate-50 transition-all" onClick={() => setShowModal(false)}>Annuler</button>
            <button className="bg-[#2563eb] text-white p-[8px_20px] rounded-lg text-sm font-bold hover:bg-[#1d4ed8] transition-all" onClick={handleSave}>{editClass ? 'Enregistrer' : 'Créer'}</button>
          </div>
        </div>
      </Modal>
    </div>
  );
}