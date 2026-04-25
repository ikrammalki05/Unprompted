import React from 'react';
import { 
  User, 
  Phone, 
  Globe, 
  Building, 
  Briefcase, 
  Undo2, 
  Save, 
  ShieldCheck, 
  Mail, 
  BadgeCheck 
} from 'lucide-react';

const ProfileField: React.FC<{ label: string; value: string; icon: React.ElementType; placeholder?: string }> = ({ label, value, icon: Icon, placeholder }) => (
  <div className="space-y-2">
    <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest">{label}</label>
    <div className="relative group">
      <div className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-300 group-focus-within:text-blue-500 transition-colors">
        <Icon className="w-4 h-4" />
      </div>
      <input 
        type="text" 
        defaultValue={value}
        placeholder={placeholder}
        className="w-full pl-11 pr-4 py-3.5 bg-slate-50 border border-slate-100 rounded-2xl text-slate-900 font-bold focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all placeholder:text-slate-400"
      />
    </div>
  </div>
);

const Profile: React.FC = () => {
  return (
    <div className="p-8 pb-20 max-w-7xl mx-auto animate-in fade-in duration-500">
      <div className="flex flex-col lg:flex-row gap-8">
        {/* Left Column: Informations Personnelles */}
        <div className="flex-[2] bg-white p-10 rounded-[40px] shadow-sm border border-slate-50 relative overflow-hidden">
          <div className="flex justify-between items-start mb-10">
            <div>
              <h2 className="text-4xl font-black text-slate-900 tracking-tight leading-none mb-4">Informations Personnelles</h2>
              <p className="text-slate-500 font-medium">Mettez à jour vos informations de profil pour l'institution.</p>
            </div>
            <div className="flex gap-3">
              <button className="flex items-center gap-2 px-6 py-2.5 bg-blue-50 text-blue-600 rounded-xl font-bold text-sm hover:bg-blue-100 transition-colors">
                <Undo2 className="w-4 h-4" />
                Retour
              </button>
              <button className="flex items-center gap-2 px-6 py-2.5 bg-[#5b8c5a] text-white rounded-xl font-bold text-sm hover:bg-[#4a7249] transition-all shadow-lg shadow-[#5b8c5a]/20">
                <Save className="w-4 h-4" />
                Enregistrer
              </button>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-10">
            <ProfileField label="NOM" value="Ghailani" icon={User} />
            <ProfileField label="PRÉNOM" value="" placeholder="Entrez votre prénom" icon={User} />
            <ProfileField label="N° DE TÉLÉPHONE" value="00000000" icon={Phone} />
            
            <div className="space-y-2">
              <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest">LANGUE LOCALE</label>
              <div className="relative group">
                <div className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-300 group-focus-within:text-blue-500 transition-colors">
                  <Globe className="w-4 h-4" />
                </div>
                <select className="w-full pl-11 pr-4 py-3.5 bg-slate-50 border border-slate-100 rounded-2xl text-slate-900 font-bold focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all appearance-none">
                  <option>Français</option>
                  <option>English</option>
                  <option>العربية</option>
                </select>
                <div className="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none">
                  <Undo2 className="w-4 h-4 rotate-[-90deg] text-slate-300" />
                </div>
              </div>
            </div>

            <ProfileField label="DÉPARTEMENT" value="Informatique" icon={Building} />
            <ProfileField label="TITRE" value="Professeur" icon={Briefcase} />
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

              <h3 className="text-3xl font-black text-slate-900 mb-1 leading-none tracking-tight">Mr Ghailani</h3>
              <p className="text-[12px] font-black text-blue-600 uppercase tracking-widest mb-8">ENSEIGNANT</p>

              <div className="space-y-4">
                <div className="flex items-center gap-4 p-4 bg-slate-50 rounded-2xl text-left border border-slate-100/50">
                  <div className="w-10 h-10 bg-white rounded-xl flex items-center justify-center text-slate-400 shadow-sm border border-slate-50">
                    <Mail className="w-5 h-5" />
                  </div>
                  <div className="flex flex-col">
                    <span className="text-[9px] font-black text-slate-400 uppercase tracking-widest leading-none mb-1">EMAIL ACADÉMIQUE</span>
                    <span className="text-sm font-bold text-slate-700 leading-none">ghailani@univ.ac.ma</span>
                  </div>
                </div>

                <div className="flex items-center justify-between p-4 bg-slate-50 rounded-2xl text-left border border-slate-100/50">
                  <div className="flex items-center gap-4">
                    <div className="w-10 h-10 bg-white rounded-xl flex items-center justify-center text-slate-400 shadow-sm border border-slate-50">
                      <BadgeCheck className="w-5 h-5" />
                    </div>
                    <div className="flex flex-col">
                      <span className="text-[9px] font-black text-slate-400 uppercase tracking-widest leading-none mb-1">STATUT</span>
                      <span className="text-sm font-bold text-slate-700 leading-none">Actif</span>
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
                <span className="block text-3xl font-black text-blue-600 leading-none mb-3">12</span>
                <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest">PROJETS</span>
              </div>
              <div className="bg-slate-50 p-6 rounded-[32px] text-center border border-slate-100/50 group hover:scale-[1.02] transition-transform cursor-pointer">
                <span className="block text-3xl font-black text-[#60a561] leading-none mb-3">156</span>
                <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest">ÉVALUATIONS</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
