import React, { useState } from 'react';
import type { Teacher } from '../types';
import Modal from '../../../../components/Modal';
import { SectionHeader } from '../../../../components/SectionHeader';
import { IconEdit, IconTrash } from '../../../../components/Icons';

interface Props {
  teachers: Teacher[];
  setTeachers: React.Dispatch<React.SetStateAction<Teacher[]>>;
}

export function TeachersSection({ teachers, setTeachers }: Props) {
  const [showModal, setShowModal] = useState(false);
  const [editTeacher, setEditTeacher] = useState<Teacher | null>(null);
  const [form, setForm] = useState({ nom: '', email: '', specialite: '', classes: '', statut: 'Actif' as 'Actif' | 'Inactif' });

  const openAdd = () => {
    setEditTeacher(null);
    setForm({ nom: '', email: '', specialite: '', classes: '', statut: 'Actif' });
    setShowModal(true);
  };

  const openEdit = (t: Teacher) => {
    setEditTeacher(t);
    setForm({ ...t, classes: t.classes.join(', ') });
    setShowModal(true);
  };

  const handleDelete = (id: number) => {
    if (window.confirm('Supprimer cet enseignant ?')) {
      setTeachers(prev => prev.filter(t => t.id !== id));
    }
  };

  const handleSave = () => {
    if (!form.nom || !form.email) return;
    const payload = { ...form, classes: form.classes.split(',').map(s => s.trim()).filter(Boolean) };
    if (editTeacher) {
      setTeachers(prev => prev.map(t => t.id === editTeacher.id ? { ...payload, id: t.id } : t));
    } else {
      setTeachers(prev => [...prev, { ...payload, id: Date.now() }]);
    }
    setShowModal(false);
  };

  return (
    <div className="flex flex-col mb-10 w-full">
      <SectionHeader title="Gestion des Enseignants" onAdd={openAdd} addLabel="Ajouter un enseignant" accentColor="bg-[#6366f1]" />
      <div className="bg-white border border-[#f1f5f9] rounded-xl overflow-hidden shadow-[0_1px_2px_rgba(0,0,0,0.03)]">
        <table className="w-full text-left">
          <thead className="bg-[#fcfcfd] border-b border-[#f1f5f9]">
            <tr>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Nom Complet</th>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Email</th>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Spécialité</th>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Classes Assignées</th>
              <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider text-right">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#f1f5f9]">
            {teachers.map(t => (
              <tr key={t.id} className="hover:bg-[#fcfcfd]/80 transition-colors">
                <td className="p-[16px_24px]"><span className="text-[14px] font-extrabold text-[#1e293b]">{t.nom}</span></td>
                <td className="p-[16px_24px] text-[13px] text-[#64748b]">{t.email}</td>
                <td className="p-[16px_24px]"><span className="px-2.5 py-1 rounded-md bg-[#eff6ff] text-[#2563eb] text-[11px] font-bold">{t.specialite}</span></td>
                <td className="p-[16px_24px]">
                  <div className="flex gap-2">
                    {t.classes.map((c, i) => <span key={i} className="px-2 py-0.5 rounded border border-[#f1f5f9] bg-[#f8fafc] text-[10px] font-bold text-[#64748b] uppercase">{c}</span>)}
                  </div>
                </td>
                <td className="p-[16px_24px] text-right">
                  <div className="flex gap-2 justify-end">
                    <button className="text-[#64748b] hover:text-[#1e293b] p-1 transition-colors" onClick={() => openEdit(t)}><IconEdit /></button>
                    <button className="text-[#64748b] hover:text-[#ef4444] p-1 transition-colors" onClick={() => handleDelete(t.id)}><IconTrash /></button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editTeacher ? 'Modifier l\'enseignant' : 'Ajouter un enseignant'}>
        <div className="flex flex-col gap-4">
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Nom Complet</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.nom} onChange={e => setForm({ ...form, nom: e.target.value })} />
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Email</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} />
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Spécialité</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.specialite} onChange={e => setForm({ ...form, specialite: e.target.value })} />
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Classes (ex: M1 IABD, L3 Info)</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.classes} onChange={e => setForm({ ...form, classes: e.target.value })} />
          </div>
          <div className="flex justify-end gap-3 mt-4">
            <button className="bg-white border border-[#e2e8f0] p-[8px_20px] rounded-lg text-sm font-bold text-[#64748b] hover:bg-slate-50 transition-all" onClick={() => setShowModal(false)}>Annuler</button>
            <button className="bg-[#2563eb] text-white p-[8px_20px] rounded-lg text-sm font-bold hover:bg-[#1d4ed8] transition-all" onClick={handleSave}>{editTeacher ? 'Enregistrer' : 'Ajouter'}</button>
          </div>
        </div>
      </Modal>
    </div>
  );
}