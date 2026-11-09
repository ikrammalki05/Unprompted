import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  BookOpen,
  GraduationCap,
  Hash,
  Loader2,
  Mail,
  Phone,
  Save,
  Undo2,
  User,
} from 'lucide-react';
import { api } from '../../services/api';
import ProfileLayout from '../../components/profile/ProfileLayout';
import ProfileFormField from '../../components/profile/ProfileFormField';
import ProfileSummaryCard from '../../components/profile/ProfileSummaryCard';

interface FormState {
  Nom: string;
  Prenom: string;
  Email: string;
  CodeApogee: string;
  Niveau: string;
  Filiere: string;
  telephone: string;
}

const emptyForm: FormState = {
  Nom: '',
  Prenom: '',
  Email: '',
  CodeApogee: '',
  Niveau: '',
  Filiere: '',
  telephone: '',
};

const ProfilePage: React.FC = () => {
  const navigate = useNavigate();
  const [form, setForm] = useState<FormState>(emptyForm);
  const [status, setStatus] = useState('Actif');
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const meRes = await api.get('/Profil/me');
        const idEtudiant = meRes.data.idEtudiant;
        const res = idEtudiant
          ? await api.get(`/Profil/etudiant/${idEtudiant}`)
          : meRes;
        const data = res.data;
        const parts = String(data.nomComplet || '').trim().split(/\s+/);
        const prenom = data.prenom || parts[0] || '';
        const nom = data.nom || parts.slice(1).join(' ') || parts[0] || '';
        setForm({
          Nom: nom,
          Prenom: prenom,
          Email: data.email || '',
          CodeApogee: data.codeApogee || '',
          Niveau: data.niveau || '',
          Filiere: data.filiere || '',
          telephone: data.telephone || '',
        });
        setStatus(data.status || data.statut || 'Actif');
      } catch (error) {
        console.error(error);
        setMessage({ type: 'error', text: 'Impossible de charger le profil.' });
      } finally {
        setLoading(false);
      }
    };
    void fetchData();
  }, []);

  const onChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const update = async () => {
    try {
      setSaving(true);
      setMessage(null);
      await api.put('/Profil/me', form);
      setMessage({ type: 'success', text: 'Profil mis à jour avec succès.' });
      setTimeout(() => setMessage(null), 4000);
    } catch (err) {
      console.error(err);
      setMessage({ type: 'error', text: 'Erreur lors de la mise à jour du profil.' });
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[50vh] text-slate-400 gap-3">
        <Loader2 className="w-5 h-5 animate-spin" />
        Chargement du profil…
      </div>
    );
  }

  const fullName = `${form.Prenom} ${form.Nom}`.trim() || 'Étudiant';

  return (
    <ProfileLayout
      title="Mon profil"
      subtitle="Mettez à jour vos informations académiques."
      message={message}
      actions={
        <>
          <button
            type="button"
            onClick={() => navigate(-1)}
            className="inline-flex items-center gap-2 px-5 py-2.5 rounded-xl text-sm font-bold bg-slate-100 text-slate-700 hover:bg-slate-200 transition-colors"
          >
            <Undo2 className="w-4 h-4" />
            Retour
          </button>
          <button
            type="button"
            onClick={() => void update()}
            disabled={saving}
            className="inline-flex items-center gap-2 px-5 py-2.5 rounded-xl text-sm font-bold bg-[#05162b] text-white hover:bg-slate-800 transition-colors disabled:opacity-60"
          >
            {saving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
            {saving ? 'Enregistrement…' : 'Enregistrer'}
          </button>
        </>
      }
      aside={
        <>
          <ProfileSummaryCard
            fullName={fullName}
            email={form.Email}
            roleLabel="Étudiant"
            statusLabel={status}
            meta={[
              { label: 'Filière', value: form.Filiere || '—' },
              { label: 'Niveau', value: form.Niveau || '—' },
            ]}
          />
          <div className="rounded-3xl bg-[#05162b] text-white p-7 relative overflow-hidden">
            <h4 className="font-bold text-lg mb-2">Aide académique</h4>
            <p className="text-sm text-slate-300 leading-relaxed">
              Besoin d’aide pour votre profil ? Contactez le support technique Unprompted.
            </p>
            <GraduationCap className="absolute -bottom-3 -right-3 w-28 h-28 opacity-10" />
          </div>
        </>
      }
    >
      <h2 className="text-lg font-bold text-slate-900 mb-6">Informations personnelles</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
        <ProfileFormField label="Prénom" name="Prenom" value={form.Prenom} onChange={onChange} icon={User} />
        <ProfileFormField label="Nom" name="Nom" value={form.Nom} onChange={onChange} icon={User} />
        <ProfileFormField label="Email académique" name="Email" value={form.Email} icon={Mail} readOnly />
        <ProfileFormField
          label="Code Apogée"
          name="CodeApogee"
          value={form.CodeApogee}
          onChange={onChange}
          icon={Hash}
        />
        <ProfileFormField
          label="Filière"
          name="Filiere"
          value={form.Filiere}
          onChange={onChange}
          icon={BookOpen}
          placeholder="Ex : Génie Informatique"
        />
        <ProfileFormField
          label="Niveau"
          name="Niveau"
          value={form.Niveau}
          onChange={onChange}
          icon={GraduationCap}
          placeholder="Ex : Master 2"
        />
        <ProfileFormField
          label="Téléphone"
          name="telephone"
          value={form.telephone}
          onChange={onChange}
          icon={Phone}
          placeholder="06…"
        />
      </div>
    </ProfileLayout>
  );
};

export default ProfilePage;
