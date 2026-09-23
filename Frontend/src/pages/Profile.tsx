import React, { useState, useEffect } from 'react';
import { 
  User, 
  Phone, 
  Building, 
  Briefcase, 
  Undo2, 
  Save, 
  ShieldCheck, 
  Mail, 
  BadgeCheck,
  GraduationCap,
  BookOpen,
  Hash,
  Loader2,
  CheckCircle2,
  AlertCircle
} from 'lucide-react';
import userService from '../services/userService';
import profileService from '../services/profileService';
import { getUserMainRole } from '../utils/authUtils';

const ProfileField: React.FC<{ 
  label: string; 
  name: string;
  value: string; 
  icon: React.ElementType; 
  placeholder?: string;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  readOnly?: boolean;
}> = ({ label, name, value, icon: Icon, placeholder, onChange, readOnly }) => (
  <div className="space-y-2">
    <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest">{label}</label>
    <div className="relative group">
      <div className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-300 group-focus-within:text-blue-500 transition-colors">
        <Icon className="w-4 h-4" />
      </div>
      <input 
        type="text" 
        name={name}
        value={value || ''}
        onChange={onChange}
        readOnly={readOnly}
        placeholder={placeholder}
        className={`w-full pl-11 pr-4 py-3.5 border rounded-2xl text-slate-900 font-bold focus:outline-none transition-all placeholder:text-slate-400 ${
          readOnly 
            ? 'bg-slate-50 border-slate-100 cursor-not-allowed' 
            : 'bg-white border-blue-200 focus:ring-4 focus:ring-blue-50 focus:border-blue-400'
        }`}
      />
    </div>
  </div>
);

const Profile: React.FC = () => {
  const [profile, setProfile] = useState<any>(null);
  const [formData, setFormData] = useState<any>({});
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error', text: string } | null>(null);
  const role = getUserMainRole();

  useEffect(() => {
    fetchProfile();
  }, [role]);

  const fetchProfile = async () => {
    try {
      setLoading(true);
      let data;
      if (role === 'etudiant') {
        data = await profileService.getMyProfile();
      } else {
        data = await userService.getCurrentUser();
      }
      setProfile(data);
      
      // Initialisation du formulaire
      if (role === 'etudiant') {
        const nameParts = (data.nomComplet || "").split(" ");
        const prenom = nameParts[0] || "";
        const nom = nameParts.slice(1).join(" ") || nameParts[0] || "";
        
        setFormData({
          nom: nom,
          prenom: prenom,
          email: data.email,
          codeApogee: data.codeApogee,
          niveau: data.niveau || "",
          filiere: data.filiere || ""
        });
      } else {
        setFormData({
          nom: data.nom || (data.nomComplet?.split(" ")[1] || ""),
          prenom: data.prenom || (data.nomComplet?.split(" ")[0] || ""),
          email: data.email,
          specialite: data.specialite || "",
          departement: data.departement || "Informatique",
          titre: data.titre || "Professeur"
        });
      }
    } catch (error) {
      console.error("Erreur lors du chargement du profil", error);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev: any) => ({ ...prev, [name]: value }));
  };

  const handleSave = async () => {
    try {
      setSaving(true);
      setMessage(null);
      
      if (role === 'etudiant') {
        await profileService.updateMyProfile(formData);
      } else {
        await userService.updateMyProfile(formData);
      }
      
      setMessage({ type: 'success', text: 'Votre profil a été mis à jour avec succès !' });
      
      // Cacher le message après 5 secondes
      setTimeout(() => setMessage(null), 5000);
      
      // Recharger les données pour rafraîchir l'affichage
      await fetchProfile();
    } catch (error: any) {
      console.error("Erreur lors de la sauvegarde", error);
      setMessage({ 
        type: 'error', 
        text: 'Une erreur est survenue : ' + (error.response?.data?.message || error.message) 
      });
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
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

  const isEtudiant = role === 'etudiant';
  const displayRole = isEtudiant ? "ÉTUDIANT" : "ENSEIGNANT";
  const displayFullName = isEtudiant ? profile.nomComplet : (profile.nomComplet || `${profile.prenom} ${profile.nom}`);

  return (
    <div className="p-8 pb-20 max-w-7xl mx-auto animate-in fade-in duration-500">
      <div className="flex flex-col lg:flex-row gap-8">
        {/* Left Column: Informations Personnelles */}
        <div className="flex-[2] bg-white p-10 rounded-[40px] shadow-sm border border-slate-50 relative overflow-hidden">
          
          {/* Status Messages */}
          {message && (
            <div className={`mb-8 p-4 rounded-2xl flex items-center gap-3 animate-in slide-in-from-top-4 duration-300 ${
              message.type === 'success' ? 'bg-green-50 text-green-700 border border-green-100' : 'bg-red-50 text-red-700 border border-red-100'
            }`}>
              {message.type === 'success' ? <CheckCircle2 className="w-5 h-5" /> : <AlertCircle className="w-5 h-5" />}
              <span className="font-bold text-sm">{message.text}</span>
            </div>
          )}

          <div className="flex justify-between items-start mb-10">
            <div>
              <h2 className="text-4xl font-black text-slate-900 tracking-tight leading-none mb-4">Informations Personnelles</h2>
              <p className="text-slate-500 font-medium">Mettez à jour vos informations de profil pour l'institution.</p>
            </div>
            <div className="flex gap-3">
              <button 
                onClick={() => window.history.back()}
                className="flex items-center gap-2 px-6 py-2.5 bg-blue-50 text-blue-600 rounded-xl font-bold text-sm hover:bg-blue-100 transition-colors"
              >
                <Undo2 className="w-4 h-4" />
                Retour
              </button>
              <button 
                onClick={handleSave}
                disabled={saving}
                className="flex items-center gap-2 px-6 py-2.5 bg-[#5b8c5a] text-white rounded-xl font-bold text-sm hover:bg-[#4a7249] transition-all shadow-lg shadow-[#5b8c5a]/20 disabled:opacity-50 disabled:cursor-not-allowed min-w-[140px] justify-center"
              >
                {saving ? (
                  <Loader2 className="w-4 h-4 animate-spin" />
                ) : (
                  <Save className="w-4 h-4" />
                )}
                {saving ? 'Enregistrement...' : 'Enregistrer'}
              </button>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-10">
            <ProfileField 
              label="PRÉNOM" 
              name="prenom"
              value={formData.prenom} 
              icon={User} 
              onChange={handleInputChange}
            />
            <ProfileField 
              label="NOM" 
              name="nom"
              value={formData.nom} 
              icon={User} 
              onChange={handleInputChange}
            />
            <ProfileField 
              label="EMAIL ACADÉMIQUE" 
              name="email"
              value={formData.email} 
              icon={Mail} 
              onChange={handleInputChange}
              readOnly={true} // Souvent non modifiable pour des raisons de sécurité
            />
            
            {isEtudiant ? (
              <>
                <ProfileField 
                  label="CODE APOGÉE" 
                  name="codeApogee"
                  value={formData.codeApogee} 
                  icon={Hash} 
                  onChange={handleInputChange}
                />
                <ProfileField 
                  label="NIVEAU" 
                  name="niveau"
                  value={formData.niveau} 
                  icon={GraduationCap} 
                  onChange={handleInputChange}
                />
                <ProfileField 
                  label="FILIÈRE" 
                  name="filiere"
                  value={formData.filiere} 
                  icon={BookOpen} 
                  onChange={handleInputChange}
                />
                <ProfileField 
                  label="STATUT COMPTE" 
                  name="statut"
                  value={profile.statut} 
                  icon={BadgeCheck} 
                  readOnly={true}
                />
              </>
            ) : (
              <>
                <ProfileField 
                  label="DÉPARTEMENT" 
                  name="departement"
                  value={formData.departement} 
                  icon={Building} 
                  onChange={handleInputChange}
                />
                <ProfileField 
                  label="TITRE" 
                  name="titre"
                  value={formData.titre} 
                  icon={Briefcase} 
                  onChange={handleInputChange}
                />
                <ProfileField 
                  label="SPÉCIALITÉ" 
                  name="specialite"
                  value={formData.specialite} 
                  icon={BookOpen} 
                  onChange={handleInputChange}
                />
                <ProfileField 
                  label="N° DE TÉLÉPHONE" 
                  name="telephone"
                  value="0600000000" 
                  icon={Phone} 
                  readOnly={true}
                />
              </>
            )}
          </div>

          {/* Verification Banner */}
          <div className="flex items-center gap-6 p-6 bg-[#f0f7ff] rounded-3xl border border-blue-100">
            <div className="w-12 h-12 bg-white rounded-2xl flex items-center justify-center text-blue-600 shadow-sm border border-blue-50">
              <ShieldCheck className="w-6 h-6" />
            </div>
            <div>
              <h4 className="font-black text-slate-900 leading-tight">Vérification de Gouvernance</h4>
              <p className="text-sm text-slate-500 font-medium">Votre profil est conforme aux normes académiques en vigueur.</p>
            </div>
          </div>
        </div>

        {/* Right Column: Profile Summary */}
        <div className="flex-1 flex flex-col gap-8">
          {/* Main Profile Card */}
          <div className="bg-white rounded-[40px] shadow-sm border border-slate-50 overflow-hidden text-center flex flex-col">
            <div className="h-28 bg-gradient-to-r from-blue-600 via-indigo-500 to-purple-600"></div>
            <div className="px-8 pb-10 -mt-14 flex-1">
              <div className="relative inline-block mb-6">
                <div className="w-28 h-28 bg-white p-2 rounded-full shadow-xl">
                  <div className="w-full h-full bg-slate-100 rounded-full flex items-center justify-center text-slate-300 overflow-hidden relative">
                    <User className="w-14 h-14" />
                    {/* Active Status Dot */}
                    <div className="absolute bottom-1 right-1 w-6 h-6 bg-[#22c55e] border-4 border-white rounded-full shadow-sm"></div>
                  </div>
                </div>
              </div>

              <h3 className="text-3xl font-black text-slate-900 mb-1 leading-none tracking-tight">
                {displayFullName}
              </h3>
              <p className="text-[12px] font-black text-blue-600 uppercase tracking-widest mb-8">
                {displayRole}
              </p>

              <div className="space-y-4">
                <div className="flex items-center gap-4 p-4 bg-slate-50 rounded-2xl text-left border border-slate-100/50">
                  <div className="w-10 h-10 bg-white rounded-xl flex items-center justify-center text-slate-400 shadow-sm border border-slate-50">
                    <Mail className="w-5 h-5" />
                  </div>
                  <div className="flex flex-col">
                    <span className="text-[9px] font-black text-slate-400 uppercase tracking-widest leading-none mb-1">EMAIL ACADÉMIQUE</span>
                    <span className="text-sm font-bold text-slate-700 leading-none">{profile.email}</span>
                  </div>
                </div>

                <div className="flex items-center justify-between p-4 bg-slate-50 rounded-2xl text-left border border-slate-100/50">
                  <div className="flex items-center gap-4">
                    <div className="w-10 h-10 bg-white rounded-xl flex items-center justify-center text-slate-400 shadow-sm border border-slate-50">
                      <BadgeCheck className="w-5 h-5" />
                    </div>
                    <div className="flex flex-col">
                      <span className="text-[9px] font-black text-slate-400 uppercase tracking-widest leading-none mb-1">STATUT</span>
                      <span className="text-sm font-bold text-slate-700 leading-none">{profile.statut || "Actif"}</span>
                    </div>
                  </div>
                  <span className="px-3 py-1 bg-[#ccf2d1] text-[#2c7a36] text-[10px] font-black rounded-lg uppercase tracking-tight">
                    CONFORME
                  </span>
                </div>
              </div>
            </div>
          </div>

          {/* Academic Activity Card */}
          <div className="bg-white p-8 rounded-[40px] shadow-sm border border-slate-50">
            <h3 className="text-[12px] font-black text-slate-700 uppercase tracking-widest mb-10 px-2 text-center lg:text-left">ACTIVITÉ ACADÉMIQUE</h3>
            <div className="grid grid-cols-2 gap-4">
              <div className="bg-slate-50 p-6 rounded-[32px] text-center border border-slate-100/50 group hover:scale-[1.02] transition-transform cursor-pointer">
                <span className="block text-3xl font-black text-blue-600 leading-none mb-3">
                  {isEtudiant ? "-" : (profile.nbProjets || "12")}
                </span>
                <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest">PROJETS</span>
              </div>
              <div className="bg-slate-50 p-6 rounded-[32px] text-center border border-slate-100/50 group hover:scale-[1.02] transition-transform cursor-pointer">
                <span className="block text-3xl font-black text-[#60a561] leading-none mb-3">
                  {isEtudiant ? "-" : (profile.nbEvaluations || "156")}
                </span>
                <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest">
                  {isEtudiant ? "COURS" : "ÉVALUATIONS"}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
