import React from 'react';
import {
  UserCircle,
  ChevronDown,
  Camera,
  GraduationCap
} from 'lucide-react';

const ProfilePage: React.FC = () => {
  return (
    <div className="relative min-h-full">
      {/* Blurred background header */}
      <div
        className="absolute top-0 left-0 right-0 h-64 bg-gradient-to-br from-blue-100 via-indigo-50 to-purple-100 z-0 opacity-60"
      ></div>

      <div className="relative z-10 p-8 flex flex-col lg:flex-row gap-8 mt-4 max-w-5xl">
        {/* Form Card */}
        <div className="flex-[2] bg-white rounded-3xl shadow-custom p-10">
          <h2 className="text-3xl font-bold text-gray-900 mb-8 border-b-4 border-black w-fit pb-1">Profil</h2>

          <form className="grid grid-cols-1 md:grid-cols-2 gap-x-8 gap-y-6">
            <FormField label="NOM" placeholder="ALAMI" defaultValue="ALAMI" />
            <FormField label="PRÉNOM" placeholder="Mohammed" defaultValue="Mohammed" />
            <FormField label="N° DE TÉLÉPHONE" placeholder="00000000" defaultValue="00000000" />

            <div className="space-y-2">
              <label className="block text-xs font-bold text-gray-400 tracking-wider">LANGUE LOCALE</label>
              <div className="relative">
                <select className="w-full bg-gray-50 border-none rounded-xl px-4 py-4 text-gray-800 font-medium appearance-none focus:ring-2 focus:ring-indigo-100 transition-all cursor-pointer">
                  <option>Français</option>
                  <option>English</option>
                  <option>العربية</option>
                </select>
                <ChevronDown className="absolute right-4 top-1/2 -translate-y-1/2 text-gray-400 pointer-events-none" size={20} />
              </div>
            </div>

            <FormField label="FILIÈRE" placeholder="Informatique" defaultValue="Informatique" />
            <FormField label="NIVEAU" placeholder="Master 2" defaultValue="Master 2" />

            <div className="md:col-span-2 flex justify-between items-center mt-8">
              <button
                type="button"
                className="bg-emerald-600 hover:bg-emerald-700 text-white px-8 py-3 rounded-xl font-bold shadow-lg shadow-emerald-200 transition-all transform active:scale-95"
              >
                Enregistrer
              </button>
              <button
                type="button"
                className="bg-blue-600 hover:bg-blue-700 text-white px-8 py-3 rounded-xl font-bold shadow-lg shadow-blue-200 transition-all transform active:scale-95"
              >
                Retour
              </button>
            </div>
          </form>
        </div>

        {/* Right Column */}
        <div className="flex-1 space-y-6">
          {/* User Profile Card */}
          <div className="bg-white rounded-3xl shadow-custom p-8 flex flex-col items-center text-center">
            <div className="relative mb-6">
              <div className="w-32 h-32 rounded-full border-4 border-gray-100 bg-gray-50 flex items-center justify-center overflow-hidden">
                <div className="w-full h-full flex items-center justify-center bg-gray-200">
                  <UserCircle className="text-gray-400" size={80} strokeWidth={1} />
                </div>
              </div>
              <button className="absolute bottom-1 right-1 bg-emerald-500 p-2 rounded-full text-white shadow-lg border-2 border-white">
                <Camera size={14} />
              </button>
            </div>

            <h3 className="text-xl font-bold text-gray-800 mb-1">ALAMI Mohammed</h3>
            <p className="text-sm text-gray-400 mb-8">alami.mohammed@etu.uae.ac.ma</p>

            <div className="w-full space-y-4">
              <div className="flex justify-between items-center px-2">
                <span className="text-xs font-bold text-gray-400 tracking-wider">STATUT</span>
                <span className="bg-green-100 text-green-600 text-[10px] font-bold px-3 py-1 rounded-full uppercase tracking-widest">Actif</span>
              </div>
              <div className="flex justify-between items-center px-2">
                <span className="text-xs font-bold text-gray-400 tracking-wider">RÔLE</span>
                <span className="text-sm font-bold text-gray-800">Étudiant</span>
              </div>
            </div>
          </div>

          {/* Help Card */}
          <div className="bg-[#001e3c] rounded-3xl p-8 text-white relative overflow-hidden shadow-custom">
            <div className="relative z-10">
              <h4 className="text-lg font-bold mb-3">Aide Académique</h4>
              <p className="text-xs text-blue-100 leading-relaxed opacity-80">
                Besoin d'aide pour configurer votre profil ? Contactez le support technique de l'UAE.
              </p>
            </div>
            <div className="absolute -bottom-4 -right-4 opacity-10">
              <GraduationCap size={120} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

interface FormFieldProps {
  label: string;
  placeholder: string;
  defaultValue?: string;
  type?: string;
}

const FormField: React.FC<FormFieldProps> = ({ label, placeholder, defaultValue, type = "text" }) => (
  <div className="space-y-2">
    <label className="block text-xs font-bold text-gray-400 tracking-wider">{label}</label>
    <input
      type={type}
      defaultValue={defaultValue}
      placeholder={placeholder}
      className="w-full bg-gray-50 border-none rounded-xl px-4 py-4 text-gray-800 font-medium focus:ring-2 focus:ring-indigo-100 transition-all placeholder:text-gray-300"
    />
  </div>
);

export default ProfilePage;
