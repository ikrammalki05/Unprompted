import React, { useState } from 'react';
import './ProfilePage.css';

// ── Admin data (could come from context/API in real app) ──
const adminData = {
  nom: 'Administrateur',
  email: 'admin@unprompted.edu',
  telephone: '00000000',
  role: 'Administration Centrale',
  institution: 'École Nationale des Sciences Appliquées',
  localisation: 'Tanger, Maroc',
};

// ── Icons ──
const IconBell = () => (
  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9" />
    <path d="M13.73 21a2 2 0 0 1-3.46 0" />
  </svg>
);

const IconEdit = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" />
    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" />
  </svg>
);

const IconUser = () => (
  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
    <circle cx="12" cy="7" r="4" />
  </svg>
);

const IconBuilding = () => (
  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <rect x="3" y="9" width="18" height="13" rx="1" />
    <path d="M8 9V5a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v4" />
    <line x1="12" y1="13" x2="12" y2="17" />
    <line x1="10" y1="15" x2="14" y2="15" />
  </svg>
);

const IconPin = () => (
  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
    <circle cx="12" cy="10" r="3" />
  </svg>
);

const IconSave = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2z" />
    <polyline points="17 21 17 13 7 13 7 21" />
    <polyline points="7 3 7 8 15 8" />
  </svg>
);

const IconClose = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <line x1="18" y1="6" x2="6" y2="18" />
    <line x1="6" y1="6" x2="18" y2="18" />
  </svg>
);

// ── Field component ──
function InfoField({ label, value, editable, editValue, onChange }) {
  return (
    <div className="info-field">
      <span className="info-label">{label}</span>
      {editable ? (
        <input
          className="info-input"
          value={editValue}
          onChange={(e) => onChange(e.target.value)}
        />
      ) : (
        <span className="info-value">{value}</span>
      )}
    </div>
  );
}

// ── Main page ──
export default function ProfilePage() {
  const [editing, setEditing] = useState(false);
  const [data, setData] = useState(adminData);
  const [draft, setDraft] = useState(adminData);
  const [saved, setSaved] = useState(false);
  const [notifOpen, setNotifOpen] = useState(false);

  const handleEdit = () => {
    setDraft({ ...data });
    setEditing(true);
  };

  const handleSave = () => {
    setData({ ...draft });
    setEditing(false);
    setSaved(true);
    setTimeout(() => setSaved(false), 2500);
  };

  const handleCancel = () => {
    setEditing(false);
    setDraft({ ...data });
  };

  return (
    <div className="profile-page">

      {/* ── Top bar ── */}
      <header className="profile-topbar">
        <div className="topbar-left">
          <h1 className="topbar-title">Profil Utilisateur</h1>
          <span className="badge-admin">COMPTE ADMINISTRATEUR</span>
        </div>
        <div className="topbar-right">
          <button
            className={`notif-btn ${notifOpen ? 'active' : ''}`}
            onClick={() => setNotifOpen((o) => !o)}
            aria-label="Notifications"
          >
            <IconBell />
            <span className="notif-dot" />
          </button>
          <div className="topbar-user">
            <span className="topbar-user-name">{data.nom}</span>
            <span className="topbar-user-email">{data.email}</span>
          </div>
        </div>
      </header>

      {/* ── Welcome banner ── */}
      <div className="welcome-banner">
        <div className="welcome-left">
          <div className="welcome-avatar">{data.nom[0]}</div>
          <div>
            <h2 className="welcome-title">Bienvenue, {data.nom}</h2>
            <p className="welcome-sub">Gérez vos paramètres personnels et les configurations de l'établissement.</p>
          </div>
        </div>
        <div className="welcome-right">
          {saved && (
            <div className="save-toast">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5"><polyline points="20 6 9 17 4 12" /></svg>
              Modifications enregistrées
            </div>
          )}
          {editing ? (
            <div style={{ display: 'flex', gap: '10px' }}>
              <button className="btn-banner btn-banner-outline" onClick={handleCancel}>
                <IconClose /> Annuler
              </button>
              <button className="btn-banner btn-banner-primary" onClick={handleSave}>
                <IconSave /> Enregistrer
              </button>
            </div>
          ) : (
            <button className="btn-banner btn-banner-primary" onClick={handleEdit}>
              <IconEdit /> Modifier le profil
            </button>
          )}
        </div>
      </div>

      {/* ── Cards grid ── */}
      <div className="profile-grid">

        {/* Informations personnelles */}
        <div className="profile-card">
          <div className="card-header">
            <div className="card-icon">
              <IconUser />
            </div>
            <h3 className="card-title">Informations personnelles</h3>
          </div>
          <div className="card-fields">
            <InfoField
              label="NOM COMPLET"
              value={data.nom}
              editable={editing}
              editValue={draft.nom}
              onChange={(v) => setDraft({ ...draft, nom: v })}
            />
            <InfoField
              label="ADRESSE E-MAIL"
              value={data.email}
              editable={editing}
              editValue={draft.email}
              onChange={(v) => setDraft({ ...draft, email: v })}
            />
            <InfoField
              label="TÉLÉPHONE"
              value={data.telephone}
              editable={editing}
              editValue={draft.telephone}
              onChange={(v) => setDraft({ ...draft, telephone: v })}
            />
            <div className="info-field">
              <span className="info-label">RÔLE AU SEIN DU SYSTÈME</span>
              <span className="info-value role-value">
                <span className="role-dot" />
                {editing ? (
                  <input
                    className="info-input"
                    value={draft.role}
                    onChange={(e) => setDraft({ ...draft, role: e.target.value })}
                  />
                ) : (
                  <strong>{data.role}</strong>
                )}
              </span>
            </div>
          </div>
        </div>

        {/* Détails de l'établissement */}
        <div className="profile-card">
          <div className="card-header">
            <div className="card-icon">
              <IconBuilding />
            </div>
            <h3 className="card-title">Détails de l'établissement</h3>
          </div>
          <div className="etablissement-grid">
            <div className="etab-item">
              <span className="etab-label">INSTITUTION</span>
              {editing ? (
                <input
                  className="info-input"
                  value={draft.institution}
                  onChange={(e) => setDraft({ ...draft, institution: e.target.value })}
                />
              ) : (
                <strong className="etab-value">{data.institution}</strong>
              )}
            </div>
            <div className="etab-item">
              <span className="etab-label">LOCALISATION</span>
              <span className="etab-location">
                <IconPin />
                {editing ? (
                  <input
                    className="info-input"
                    value={draft.localisation}
                    onChange={(e) => setDraft({ ...draft, localisation: e.target.value })}
                  />
                ) : (
                  data.localisation
                )}
              </span>
            </div>
          </div>
        </div>

      </div>

      {/* Notification dropdown */}
      {notifOpen && (
        <div className="notif-dropdown">
          <div className="notif-header">Notifications</div>
          <div className="notif-item unread">
            <div className="notif-dot-inline" />
            <div>
              <p>Nouvel étudiant ajouté à Master IABD – G1</p>
              <span>Il y a 5 min</span>
            </div>
          </div>
          <div className="notif-item">
            <div className="notif-dot-inline" style={{ opacity: 0 }} />
            <div>
              <p>Affectation mise à jour pour Dr. Alami</p>
              <span>Il y a 1h</span>
            </div>
          </div>
          <div className="notif-item">
            <div className="notif-dot-inline" style={{ opacity: 0 }} />
            <div>
              <p>Rapport mensuel disponible</p>
              <span>Hier</span>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
