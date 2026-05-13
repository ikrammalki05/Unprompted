import React, { useState, useEffect } from 'react';
import {
  FileUp,
  Infinity as InfinityIcon,
  Hourglass,
  ShieldAlert,
  Filter,
  Download,
  CheckCircle2
} from 'lucide-react';

import iaService from '../services/iaService';
import userService from '../services/userService';
import projetService from '../services/projetService';

interface AccessCardProps {
  id: string;
  icon: React.ElementType;
  title: string;
  description: string;
  active: boolean;
  onClick: () => void;
}

const AccessCard: React.FC<AccessCardProps> = ({
  icon: Icon,
  title,
  description,
  active,
  onClick
}) => (
  <div
    onClick={onClick}
    className={`p-6 rounded-2xl border-2 cursor-pointer transition-all duration-300 relative group flex flex-col h-full bg-white ${
      active
        ? 'border-blue-500 shadow-lg shadow-blue-50'
        : 'border-slate-100 hover:border-slate-200 hover:shadow-md'
    }`}
  >
    {active && (
      <div className="absolute top-4 right-4">
        <CheckCircle2
          className="w-6 h-6 text-blue-500"
          fill="currentColor"
          fillOpacity={0.1}
        />
      </div>
    )}

    <div
      className={`w-12 h-12 rounded-xl flex items-center justify-center mb-6 ${
        active
          ? 'bg-blue-100 text-blue-600'
          : 'bg-slate-50 text-slate-400 group-hover:bg-slate-100'
      }`}
    >
      <Icon className="w-6 h-6" />
    </div>

    <h3 className="text-xl font-bold text-slate-800 mb-3">
      {title}
    </h3>

    <p className="text-sm text-slate-500 leading-relaxed line-clamp-3">
      {description}
    </p>
  </div>
);

const ConfigurationIA: React.FC = () => {
  const [projects, setProjects] = useState<any[]>([]);
  const [selectedProjetId, setSelectedProjetId] = useState<number | string>('');
  const [selectedAccess, setSelectedAccess] = useState('unlimited');
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        setLoading(true);
        const userProfile = await userService.getCurrentUser();
        const data = await projetService.getByEnseignant(userProfile.id);
        setProjects(data);
        if (data.length > 0 && data[0].id) {
            setSelectedProjetId(data[0].id.toString());
        }
      } catch (error) {
        console.error(error);
      } finally {
        setLoading(false);
      }
    };
    fetchProjects();
  }, []);

  const handleFileChange = async (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {
    const file = e.target.files?.[0];

    if (!file) return;

    setSelectedFile(file);

    try {
      await iaService.uploadCahierCharge(file);
      alert('PDF uploadé avec succès');
    } catch (error: any) {
      console.error('Erreur upload :', error);
      const errorMsg = error.response?.data?.message || error.response?.data?.details || "Erreur lors de l'envoi du fichier";
      alert(`Erreur upload PDF : ${errorMsg}`);
    }
  };

  const saveConfiguration = async () => {
    if (!selectedProjetId) {
        alert("Veuillez sélectionner un projet.");
        return;
    }

    try {
      const projectId = Number(selectedProjetId);
      if (isNaN(projectId) || projectId <= 0) {
        alert("ID de projet invalide. Veuillez resélectionner le projet.");
        return;
      }

      const data = {
        idProjet: projectId,
        generationCodeAutorisee: selectedAccess !== 'restricted',
        quotaRequetes: selectedAccess === 'limited' ? 50 : 1000,
        periodeQuota: 'Projet'
      };

      console.log('Configuration envoyée :', data);

      await iaService.saveConfiguration(data);

      alert('Configuration sauvegardée avec succès !');
    } catch (error: any) {
      console.error('Erreur sauvegarde :', error);
      const errorMsg = error.response?.data?.message || error.response?.data?.title || "Erreur inconnue";
      const details = error.response?.data?.errors ? JSON.stringify(error.response.data.errors) : "";
      alert(`Erreur sauvegarde configuration : ${errorMsg} ${details}`);
    }
  };

  if (projects.length === 0 && !loading) {
     // Optionnel : on peut afficher un message si aucun projet n'existe
  }

  return (
    <div className="p-8 space-y-12 max-w-7xl mx-auto">
      {/* Project Selection Section */}
      <section>
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-slate-800 tracking-tight">
            Sélectionner le projet
          </h2>

          <p className="text-slate-500 mt-1">
            Choisissez le projet académique pour lequel vous souhaitez configurer les accès IA.
          </p>

          <div className="mt-4">
            <select
              value={selectedProjetId}
              onChange={(e) => {
                const val = e.target.value;
                console.log("Projet sélectionné (ID brut) :", val);
                setSelectedProjetId(val);
              }}
              className="w-full max-w-md px-4 py-2.5 bg-white border border-slate-200 rounded-xl font-medium focus:ring-2 focus:ring-blue-100 outline-none"
            >
              <option value="">-- Choisir un projet --</option>
              {projects.map((p) => (
                <option key={p.id || Math.random()} value={p.id?.toString() || ""}>
                  {p.titre || "Sans titre"}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="bg-slate-50/50 border-2 border-dashed border-slate-200 rounded-3xl p-12 text-center transition-colors hover:border-blue-300 group">
          <div className="w-16 h-16 bg-blue-50 text-blue-600 rounded-2xl flex items-center justify-center mx-auto mb-4 group-hover:scale-110 transition-transform">
            <FileUp className="w-8 h-8" />
          </div>

          <h3 className="text-lg font-bold text-slate-800 mb-4 tracking-tight">
            Importer le cahier des charges (PDF)
          </h3>

          <input
            type="file"
            accept="application/pdf"
            onChange={handleFileChange}
            className="hidden"
            id="pdfUpload"
          />

          <label
            htmlFor="pdfUpload"
            className="px-8 py-2.5 bg-slate-200 text-slate-700 rounded-xl font-bold hover:bg-slate-300 transition-colors shadow-sm cursor-pointer"
          >
            Choisir un fichier
          </label>

          {selectedFile && (
            <p className="mt-4 text-sm text-slate-500">
              {selectedFile.name}
            </p>
          )}
        </div>
      </section>

      {/* Access Levels Section */}
      <section>
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-slate-800 tracking-tight">
            Niveaux d'accès IA
          </h2>

          <p className="text-slate-500 mt-1">
            Définissez les privilèges d'utilisation de l'intelligence artificielle.
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <AccessCard
            id="unlimited"
            icon={InfinityIcon}
            title="Accès illimité"
            description="Utilisation libre."
            active={selectedAccess === 'unlimited'}
            onClick={() => setSelectedAccess('unlimited')}
          />

          <AccessCard
            id="limited"
            icon={Hourglass}
            title="Accès limité"
            description="Quota limité."
            active={selectedAccess === 'limited'}
            onClick={() => setSelectedAccess('limited')}
          />

          <AccessCard
            id="restricted"
            icon={ShieldAlert}
            title="Accès restreint"
            description="Fonctionnalités filtrées."
            active={selectedAccess === 'restricted'}
            onClick={() => setSelectedAccess('restricted')}
          />
        </div>

        <div className="mt-8">
          <button
            onClick={saveConfiguration}
            className="px-6 py-3 bg-blue-600 text-white rounded-xl font-bold hover:bg-blue-700"
          >
            Sauvegarder la configuration
          </button>
        </div>
      </section>

      {/* Pedagogical Analysis Section */}
      <section>
        <div className="flex flex-col md:flex-row md:items-end justify-between gap-4 mb-8">
          <div>
            <h2 className="text-2xl font-bold text-slate-800 tracking-tight">
              Analyse pédagogique
            </h2>

            <p className="text-slate-500 mt-1">
              Surveillance en temps réel.
            </p>
          </div>

          <div className="flex gap-3">
            <button className="flex items-center gap-2 px-5 py-2.5 bg-slate-100 text-slate-600 rounded-xl font-bold hover:bg-slate-200 transition-all border border-slate-200/50">
              <Filter className="w-4 h-4" />
              <span>Filtrer</span>
            </button>

            <button className="flex items-center gap-2 px-5 py-2.5 bg-slate-100 text-slate-600 rounded-xl font-bold hover:bg-slate-200 transition-all border border-slate-200/50">
              <Download className="w-4 h-4" />
              <span>Exporter CSV</span>
            </button>
          </div>
        </div>

        <div className="bg-white rounded-3xl border border-slate-100 overflow-hidden shadow-sm">
          <div className="overflow-x-auto">
            <table className="w-full text-left">
              <thead>
                <tr className="bg-slate-50/50 border-b border-slate-100">
                  <th className="px-6 py-4">Étudiant</th>
                  <th className="px-6 py-4">Projet</th>
                  <th className="px-6 py-4">Commits</th>
                  <th className="px-6 py-4">Git</th>
                  <th className="px-6 py-4">Prompts</th>
                  <th className="px-6 py-4">Tokens</th>
                  <th className="px-6 py-4">Dépendance</th>
                </tr>
              </thead>

              <tbody>
                <tr>
                  <td
                    className="px-6 py-5 col-span-7 text-center text-slate-400"
                    colSpan={7}
                  >
                    Aucune donnée disponible
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </section>
    </div>
  );
};

export default ConfigurationIA;