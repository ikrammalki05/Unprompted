import React, { useState } from 'react';
import type { Student } from '../types';
import Modal from '../../../../components/Modal';
import { SectionHeader } from '../../../../components/SectionHeader';
import { IconEdit, IconTrash, IconChevron } from '../../../../components/Icons';

interface Props {
  students: Student[];
  setStudents: React.Dispatch<React.SetStateAction<Student[]>>;
}

const PAGE_SIZE = 3;

export function StudentsSection({ students, setStudents }: Props) {
  const [page, setPage] = useState(1);
  const [showModal, setShowModal] = useState(false);
  const [editStudent, setEditStudent] = useState<Student | null>(null);
  const [form, setForm] = useState<Omit<Student, 'id'>>({ code: '', nom: '', email: '', classe: '', statut: 'Actif' });

  const totalPages = Math.ceil(students.length / PAGE_SIZE);
  const paged = students.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);

  const openAdd = () => {
    setEditStudent(null);
    setForm({ code: '', nom: '', email: '', classe: '', statut: 'Actif' });
    setShowModal(true);
  };

  const openEdit = (s: Student) => {
    setEditStudent(s);
    setForm({ ...s });
    setShowModal(true);
  };

  const handleDelete = (id: number) => {
    if (window.confirm('Supprimer cet étudiant ?')) {
      setStudents(prev => prev.filter(s => s.id !== id));
    }
  };

  const handleSave = () => {
    if (!form.code || !form.nom || !form.email) return;
    if (editStudent) {
      setStudents(prev => prev.map(s => s.id === editStudent.id ? { ...form, id: s.id } : s));
    } else {
      setStudents(prev => [...prev, { ...form, id: Date.now() }]);
    }
    setShowModal(false);
  };

  return (
    <div className="flex flex-col mb-10 w-full">
      <SectionHeader title="Gestion des Étudiants" onAdd={openAdd} addLabel="Ajouter un étudiant" />

      <div className="bg-white border border-[#f1f5f9] rounded-xl overflow-hidden shadow-[0_1px_2px_rgba(0,0,0,0.03)] flex flex-col">
        <div className="overflow-x-auto">
          <table className="w-full text-left">
            <thead className="bg-[#fcfcfd] border-b border-[#f1f5f9]">
              <tr>
                <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Code Apogée</th>
                <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Nom Complet</th>
                <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Email</th>
                <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Classe</th>
                <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider">Statut</th>
                <th className="p-[14px_24px] text-[10px] font-bold text-[#94a3b8] uppercase tracking-wider text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-[#f1f5f9]">
              {paged.map(s => (
                <tr key={s.id} className="hover:bg-[#fcfcfd]/80 transition-colors">
                  <td className="p-[16px_24px]"><span className="text-[13px] font-bold text-[#2563eb] hover:underline cursor-pointer">{s.code}</span></td>
                  <td className="p-[16px_24px]"><span className="text-[14px] font-extrabold text-[#1e293b]">{s.nom}</span></td>
                  <td className="p-[16px_24px] text-[13px] text-[#64748b]">{s.email}</td>
                  <td className="p-[16px_24px] text-[13px] text-[#1e293b] font-medium">{s.classe}</td>
                  <td className="p-[16px_24px]">
                    <span className={`inline-flex px-2.5 py-1 rounded-full text-[11px] font-bold ${s.statut === 'Actif' ? 'bg-[#dcfce7] text-[#15803d]' : 'bg-[#fee2e2] text-[#dc2626]'}`}>
                      {s.statut}
                    </span>
                  </td>
                  <td className="p-[16px_24px] text-right">
                    <div className="flex gap-2 justify-end">
                      <button className="text-[#64748b] hover:text-[#1e293b] p-1" onClick={() => openEdit(s)}><IconEdit /></button>
                      <button className="text-[#64748b] hover:text-[#ef4444] p-1" onClick={() => handleDelete(s.id)}><IconTrash /></button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className="flex items-center justify-between p-[14px_24px] border-t border-[#f1f5f9] bg-[#fcfcfd]">
          <span className="text-[12px] font-medium text-[#94a3b8]">Affichage de {paged.length} sur {students.length} étudiants</span>
          <div className="flex gap-2">
            <button className="w-8 h-8 flex items-center justify-center rounded-lg border border-[#f1f5f9] text-[#64748b] hover:bg-white disabled:opacity-40" onClick={() => setPage(p => p - 1)} disabled={page === 1}><IconChevron dir="left" /></button>
            <button className="w-8 h-8 flex items-center justify-center rounded-lg border border-[#f1f5f9] text-[#64748b] hover:bg-white disabled:opacity-40" onClick={() => setPage(p => p + 1)} disabled={page === totalPages}><IconChevron /></button>
          </div>
        </div>
      </div>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editStudent ? 'Modifier l\'étudiant' : 'Ajouter un étudiant'}>
        <div className="flex flex-col gap-4">
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Code Apogée</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.code} onChange={e => setForm({ ...form, code: e.target.value })} />
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Nom Complet</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.nom} onChange={e => setForm({ ...form, nom: e.target.value })} />
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Email</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} />
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Classe</label>
            <select className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.classe} onChange={e => setForm({ ...form, classe: e.target.value })}>
              <option value="">Sélectionner une classe</option>
              <option>Master IABD – G1</option>
              <option>Master IABD – G2</option>
              <option>L3 Informatique</option>
            </select>
          </div>
          <div className="flex justify-end gap-3 mt-4">
            <button className="bg-white border border-[#e2e8f0] p-[8px_20px] rounded-lg text-sm font-bold text-[#64748b] hover:bg-slate-50 transition-all" onClick={() => setShowModal(false)}>Annuler</button>
            <button className="bg-[#2563eb] text-white p-[8px_20px] rounded-lg text-sm font-bold hover:bg-[#1d4ed8] transition-all" onClick={handleSave}>{editStudent ? 'Enregistrer' : 'Ajouter'}</button>
          </div>
        </div>
      </Modal>
    </div>
  );
}