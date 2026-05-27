import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import {
  FileText, CheckCircle2, Clock, CalendarDays,
  Users, Download, Target, Layers, Award,
} from 'lucide-react';
import { api } from '../../services/api';

interface Membre {
  idEtudiant: number;
  nomComplet: string;
  role: string;
}

interface ProjetDetail {
  idProjet: number;
  titre: string;
  description: string;
  objectifs: string[];
  livrables: string[];
  criteresEvaluation: string[];
  technologiesRequises: string[];
  contraintes: string[];
  ressourcesDisponibles: string[];
  dateDebut: string;
  dateFin: string;
  progression: number;
  notesEnseignant: string | null;
  hasCahierDesCharges: boolean;
  nomEnseignant: string;
}

const MOCK_DETAILS: Record<number, ProjetDetail> = {
  1: {
    idProjet: 1,
    titre: "Gouvernance IA dans le secteur public",
    description: "Ce projet vise à analyser les cadres réglementaires et éthiques entourant le déploiement de l'intelligence artificielle dans les institutions publiques, afin de proposer des recommandations adaptées au contexte académique et gouvernemental.",
    objectifs: [
      "Cartographie des réglementations IA en vigueur (EU AI Act, RGPD).",
      "Développement d'un outil d'audit automatisé pour la conformité.",
      "Mise en place d'indicateurs de transparence algorithmique."
    ],
    livrables: ["Rapport de cartographie", "Prototype d'outil d'audit", "Documentation technique"],
    criteresEvaluation: ["Qualité de l'analyse", "Fonctionnalité de l'outil"],
    technologiesRequises: ["React", "FastAPI", "PostgreSQL"],
    contraintes: ["Respect des délais", "Données anonymisées"],
    ressourcesDisponibles: ["Accès documentation EU AI Act"],
    dateDebut: "2025-05-05",
    dateFin: "2025-06-25",
    progression: 35,
    notesEnseignant: "Bon démarrage.",
    hasCahierDesCharges: false,
    nomEnseignant: "Prof. Amrani"
  },
  2: {
    idProjet: 2,
    titre: "Optimisation des modèles de langage",
    description: "Étude comparative des techniques de quantification 4-bit pour l'inférence locale.",
    objectifs: ["Réduction de l'empreinte mémoire", "Optimisation de la latence"],
    livrables: ["API d'inférence", "Benchmarks"],
    criteresEvaluation: ["Performance vs baseline"],
    technologiesRequises: ["Python", "PyTorch", "Docker"],
    contraintes: ["GPU limité"],
    ressourcesDisponibles: ["Cluster GPU"],
    dateDebut: "2025-05-12",
    dateFin: "2025-06-30",
    progression: 0,
    notesEnseignant: null,
    hasCahierDesCharges: false,
    nomEnseignant: "Prof. Amrani"
  },
  3: {
    idProjet: 3,
    titre: "Sécurisation des APIs Locales",
    description: "Protocoles de sécurité pour l'inférence sur site.",
    objectifs: ["Authentification mTLS", "Détection d'intrusion"],
    livrables: ["Module mTLS", "Dashboard monitoring"],
    criteresEvaluation: ["Robustesse de l'auth"],
    technologiesRequises: ["Go", "Nginx", "Prometheus"],
    contraintes: ["Pas de dépendance externe"],
    ressourcesDisponibles: ["Certificats test"],
    dateDebut: "2025-06-01",
    dateFin: "2025-07-15",
    progression: 0,
    notesEnseignant: null,
    hasCahierDesCharges: false,
    nomEnseignant: "Prof. Amrani"
  }
};

const CahierDesChargesPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [project, setProject] = useState<ProjetDetail | null>(null);
  const [membres, setMembres] = useState<Membre[]>([]);
  const [loading, setLoading] = useState(true);
  const [downloading, setDownloading] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        // 1. Charger les détails du projet
        const resProj = await api.get<ProjetDetail>(`/projet/${id}`);
        setProject(resProj.data);

        // 2. Récupérer mon ID pour charger mes collègues
        try {
          const resMe = await api.get<{ idEtudiant: number }>('/profil/me');
          const myId = resMe.data.idEtudiant;

          // 3. Charger les collègues du groupe
          const resMembres = await api.get<Membre[]>(`/projet/${id}/etudiant/${myId}/collegues`);
          setMembres(resMembres.data);
        } catch (meErr) {
          console.warn("Impossible de charger les collègues (mode démo ou erreur profil).");
        }

      } catch (err) {
        console.error("Erreur lors du chargement des données réelles, basculement sur démo:", err);
        const mockId = parseInt(id || "0");
        if (MOCK_DETAILS[mockId]) {
          setProject(MOCK_DETAILS[mockId]);
        }
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [id]);

  const handleDownload = async () => {
    if (!project) return;
    setDownloading(true);
    try {
      const response = await api.get(`/projet/${id}/download-cahier`, { responseType: 'blob' });
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `Cahier_des_Charges_${project.titre}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();
    } catch (err) {
      console.error("Erreur téléchargement:", err);
      alert("Le fichier PDF n'est pas disponible pour ce projet.");
    } finally {
      setDownloading(false);
    }
  };

  if (loading) return <div className="p-20 text-center text-gray-400">Chargement du cahier des charges...</div>;
  if (!project) return <div className="p-20 text-center">Projet introuvable.</div>;

  return (
    <div className="p-8 max-w-6xl mx-auto animate-fadeIn">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 mb-10">
        <div>
          <div className="flex items-center gap-2 text-xs font-bold text-blue-600 uppercase tracking-widest mb-3">
            <FileText size={14} />
            Cahier des Charges Officiel
          </div>
          <h1 className="text-3xl font-bold text-gray-900 tracking-tight">{project.titre}</h1>
        </div>
        <button
          onClick={handleDownload}
          className={`flex items-center gap-2 px-6 py-3 rounded-xl font-bold transition-all duration-300 shadow-sm
            ${project.hasCahierDesCharges 
              ? 'bg-blue-600 text-white hover:bg-blue-700 hover:shadow-blue-200 active:scale-95' 
              : 'bg-gray-100 text-gray-400 cursor-not-allowed'}`}
        >
          <Download size={18} className={downloading ? 'animate-bounce' : ''} />
          {downloading ? 'Téléchargement...' : 'Télécharger PDF'}
        </button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div className="lg:col-span-2 space-y-8">
          {/* Présentation */}
          <section className="bg-white p-8 rounded-3xl border border-gray-100 shadow-sm">
            <h2 className="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
              <Target className="text-blue-500" size={20} /> Présentation du Projet
            </h2>
            <p className="text-gray-600 leading-relaxed text-lg">{project.description}</p>
          </section>

          {/* Objectifs & Livrables */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <section className="bg-white p-8 rounded-3xl border border-gray-100 shadow-sm">
              <h3 className="text-lg font-bold text-gray-900 mb-5 flex items-center gap-2">
                <CheckCircle2 className="text-emerald-500" size={18} /> Objectifs
              </h3>
              <ul className="space-y-4">
                {project.objectifs?.map((obj, i) => (
                  <li key={i} className="flex gap-3 text-sm text-gray-600">
                    <span className="w-5 h-5 rounded-full bg-emerald-50 text-emerald-600 flex items-center justify-center text-[10px] font-bold flex-shrink-0 mt-0.5">{i + 1}</span>
                    {obj}
                  </li>
                ))}
              </ul>
            </section>

            <section className="bg-white p-8 rounded-3xl border border-gray-100 shadow-sm">
              <h3 className="text-lg font-bold text-gray-900 mb-5 flex items-center gap-2">
                <Layers className="text-purple-500" size={18} /> Livrables attendus
              </h3>
              <div className="space-y-3">
                {project.livrables?.map((liv, i) => (
                  <div key={i} className="px-4 py-3 bg-gray-50 rounded-xl text-sm font-medium text-gray-700 border border-gray-100">
                    {liv}
                  </div>
                ))}
              </div>
            </section>
          </div>

          {/* Équipe */}
          <section className="bg-white p-8 rounded-3xl border border-gray-100 shadow-sm">
            <h2 className="text-xl font-bold text-gray-900 mb-6 flex items-center gap-2">
              <Users className="text-blue-500" size={20} /> Membres du Groupe
            </h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              {membres.length > 0 ? membres.map((membre) => (
                <div key={membre.idEtudiant} className="flex items-center gap-3 p-4 bg-gray-50 rounded-2xl border border-gray-100">
                  <div className="w-10 h-10 rounded-full bg-blue-100 text-blue-600 flex items-center justify-center font-bold">
                    {membre.nomComplet.charAt(0)}
                  </div>
                  <div>
                    <p className="text-sm font-bold text-gray-900">{membre.nomComplet}</p>
                    <p className="text-xs text-blue-600 font-medium">{membre.role}</p>
                  </div>
                </div>
              )) : (
                <p className="text-sm text-gray-400 italic col-span-2 text-center py-4">Aucun collègue assigné ou vous êtes seul.</p>
              )}
            </div>
          </section>
        </div>

        {/* Sidebar Status */}
        <div className="space-y-6">
          <div className="bg-gray-900 text-white p-8 rounded-3xl shadow-xl">
            <h3 className="text-lg font-bold mb-6 flex items-center gap-2">
              <Clock className="text-blue-400" size={20} /> État du Projet
            </h3>
            <div className="space-y-6">
              <div>
                <div className="flex justify-between text-sm mb-2 font-medium">
                  <span className="text-gray-400">Progression</span>
                  <span className="text-blue-400">{project.progression}%</span>
                </div>
                <div className="w-full bg-white/10 h-2 rounded-full overflow-hidden">
                  <div className="bg-blue-500 h-full transition-all duration-1000" style={{ width: `${project.progression}%` }} />
                </div>
              </div>

              <div className="pt-4 border-t border-white/10 space-y-4">
                <div className="flex items-center gap-3">
                  <CalendarDays size={18} className="text-gray-500" />
                  <div>
                    <p className="text-[10px] text-gray-500 uppercase font-bold tracking-tighter">Fin estimée</p>
                    <p className="text-sm font-semibold">{new Date(project.dateFin).toLocaleDateString()}</p>
                  </div>
                </div>
                <div className="flex items-center gap-3">
                   <Award size={18} className="text-gray-500" />
                  <div>
                    <p className="text-[10px] text-gray-500 uppercase font-bold tracking-tighter">Encadrant</p>
                    <p className="text-sm font-semibold">{project.nomEnseignant}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CahierDesChargesPage;
