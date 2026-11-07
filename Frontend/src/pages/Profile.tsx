import React, { useEffect, useState } from 'react';
import {
  BookOpen,
  Briefcase,
  Building,
  Hash,
  Loader2,
  Mail,
  Phone,
  Save,
  Undo2,
  User,
} from 'lucide-react';
import userService from '../services/userService';
import profileService from '../services/profileService';
import { getUserMainRole } from '../utils/authUtils';
import ProfileLayout from '../components/profile/ProfileLayout';
import ProfileFormField from '../components/profile/ProfileFormField';
import ProfileSummaryCard from '../components/profile/ProfileSummaryCard';

const Profile: React.FC = () => {
  const [profile, setProfile] = useState<any>(null);
  const [formData, setFormData] = useState<any>({});
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);
  const role = getUserMainRole();
  const isEtudiant = role === 'etudiant';

  useEffect(() => {
    void fetchProfile();
  }, [role]);

  const fetchProfile = async () => {
    try {
      setLoading(true);
      const data = isEtudiant
        ? await profileService.getMyProfile()
        : await userService.getCurrentUser();
      setProfile(data);

      if (isEtudiant) {
        const nameParts = String(data.nomComplet || '').split(/\s+/);
        setFormData({
          nom: nameParts.slice(1).join(' ') || nameParts[0] || '',
          prenom: nameParts[0] || '',
          email: data.email,
          codeApogee: data.codeApogee,
          niveau: data.niveau || '',
          filiere: data.filiere || '',
        });
      } else {
        setFormData({
          nom: data.nom || '',
          prenom: data.prenom || '',
          email: data.email,
          specialite: data.specialite || '',
          departement: data.departement || 'Informatique',
          titre: data.titre || 'Professeur',
          telephone: data.telephone || '',
        });
      }
    } catch (error) {
      console.error('Erreur lors du chargement du profil', error);
      setMessage({ type: 'error', text: 'Impossible de charger les données du profil.' });
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev: any) => ({ ...prev, [name]: value }));
  };

  const handleSave = async () => {
    try {
      setSaving(true);
      setMessage(null);
      if (isEtudiant) {
        await profileService.updateMyProfile(formData);
      } else {
        await userService.updateMyProfile(formData);
      }
      setMessage({ type: 'success', text: 'Profil mis à jour avec succès.' });
      setTimeout(() => setMessage(null), 4000);
      await fetchProfile();
    } catch (error: any) {
      console.error(error);
      setMessage({
        type: 'error',
        text: error.response?.data?.message || 'Une erreur est survenue lors de la sauvegarde.',
      });
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[60vh] text-slate-400 gap-3">
        <Loader2 className="w-5 h-5 animate-spin" />
        Chargement…
      </div>
    );
  }

  if (!profile) {
    return (
      <div className="p-8 text-center text-slate-500 font-medium">
        Impossible de charger les données du profil.
      </div>
    );
  }

  const displayFullName = isEtudiant
    ? profile.nomComplet
    : profile.nomComplet || `${formData.prenom || ''} ${formData.nom || ''}`.trim();

  return (
    <ProfileLayout
      title="Informations personnelles"
      subtitle="Mettez à jour vos informations de profil pour l’institution."
      message={message}
      actions={
        <>
          <button
            type="button"
            onClick={() => window.history.back()}
            className="inline-flex items-center gap-2 px-5 py-2.5 rounded-xl text-sm font-bold bg-slate-100 text-slate-700 hover:bg-slate-200 transition-colors"
          >
            <Undo2 className="w-4 h-4" />
            Retour
          </button>
          <button
            type="button"
            onClick={() => void handleSave()}
            disabled={saving}
            className="inline-flex items-center gap-2 px-5 py-2.5 rounded-xl text-sm font-bold bg-[#05162b] text-white hover:bg-slate-800 transition-colors disabled:opacity-60 min-w-[140px] justify-center"
          >
            {saving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
            {saving ? 'Enregistrement…' : 'Enregistrer'}
          </button>
        </>
      }
      aside={
        <>
          <ProfileSummaryCard
            fullName={displayFullName || 'Utilisateur'}
            email={profile.email}
            roleLabel={isEtudiant ? 'Étudiant' : 'Enseignant'}
            statusLabel={profile.statut || profile.status || 'Actif'}
            meta={
              isEtudiant
                ? [
                    { label: 'Filière', value: formData.filiere || '—' },
                    { label: 'Niveau', value: formData.niveau || '—' },
                  ]
                : [
                    { label: 'Spécialité', value: formData.specialite || '—' },
                    { label: 'Département', value: formData.departement || '—' },
                  ]
            }
          />
          <div className="bg-white rounded-3xl border border-slate-100 shadow-sm p-6">
            <h3 className="text-[11px] font-bold uppercase tracking-[0.12em] text-slate-400 mb-4">
              Activité académique
            </h3>
            <div className="grid grid-cols-2 gap-3">
              <div className="rounded-2xl bg-slate-50 border border-slate-100 p-5 text-center">
                <p className="text-2xl font-black text-blue-600">
                  {isEtudiant ? '—' : profile.nbProjets ?? '—'}
                </p>
                <p className="mt-1 text-[10px] font-bold uppercase tracking-wider text-slate-400">
                  Projets
                </p>
              </div>
              <div className="rounded-2xl bg-slate-50 border border-slate-100 p-5 text-center">
                <p className="text-2xl font-black text-emerald-600">
                  {isEtudiant ? '—' : profile.nbEvaluations ?? '—'}
                </p>
                <p className="mt-1 text-[10px] font-bold uppercase tracking-wider text-slate-400">
                  {isEtudiant ? 'Cours' : 'Évaluations'}
                </p>
              </div>
            </div>
          </div>
        </>
      }
    >
      <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
        <ProfileFormField
          label="Prénom"
          name="prenom"
          value={formData.prenom}
          icon={User}
          onChange={handleInputChange}
        />
        <ProfileFormField
          label="Nom"
          name="nom"
          value={formData.nom}
          icon={User}
          onChange={handleInputChange}
        />
        <ProfileFormField
          label="Email académique"
          name="email"
          value={formData.email}
          icon={Mail}
          readOnly
        />

        {isEtudiant ? (
          <>
            <ProfileFormField
              label="Code Apogée"
              name="codeApogee"
              value={formData.codeApogee}
              icon={Hash}
              onChange={handleInputChange}
            />
            <ProfileFormField
              label="Niveau"
              name="niveau"
              value={formData.niveau}
              icon={BookOpen}
              onChange={handleInputChange}
            />
            <ProfileFormField
              label="Filière"
              name="filiere"
              value={formData.filiere}
              icon={BookOpen}
              onChange={handleInputChange}
            />
          </>
        ) : (
          <>
            <ProfileFormField
              label="Département"
              name="departement"
              value={formData.departement}
              icon={Building}
              onChange={handleInputChange}
            />
            <ProfileFormField
              label="Titre"
              name="titre"
              value={formData.titre}
              icon={Briefcase}
              onChange={handleInputChange}
            />
            <ProfileFormField
              label="Spécialité"
              name="specialite"
              value={formData.specialite}
              icon={BookOpen}
              onChange={handleInputChange}
            />
            <ProfileFormField
              label="Téléphone"
              name="telephone"
              value={formData.telephone}
              icon={Phone}
              onChange={handleInputChange}
              placeholder="06…"
            />
          </>
        )}
      </div>
    </ProfileLayout>
  );
};

export default Profile;
