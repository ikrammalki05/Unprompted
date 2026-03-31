import React, { useState, useMemo } from 'react';
import Modal from "../components/Modal";
import './GestionPage.css';

// ─────────────── Sample Data ───────────────
const initialStudents = [
  { id: 1, code: '21904562', nom: 'Amine Benjelloun', email: 'a.benjelloun@univ.ma', classe: 'Master IABD – G1', statut: 'Actif' },
  { id: 2, code: '21808912', nom: 'Sarah Mansouri', email: 's.mansouri@univ.ma', classe: 'Master IABD – G2', statut: 'Actif' },
  { id: 3, code: '20812354', nom: 'Karim Tazi', email: 'k.tazi@univ.ma', classe: 'L3 Informatique', statut: 'Inactif' },
  { id: 4, code: '22101230', nom: 'Nadia El Fassi', email: 'n.elfassi@univ.ma', classe: 'Master IABD – G1', statut: 'Actif' },
  { id: 5, code: '22203311', nom: 'Youssef Berrada', email: 'y.berrada@univ.ma', classe: 'L3 Informatique', statut: 'Actif' },
  { id: 6, code: '21700123', nom: 'Leila Ouali', email: 'l.ouali@univ.ma', classe: 'Master IABD – G2', statut: 'Inactif' },
];

const initialTeachers = [
  { id: 1, nom: 'Dr. Ahmed Alami', email: 'a.alami@univ.ma', specialite: 'Intelligence Artificielle', classes: ['M1 IABD', 'M2 Data'], statut: 'Actif' },
  { id: 2, nom: 'Pr. Myriam Bennani', email: 'm.bennani@univ.ma', specialite: 'Génie Logiciel', classes: ['L3 Info'], statut: 'Actif' },
  { id: 3, nom: 'Dr. Hassan Raji', email: 'h.raji@univ.ma', specialite: 'Réseaux & Systèmes', classes: ['L3 Info', 'M1 IABD'], statut: 'Actif' },
];

const initialClasses = [
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
  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" /><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" /></svg>
);
const IconTrash = () => (
  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="3 6 5 6 21 6" /><path d="M19 6l-1 14H6L5 6" /><path d="M10 11v6M14 11v6" /><path d="M9 6V4h6v2" /></svg>
);
const IconEye = () => (
  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" /><circle cx="12" cy="12" r="3" /></svg>
);
const IconUsers = () => (
  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" /><circle cx="9" cy="7" r="4" /><path d="M23 21v-2a4 4 0 0 0-3-3.87" /><path d="M16 3.13a4 4 0 0 1 0 7.75" /></svg>
);
const IconChevron = ({ dir = 'right' }) => (
  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"
    style={{ transform: dir === 'left' ? 'rotate(180deg)' : '' }}>
    <polyline points="9 18 15 12 9 6" />
  </svg>
);

// ─────────────── Section Header ───────────────
function SectionHeader({ title, onAdd, addLabel }) {
  return (
    <div className="section-header">
      <h2 className="section-title">
        <span className="section-accent" />
        {title}
      </h2>
      {onAdd && (
        <button className="btn btn-primary" onClick={onAdd}>
          <IconPlus /> {addLabel}
        </button>
      )}
    </div>
  );
}

// ─────────────── Students Section ───────────────
function StudentsSection({ students, setStudents }) {
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editStudent, setEditStudent] = useState(null);
  const [form, setForm] = useState({ code: '', nom: '', email: '', classe: '', statut: 'Actif' });

  const filtered = useMemo(() =>
    students.filter(s =>
      s.nom.toLowerCase().includes(search.toLowerCase()) ||
      s.code.includes(search) ||
      s.email.toLowerCase().includes(search.toLowerCase())
    ), [students, search]);

  const totalPages = Math.ceil(filtered.length / PAGE_SIZE);
  const paged = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);

  const openAdd = () => {
    setEditStudent(null);
    setForm({ code: '', nom: '', email: '', classe: '', statut: 'Actif' });
    setShowModal(true);
  };

  const openEdit = (s) => {
    setEditStudent(s);
    setForm({ ...s });
    setShowModal(true);
  };

  const handleDelete = (id) => {
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
    <section className="section fade-in">
      <div className="section-toolbar">
        <SectionHeader title="Gestion des Étudiants" onAdd={openAdd} addLabel="Ajouter un étudiant" />
        <div className="search-bar" style={{ marginBottom: 0 }}>
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><circle cx="11" cy="11" r="8" /><path d="m21 21-4.35-4.35" /></svg>
          <input placeholder="Rechercher..." value={search} onChange={e => { setSearch(e.target.value); setPage(1); }} />
          {search && (
            <button style={{ background: 'none', border: 'none', cursor: 'pointer', color: 'var(--text-muted)', lineHeight: 1 }} onClick={() => setSearch('')}>×</button>
          )}
        </div>
      </div>

      <div className="card">
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Code Apogée</th>
                <th>Nom Complet</th>
                <th>Email</th>
                <th>Classe</th>
                <th>Statut</th>
                <th style={{ textAlign: 'right' }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {paged.length === 0 ? (
                <tr><td colSpan={6} style={{ textAlign: 'center', padding: '24px', color: 'var(--text-muted)' }}>Aucun étudiant trouvé.</td></tr>
              ) : paged.map(s => (
                <tr key={s.id}>
                  <td>
                    <span className="code-link">{s.code}</span>
                  </td>
                  <td><strong>{s.nom}</strong></td>
                  <td style={{ color: 'var(--text-secondary)' }}>{s.email}</td>
                  <td>{s.classe}</td>
                  <td>
                    <span className={`badge ${s.statut === 'Actif' ? 'badge-green' : 'badge-red'}`}>
                      {s.statut}
                    </span>
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '6px', justifyContent: 'flex-end' }}>
                      <button className="action-btn" data-tooltip="Modifier" onClick={() => openEdit(s)}><IconEdit /></button>
                      <button className="action-btn danger" data-tooltip="Supprimer" onClick={() => handleDelete(s.id)}><IconTrash /></button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className="table-footer">
          <span className="table-count">
            Affichage de {Math.min(paged.length, PAGE_SIZE)} sur {filtered.length} étudiants
          </span>
          <div className="pagination">
            <button className="page-btn" onClick={() => setPage(p => p - 1)} disabled={page === 1}><IconChevron dir="left" /></button>
            {Array.from({ length: totalPages }, (_, i) => (
              <button key={i} className={`page-btn ${page === i + 1 ? 'active' : ''}`} onClick={() => setPage(i + 1)}>{i + 1}</button>
            ))}
            <button className="page-btn" onClick={() => setPage(p => p + 1)} disabled={page === totalPages || totalPages === 0}><IconChevron /></button>
          </div>
        </div>
      </div>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editStudent ? 'Modifier l\'étudiant' : 'Ajouter un étudiant'}>
        <div className="form-group">
          <label className="form-label">Code Apogée</label>
          <input className="form-input" placeholder="Ex: 22301234" value={form.code} onChange={e => setForm({ ...form, code: e.target.value })} />
        </div>
        <div className="form-group">
          <label className="form-label">Nom Complet</label>
          <input className="form-input" placeholder="Prénom Nom" value={form.nom} onChange={e => setForm({ ...form, nom: e.target.value })} />
        </div>
        <div className="form-group">
          <label className="form-label">Email</label>
          <input className="form-input" type="email" placeholder="email@univ.ma" value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} />
        </div>
        <div className="form-group">
          <label className="form-label">Classe</label>
          <div className="select-wrapper">
            <select className="form-select" value={form.classe} onChange={e => setForm({ ...form, classe: e.target.value })}>
              <option value="">Sélectionner une classe</option>
              <option>Master IABD – G1</option>
              <option>Master IABD – G2</option>
              <option>L3 Informatique</option>
            </select>
          </div>
        </div>
        <div className="form-group">
          <label className="form-label">Statut</label>
          <div className="select-wrapper">
            <select className="form-select" value={form.statut} onChange={e => setForm({ ...form, statut: e.target.value })}>
              <option>Actif</option>
              <option>Inactif</option>
            </select>
          </div>
        </div>
        <div className="modal-footer">
          <button className="btn btn-outline" onClick={() => setShowModal(false)}>Annuler</button>
          <button className="btn btn-primary" onClick={handleSave}>{editStudent ? 'Enregistrer' : 'Ajouter'}</button>
        </div>
      </Modal>
    </section>
  );
}

// ─────────────── Affectation Panel ───────────────
function AffectationPanel({ teachers, classes }) {
  const [selectedTeacher, setSelectedTeacher] = useState(teachers[0]?.nom || '');
  const [selectedClass, setSelectedClass] = useState(classes[0]?.nom || '');
  const [done, setDone] = useState(false);

  const handleAssign = () => {
    setDone(true);
    setTimeout(() => setDone(false), 2500);
  };

  return (
    <div className="affectation-panel card">
      <h3 className="affectation-title">
        <span className="section-accent" />
        Affectation
      </h3>

      <div className="affectation-desc">
        <strong>Assigner un enseignant</strong>
        <p>Liez un membre du corps professoral à une classe spécifique pour l'année académique.</p>
      </div>

      <div className="form-group">
        <label className="form-label">Sélectionner l'enseignant</label>
        <div className="select-wrapper">
          <select className="form-select" value={selectedTeacher} onChange={e => setSelectedTeacher(e.target.value)}>
            {teachers.map(t => <option key={t.id}>{t.nom}</option>)}
          </select>
        </div>
      </div>

      <div className="form-group">
        <label className="form-label">Sélectionner la classe</label>
        <div className="select-wrapper">
          <select className="form-select" value={selectedClass} onChange={e => setSelectedClass(e.target.value)}>
            {classes.map(c => <option key={c.id}>{c.nom}</option>)}
          </select>
        </div>
      </div>

      {done && (
        <div className="assign-success">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5"><polyline points="20 6 9 17 4 12" /></svg>
          Affectation réussie !
        </div>
      )}

      <button className="btn btn-primary" style={{ width: '100%', justifyContent: 'center', marginTop: '4px' }} onClick={handleAssign}>
        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M5 12h14M12 5l7 7-7 7" /></svg>
        Assigner
      </button>
    </div>
  );
}

// ─────────────── Teachers Section ───────────────
function TeachersSection({ teachers, setTeachers }) {
  const [showModal, setShowModal] = useState(false);
  const [editTeacher, setEditTeacher] = useState(null);
  const [form, setForm] = useState({ nom: '', email: '', specialite: '', classes: '', statut: 'Actif' });

  const openAdd = () => {
    setEditTeacher(null);
    setForm({ nom: '', email: '', specialite: '', classes: '', statut: 'Actif' });
    setShowModal(true);
  };

  const openEdit = (t) => {
    setEditTeacher(t);
    setForm({ ...t, classes: t.classes.join(', ') });
    setShowModal(true);
  };

  const handleDelete = (id) => {
    if (window.confirm('Supprimer cet enseignant ?')) setTeachers(prev => prev.filter(t => t.id !== id));
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

  const specialiteColor = (sp) => {
    if (sp.includes('Artif')) return 'badge-purple';
    if (sp.includes('Génie')) return 'badge-blue';
    return 'badge-orange';
  };

  return (
    <section className="section fade-in" style={{ animationDelay: '0.1s' }}>
      <SectionHeader title="Gestion des Enseignants" onAdd={openAdd} addLabel="Ajouter un enseignant" />

      <div className="card">
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Nom Complet</th>
                <th>Email</th>
                <th>Spécialité</th>
                <th>Classes Assignées</th>
                <th>Statut</th>
                <th style={{ textAlign: 'right' }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {teachers.map(t => (
                <tr key={t.id}>
                  <td>
                    <div className="teacher-cell">
                      <div className="teacher-avatar">{t.nom[0]}</div>
                      <strong>{t.nom}</strong>
                    </div>
                  </td>
                  <td style={{ color: 'var(--text-secondary)' }}>{t.email}</td>
                  <td><span className={`badge ${specialiteColor(t.specialite)}`}>{t.specialite}</span></td>
                  <td>
                    <div style={{ display: 'flex', gap: '4px', flexWrap: 'wrap' }}>
                      {t.classes.map((c, i) => <span key={i} className="badge badge-blue">{c}</span>)}
                    </div>
                  </td>
                  <td>
                    <span className={`badge ${t.statut === 'Actif' ? 'badge-green' : 'badge-red'}`}>{t.statut}</span>
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '6px', justifyContent: 'flex-end' }}>
                      <button className="action-btn" data-tooltip="Modifier" onClick={() => openEdit(t)}><IconEdit /></button>
                      <button className="action-btn danger" data-tooltip="Supprimer" onClick={() => handleDelete(t.id)}><IconTrash /></button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editTeacher ? 'Modifier l\'enseignant' : 'Ajouter un enseignant'}>
        <div className="form-group">
          <label className="form-label">Nom Complet</label>
          <input className="form-input" placeholder="Dr. Prénom Nom" value={form.nom} onChange={e => setForm({ ...form, nom: e.target.value })} />
        </div>
        <div className="form-group">
          <label className="form-label">Email</label>
          <input className="form-input" type="email" placeholder="email@univ.ma" value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} />
        </div>
        <div className="form-group">
          <label className="form-label">Spécialité</label>
          <input className="form-input" placeholder="Intelligence Artificielle, ..." value={form.specialite} onChange={e => setForm({ ...form, specialite: e.target.value })} />
        </div>
        <div className="form-group">
          <label className="form-label">Classes (séparées par des virgules)</label>
          <input className="form-input" placeholder="M1 IABD, L3 Info" value={form.classes} onChange={e => setForm({ ...form, classes: e.target.value })} />
        </div>
        <div className="form-group">
          <label className="form-label">Statut</label>
          <div className="select-wrapper">
            <select className="form-select" value={form.statut} onChange={e => setForm({ ...form, statut: e.target.value })}>
              <option>Actif</option>
              <option>Inactif</option>
            </select>
          </div>
        </div>
        <div className="modal-footer">
          <button className="btn btn-outline" onClick={() => setShowModal(false)}>Annuler</button>
          <button className="btn btn-primary" onClick={handleSave}>{editTeacher ? 'Enregistrer' : 'Ajouter'}</button>
        </div>
      </Modal>
    </section>
  );
}

// ─────────────── Classes Section ───────────────
function ClassesSection({ classes, setClasses, teachers }) {
  const [showModal, setShowModal] = useState(false);
  const [editClass, setEditClass] = useState(null);
  const [viewClass, setViewClass] = useState(null);
  const [form, setForm] = useState({ nom: '', desc: '', annee: '2023 – 2024', effectif: '', capacite: '', enseignant: '' });

  const openAdd = () => {
    setEditClass(null);
    setForm({ nom: '', desc: '', annee: '2023 – 2024', effectif: '', capacite: '', enseignant: '' });
    setShowModal(true);
  };

  const openEdit = (c) => {
    setEditClass(c);
    setForm({ ...c, effectif: String(c.effectif), capacite: String(c.capacite) });
    setShowModal(true);
  };

  const handleDelete = (id) => {
    if (window.confirm('Supprimer cette classe ?')) setClasses(prev => prev.filter(c => c.id !== id));
  };

  const handleSave = () => {
    if (!form.nom) return;
    const payload = { ...form, effectif: Number(form.effectif), capacite: Number(form.capacite) };
    if (editClass) {
      setClasses(prev => prev.map(c => c.id === editClass.id ? { ...payload, id: c.id } : c));
    } else {
      setClasses(prev => [...prev, { ...payload, id: Date.now() }]);
    }
    setShowModal(false);
  };

  return (
    <section className="section fade-in" style={{ animationDelay: '0.2s' }}>
      <SectionHeader title="Gestion des Classes" onAdd={openAdd} addLabel="Créer une classe" />

      <div className="card">
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Nom de la Classe</th>
                <th>Année Académique</th>
                <th>Effectif</th>
                <th>Enseignant(s) Référent(s)</th>
                <th style={{ textAlign: 'right' }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {classes.map(c => {
                const pct = c.capacite ? Math.min(100, Math.round((c.effectif / c.capacite) * 100)) : 0;
                const color = pct >= 90 ? '#ef4444' : pct >= 70 ? '#f97316' : '#22c55e';
                return (
                  <tr key={c.id}>
                    <td>
                      <div>
                        <strong>{c.nom}</strong>
                        {c.desc && <div style={{ fontSize: '12px', color: 'var(--text-muted)', marginTop: '2px' }}>{c.desc}</div>}
                      </div>
                    </td>
                    <td style={{ color: 'var(--text-secondary)' }}>{c.annee}</td>
                    <td>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                        <div className="progress-bar" style={{ width: '80px' }}>
                          <div className="progress-fill" style={{ width: `${pct}%`, background: color }} />
                        </div>
                        <span style={{ fontSize: '12.5px', fontWeight: 600 }}>{c.effectif}/{c.capacite}</span>
                      </div>
                    </td>
                    <td style={{ color: 'var(--text-secondary)' }}>{c.enseignant}</td>
                    <td>
                      <div style={{ display: 'flex', gap: '6px', justifyContent: 'flex-end' }}>
                        <button className="action-btn view" data-tooltip="Voir" onClick={() => setViewClass(c)}><IconEye /></button>
                        <button className="action-btn" data-tooltip="Modifier" onClick={() => openEdit(c)}><IconEdit /></button>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>

      {/* View Modal */}
      <Modal isOpen={!!viewClass} onClose={() => setViewClass(null)} title="Détails de la Classe">
        {viewClass && (
          <div>
            <div className="view-field"><span className="view-label">Nom</span><span>{viewClass.nom}</span></div>
            <div className="view-field"><span className="view-label">Description</span><span>{viewClass.desc}</span></div>
            <div className="view-field"><span className="view-label">Année</span><span>{viewClass.annee}</span></div>
            <div className="view-field"><span className="view-label">Effectif</span><span>{viewClass.effectif} / {viewClass.capacite}</span></div>
            <div className="view-field"><span className="view-label">Enseignant</span><span>{viewClass.enseignant}</span></div>
            <div className="modal-footer">
              <button className="btn btn-outline" onClick={() => setViewClass(null)}>Fermer</button>
              <button className="btn btn-primary" onClick={() => { setViewClass(null); openEdit(viewClass); }}>Modifier</button>
            </div>
          </div>
        )}
      </Modal>

      {/* Edit/Add Modal */}
      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editClass ? 'Modifier la classe' : 'Créer une classe'}>
        <div className="form-group">
          <label className="form-label">Nom de la Classe</label>
          <input className="form-input" placeholder="Master IABD – G1" value={form.nom} onChange={e => setForm({ ...form, nom: e.target.value })} />
        </div>
        <div className="form-group">
          <label className="form-label">Description</label>
          <input className="form-input" placeholder="Description courte" value={form.desc} onChange={e => setForm({ ...form, desc: e.target.value })} />
        </div>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '12px' }}>
          <div className="form-group">
            <label className="form-label">Effectif</label>
            <input className="form-input" type="number" placeholder="30" value={form.effectif} onChange={e => setForm({ ...form, effectif: e.target.value })} />
          </div>
          <div className="form-group">
            <label className="form-label">Capacité</label>
            <input className="form-input" type="number" placeholder="40" value={form.capacite} onChange={e => setForm({ ...form, capacite: e.target.value })} />
          </div>
        </div>
        <div className="form-group">
          <label className="form-label">Enseignant Référent</label>
          <div className="select-wrapper">
            <select className="form-select" value={form.enseignant} onChange={e => setForm({ ...form, enseignant: e.target.value })}>
              <option value="">Sélectionner un enseignant</option>
              {teachers.map(t => <option key={t.id}>{t.nom}</option>)}
            </select>
          </div>
        </div>
        <div className="form-group">
          <label className="form-label">Année Académique</label>
          <input className="form-input" placeholder="2023 – 2024" value={form.annee} onChange={e => setForm({ ...form, annee: e.target.value })} />
        </div>
        <div className="modal-footer">
          <button className="btn btn-outline" onClick={() => setShowModal(false)}>Annuler</button>
          <button className="btn btn-primary" onClick={handleSave}>{editClass ? 'Enregistrer' : 'Créer'}</button>
        </div>
      </Modal>
    </section>
  );
}

// ─────────────── Stats Bar ───────────────
function StatsBar({ students, teachers, classes }) {
  const actifs = students.filter(s => s.statut === 'Actif').length;
  return (
    <div className="stats-bar">
      {[
        { label: 'Étudiants', value: students.length, color: 'var(--accent)', icon: <IconUsers /> },
        { label: 'Actifs', value: actifs, color: 'var(--green)', icon: <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="20 6 9 17 4 12" /></svg> },
        { label: 'Enseignants', value: teachers.length, color: 'var(--purple)', icon: <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" /><circle cx="12" cy="7" r="4" /></svg> },
        { label: 'Classes', value: classes.length, color: 'var(--orange)', icon: <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z" /><path d="M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z" /></svg> },
      ].map((stat, i) => (
        <div key={i} className="stat-card card">
          <div className="stat-icon" style={{ color: stat.color, background: `${stat.color}15` }}>{stat.icon}</div>
          <div>
            <div className="stat-value" style={{ color: stat.color }}>{stat.value}</div>
            <div className="stat-label">{stat.label}</div>
          </div>
        </div>
      ))}
    </div>
  );
}

// ─────────────── Main Page ───────────────
export default function GestionPage() {
  const [students, setStudents] = useState(initialStudents);
  const [teachers, setTeachers] = useState(initialTeachers);
  const [classes, setClasses] = useState(initialClasses);

  return (
    <div className="gestion-page">
      <div className="page-header">
        <div>
          <h1 className="page-title">Gestion des utilisateurs et des classes</h1>
          <p className="page-subtitle">Gérez les étudiants, les enseignants, les classes et les affectations.</p>
        </div>
      </div>

      <StatsBar students={students} teachers={teachers} classes={classes} />

      <div className="page-grid">
        <div className="page-main">
          <StudentsSection students={students} setStudents={setStudents} />
          <TeachersSection teachers={teachers} setTeachers={setTeachers} />
          <ClassesSection classes={classes} setClasses={setClasses} teachers={teachers} />
        </div>
        <div className="page-aside">
          <AffectationPanel teachers={teachers} classes={classes} />
        </div>
      </div>
    </div>
  );
}
