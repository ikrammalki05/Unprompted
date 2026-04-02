import React, { useState, useMemo } from 'react';
import Modal from "../components/Modal";

// ─────────────── Types ───────────────
interface Student {
  id: number;
  code: string;
  nom: string;
  email: string;
  classe: string;
  statut: 'Actif' | 'Inactif';
}

interface Teacher {
  id: number;
  nom: string;
  email: string;
  specialite: string;
  classes: string[];
  statut: 'Actif' | 'Inactif';
}

interface ClassData {
  id: number;
  nom: string;
  desc: string;
  annee: string;
  effectif: number;
  capacite: number;
  enseignant: string;
}

// ─────────────── Sample Data ───────────────
const initialStudents: Student[] = [
  { id: 1, code: '21904562', nom: 'Amine Benjelloun', email: 'a.benjelloun@univ.ma', classe: 'Master IABD – G1', statut: 'Actif' },
  { id: 2, code: '21808912', nom: 'Sarah Mansouri', email: 's.mansouri@univ.ma', classe: 'Master IABD – G2', statut: 'Actif' },
  { id: 3, code: '20812354', nom: 'Karim Tazi', email: 'k.tazi@univ.ma', classe: 'L3 Informatique', statut: 'Inactif' },
  { id: 4, code: '22101230', nom: 'Nadia El Fassi', email: 'n.elfassi@univ.ma', classe: 'Master IABD – G1', statut: 'Actif' },
  { id: 5, code: '22203311', nom: 'Youssef Berrada', email: 'y.berrada@univ.ma', classe: 'L3 Informatique', statut: 'Actif' },
  { id: 6, code: '21700123', nom: 'Leila Ouali', email: 'l.ouali@univ.ma', classe: 'Master IABD – G2', statut: 'Inactif' },
];

const initialTeachers: Teacher[] = [
  { id: 1, nom: 'Dr. Ahmed Alami', email: 'a.alami@univ.ma', specialite: 'Intelligence Artificielle', classes: ['M1 IABD', 'M2 Data'], statut: 'Actif' },
  { id: 2, nom: 'Pr. Myriam Bennani', email: 'm.bennani@univ.ma', specialite: 'Génie Logiciel', classes: ['L3 Info'], statut: 'Actif' },
  { id: 3, nom: 'Dr. Hassan Raji', email: 'h.raji@univ.ma', specialite: 'Réseaux & Systèmes', classes: ['L3 Info', 'M1 IABD'], statut: 'Actif' },
];

const initialClasses: ClassData[] = [
  { id: 1, nom: 'Master IABD – G1', desc: 'Intelligence Artificielle et Big Data', annee: '2023 – 2024', effectif: 34, capacite: 40, enseignant: 'Dr. Ahmed Alami' },
  { id: 2, nom: 'L3 Informatique', desc: 'Licence Fondamentale', annee: '2023 – 2024', effectif: 58, capacite: 60, enseignant: 'Pr. Myriam Bennani' },
  { id: 3, nom: 'Master IABD – G2', desc: 'Intelligence Artificielle et Big Data', annee: '2023 – 2024', effectif: 28, capacite: 40, enseignant: 'Dr. Hassan Raji' },
];

const PAGE_SIZE = 3;

// ─────────────── Icons ───────────────
const IconPlus = () => (
  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5"><path d="M12 5v14M5 12h14" /></svg>
);
const IconEdit = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" /><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" /></svg>
);
const IconTrash = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><polyline points="3 6 5 6 21 6" /><path d="M19 6l-1 14H6L5 6" /><path d="M10 11v6M14 11v6" /><path d="M9 6V4h6v2" /></svg>
);
const IconEye = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" /><circle cx="12" cy="12" r="3" /></svg>
);
const IconChevron = ({ dir = 'right' }: { dir?: 'left' | 'right' }) => (
  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" 
    style={{ transform: dir === 'left' ? 'rotate(180deg)' : '' }}>
    <polyline points="15 18 9 12 15 6" />
  </svg>
);

// ─────────────── Section Header ───────────────
interface SectionHeaderProps {
  title: string;
  onAdd?: () => void;
  addLabel?: string;
  accentColor?: string;
}

function SectionHeader({ title, onAdd, addLabel, accentColor = 'bg-[#1e293b]' }: SectionHeaderProps) {
  return (
    <div className="flex items-center justify-between flex-1 min-w-0 mb-4">
      <h2 className="flex items-center gap-3 text-[17px] font-bold text-[#1e293b]">
        <span className={`w-[4px] h-5 ${accentColor} rounded-full shrink-0`} />
        {title}
      </h2>
      {onAdd && (
        <button className="inline-flex items-center gap-2 p-[7px_16px] rounded-lg bg-[#1e293b] text-white text-[13px] font-bold shadow-sm hover:opacity-90 transition-all" onClick={onAdd}>
          <IconPlus /> {addLabel}
        </button>
      )}
    </div>
  );
}

// ─────────────── Students Section ───────────────
function StudentsSection({ students, setStudents }: { students: Student[], setStudents: React.Dispatch<React.SetStateAction<Student[]>> }) {
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

// ─────────────── Teachers Section ───────────────
function TeachersSection({ teachers, setTeachers }: { teachers: Teacher[], setTeachers: React.Dispatch<React.SetStateAction<Teacher[]>> }) {
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

// ─────────────── Classes Section ───────────────
function ClassesSection({ classes, setClasses, teachers }: { classes: ClassData[], setClasses: React.Dispatch<React.SetStateAction<ClassData[]>>, teachers: Teacher[] }) {
  const [showModal, setShowModal] = useState(false);
  const [editClass, setEditClass] = useState<ClassData | null>(null);
  const [viewClass, setViewClass] = useState<ClassData | null>(null);
  const [form, setForm] = useState({ nom: '', desc: '', annee: '2023 – 2024', effectif: '', capacite: '', enseignant: '' });

  const openAdd = () => {
    setEditClass(null);
    setForm({ nom: '', desc: '', annee: '2023 – 2024', effectif: '', capacite: '', enseignant: '' });
    setShowModal(true);
  };

  const openEdit = (c: ClassData) => {
    setEditClass(c);
    setForm({ ...c, effectif: String(c.effectif), capacite: String(c.capacite) });
    setShowModal(true);
  };

  const handleDelete = (id: number) => {
    if (window.confirm('Supprimer cette classe ?')) {
      setClasses(prev => prev.filter(c => c.id !== id));
    }
  };

  const handleSave = () => {
    if (!form.nom) return;
    const payload = { 
      ...form, 
      id: editClass ? editClass.id : Date.now(),
      effectif: Number(form.effectif) || 0, 
      capacite: Number(form.capacite) || 0 
    };
    if (editClass) {
      setClasses(prev => prev.map(c => c.id === editClass.id ? payload as ClassData : c));
    } else {
      setClasses(prev => [...prev, payload as ClassData]);
    }
    setShowModal(false);
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
            {classes.map(c => {
               const pct = c.capacite ? Math.round((c.effectif / c.capacite) * 100) : 0;
               return (
                <tr key={c.id} className="hover:bg-[#fcfcfd]/80 transition-colors">
                  <td className="p-[16px_24px]">
                    <div className="flex flex-col">
                      <span className="text-[14px] font-extrabold text-[#1e293b]">{c.nom}</span>
                      <span className="text-[11px] text-[#94a3b8] font-medium">{c.desc}</span>
                    </div>
                  </td>
                  <td className="p-[16px_24px] text-[13px] text-[#1e293b] font-semibold">{c.annee}</td>
                  <td className="p-[16px_24px]">
                    <div className="flex items-center gap-3">
                      <div className="h-[5px] w-20 bg-[#f1f5f9] rounded-full overflow-hidden">
                        <div className="h-full bg-[#10b981] rounded-full transition-all duration-500" style={{ width: `${pct}%` }} />
                      </div>
                      <span className="text-[12px] font-extrabold text-[#1e293b]">{c.effectif}/{c.capacite}</span>
                    </div>
                  </td>
                  <td className="p-[16px_24px] text-[13px] text-[#1e293b] font-medium">{c.enseignant}</td>
                  <td className="p-[16px_24px] text-right">
                    <div className="flex gap-2 justify-end scale-90">
                      <button className="text-[#64748b] hover:text-[#1e293b] p-1 transition-colors" onClick={() => setViewClass(c)}><IconEye /></button>
                      <button className="text-[#64748b] hover:text-[#1e293b] p-1 transition-colors" onClick={() => openEdit(c)}><IconEdit /></button>
                      <button className="text-[#64748b] hover:text-[#ef4444] p-1 transition-colors" onClick={() => handleDelete(c.id)}><IconTrash /></button>
                    </div>
                  </td>
                </tr>
               )
            })}
          </tbody>
        </table>
      </div>

      {/* View Details Modal */}
      <Modal isOpen={!!viewClass} onClose={() => setViewClass(null)} title="Détails de la Classe">
        {viewClass && (
          <div className="flex flex-col gap-4 text-sm">
            <div className="flex flex-col gap-1"><span className="text-[10px] font-bold text-slate-400 uppercase">Nom</span><span className="font-bold text-slate-800">{viewClass.nom}</span></div>
            <div className="flex flex-col gap-1"><span className="text-[10px] font-bold text-slate-400 uppercase">Année</span><span className="font-medium text-slate-700">{viewClass.annee}</span></div>
            <div className="flex flex-col gap-1"><span className="text-[10px] font-bold text-slate-400 uppercase">Effectif</span><span className="font-medium text-slate-700">{viewClass.effectif} / {viewClass.capacite}</span></div>
            <div className="flex flex-col gap-1"><span className="text-[10px] font-bold text-slate-400 uppercase">Enseignant</span><span className="font-bold text-blue-600">{viewClass.enseignant}</span></div>
            <div className="flex justify-end mt-4"><button className="bg-[#2563eb] text-white p-[8px_20px] rounded-lg text-sm font-bold" onClick={() => setViewClass(null)}>Fermer</button></div>
          </div>
        )}
      </Modal>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editClass ? 'Modifier la classe' : 'Créer une classe'}>
        <div className="flex flex-col gap-4">
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Nom de la Classe</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.nom} onChange={e => setForm({ ...form, nom: e.target.value })} />
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Description</label>
            <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.desc} onChange={e => setForm({ ...form, desc: e.target.value })} />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Effectif</label>
              <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" type="number" value={form.effectif} onChange={e => setForm({ ...form, effectif: e.target.value })} />
            </div>
            <div>
              <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Capacité</label>
              <input className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" type="number" value={form.capacite} onChange={e => setForm({ ...form, capacite: e.target.value })} />
            </div>
          </div>
          <div>
            <label className="block text-[11px] font-bold text-[#64748b] uppercase tracking-wider mb-1.5">Enseignant Référent</label>
            <select className="w-full p-2.5 border border-[#e2e8f0] rounded-lg text-sm bg-white focus:border-[#2563eb] outline-none transition-all" value={form.enseignant} onChange={e => setForm({ ...form, enseignant: e.target.value })}>
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

// ─────────────── Affectation Panel ───────────────
function AffectationPanel({ teachers, classes, onAssign }: { teachers: Teacher[], classes: ClassData[], onAssign: (t: string, c: string) => void }) {
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

// ─────────────── Main Page ───────────────
export default function GestionPage() {
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
      <div className="flex flex-col mb-8 gap-1.5">
        <h1 className="text-[28px] font-extrabold text-[#1e293b] tracking-tight">Gestion des utilisateurs et des classes</h1>
        <p className="text-[14.5px] text-[#64748b] font-medium">Gérez les étudiants, les enseignants, les classes et les affectations.</p>
      </div>

      <div className="flex items-center gap-3 bg-[#f1f5f9]/60 rounded-xl p-2.5 w-[380px] mb-10 border border-[#f1f5f9] focus-within:bg-white focus-within:border-[var(--accent)] focus-within:shadow-sm transition-all">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" className="text-[#94a3b8] ml-2"><circle cx="11" cy="11" r="8" /><path d="m21 21-4.35-4.35" /></svg>
        <input 
          placeholder="Rechercher un étudiant..." 
          className="bg-transparent border-none outline-none text-sm w-full font-medium" 
          value={search}
          onChange={e => setSearch(e.target.value)}
        />
      </div>

      <div className="grid grid-cols-1 min-[1280px]:grid-cols-[1fr_320px] gap-10 items-start">
        <div className="flex flex-col">
          <StudentsSection students={filteredStudents} setStudents={setStudents} />
          <TeachersSection teachers={teachers} setTeachers={setTeachers} />
          <ClassesSection classes={classes} setClasses={setClasses} teachers={teachers} />
        </div>
        <div className="min-[1280px]:sticky min-[1280px]:top-10">
          <AffectationPanel teachers={teachers} classes={classes} onAssign={handleAssign} />
        </div>
      </div>
    </div>
  );
}
