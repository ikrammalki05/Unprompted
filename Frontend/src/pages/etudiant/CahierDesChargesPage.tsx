import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  ChevronRight,
  FileText,
  Circle,
  Github,
  BookMarked,
  CheckCircle2,
  Clock,
  CalendarDays,
} from 'lucide-react';

// ── Types ──────────────────────────────────────────────────────────────────────
interface Member {
  name: string;
  role: 'Frontend' | 'Backend' | 'DevOps' | 'IA';
}

interface ProjectDetail {
  id: number;
  title: string;
  badge: string;
  promo: string;
  status: 'active' | 'pending' | 'upcoming';
  statusLabel: string;
  description: string;
  objectifsTechniques: string[];
  objectifsFonctionnels: string[];
  members: Member[];
  dateDebut: { day: string; month: string };
  dateFin: { day: string; month: string };
  soutenance: string;
  resources: { label: string; type: 'github' | 'doc' }[];
}

// ── Static data ────────────────────────────────────────────────────────────────
const projectsData: Record<number, ProjectDetail> = {
  1: {
    id: 1,
    title: 'Gouvernance IA dans le secteur public',
    badge: 'RESPONSABLE RECHERCHE',
    promo: 'Promotion 2024 • Master IA Gouvernance',
    status: 'active',
    statusLabel: 'En cours de développement',
    description:
      "Ce projet vise à analyser les cadres réglementaires et éthiques entourant le déploiement de l'intelligence artificielle dans les institutions publiques, afin de proposer des recommandations adaptées au contexte académique et gouvernemental.",
    objectifsTechniques: [
      "Cartographie des réglementations IA en vigueur (EU AI Act, RGPD) et de leur applicabilité aux systèmes d'information publics.",
      "Développement d'un outil d'audit automatisé permettant d'évaluer la conformité des systèmes IA déployés.",
      "Mise en place d'indicateurs de transparence algorithmique mesurables et reproductibles.",
    ],
    objectifsFonctionnels: [
      "Interface de reporting permettant aux décideurs publics de visualiser les risques associés aux systèmes IA.",
      "Système de recommandations contextuelles basé sur l'analyse des politiques existantes.",
      "Module de simulation des impacts éthiques avant déploiement d'un modèle en production.",
    ],
    members: [
      { name: 'Fatima Z.', role: 'Frontend' },
      { name: 'Vous', role: 'Backend' },
      { name: 'Karim B.', role: 'IA' },
    ],
    dateDebut: { day: '05', month: 'MAI' },
    dateFin: { day: '25', month: 'JUIN' },
    soutenance: 'Soutenance prévue fin juin',
    resources: [
      { label: 'Repository GitHub', type: 'github' },
      { label: 'Documentation API', type: 'doc' },
    ],
  },
  2: {
    id: 2,
    title: 'Optimisation des modèles de langage',
    badge: 'RESPONSABLE BACKEND',
    promo: 'Promotion 2024 • Master IA Gouvernance',
    status: 'pending',
    statusLabel: 'En attente de validation',
    description:
      "Ce projet vise à concevoir une architecture logicielle permettant le déploiement et l'exécution de Large Language Models (LLM) sur des infrastructures locales restreintes, tout en garantissant des performances de latence et de confidentialité optimales pour les institutions académiques.",
    objectifsTechniques: [
      "Mise en œuvre de techniques de quantification avancées (4-bit, 8-bit) pour réduire l'empreinte mémoire sans dégradation significative de la perplexité.",
      "Développement d'une API de service asynchrone pour la gestion des requêtes simultanées en environnement conteneurisé.",
      "Optimisation des couches d'attention via FlashAttention-2 pour accélérer le traitement des contextes longs sur GPU grand public.",
    ],
    objectifsFonctionnels: [
      "Interface de gestion de prompts sécurisée avec historisation locale chiffrée.",
      "Module d'évaluation automatisé permettant de comparer les sorties du modèle local face à des benchmarks de référence (MMLU, GSM8K).",
      "Système de \"Guardrails\" pour assurer la conformité éthique des réponses générées dans un cadre pédagogique.",
    ],
    members: [
      { name: 'Marc-Antoine D.', role: 'Frontend' },
      { name: 'Vous', role: 'Backend' },
      { name: 'Sophie L.', role: 'DevOps' },
    ],
    dateDebut: { day: '12', month: 'MAI' },
    dateFin: { day: '30', month: 'JUIN' },
    soutenance: 'Soutenance prévue fin juin',
    resources: [
      { label: 'Repository GitHub', type: 'github' },
      { label: 'Documentation API', type: 'doc' },
    ],
  },
  3: {
    id: 3,
    title: 'Sécurisation des APIs Locales',
    badge: 'RESPONSABLE DEVOPS',
    promo: 'Promotion 2024 • Master IA Gouvernance',
    status: 'upcoming',
    statusLabel: 'À venir',
    description:
      "Ce projet porte sur la conception et la mise en œuvre de protocoles de sécurité robustes pour les APIs d'inférence déployées en local, afin de protéger les données sensibles et d'assurer la conformité aux standards de cybersécurité académiques.",
    objectifsTechniques: [
      "Implémentation d'un mécanisme d'authentification mutuelle (mTLS) entre les services d'inférence et les clients.",
      "Développement d'un système de détection d'intrusion basé sur l'analyse comportementale des requêtes API.",
      "Chiffrement de bout en bout des flux de données entre les composants du système distribué.",
    ],
    objectifsFonctionnels: [
      "Tableau de bord de monitoring en temps réel des accès et anomalies détectées.",
      "Système de rotation automatique des clés d'API avec journalisation sécurisée.",
      "Interface de configuration des politiques de sécurité adaptée aux administrateurs non-experts.",
    ],
    members: [
      { name: 'Yasmine T.', role: 'Frontend' },
      { name: 'Vous', role: 'Backend' },
      { name: 'Omar K.', role: 'DevOps' },
    ],
    dateDebut: { day: '01', month: 'JUIN' },
    dateFin: { day: '15', month: 'JUIL' },
    soutenance: 'Soutenance prévue mi-juillet',
    resources: [
      { label: 'Repository GitHub', type: 'github' },
      { label: 'Documentation API', type: 'doc' },
    ],
  },
};

// ── Role badge colors ──────────────────────────────────────────────────────────
const roleStyles: Record<string, string> = {
  Frontend: 'bg-blue-100 text-blue-700',
  Backend: 'bg-gray-900 text-white',
  DevOps: 'bg-purple-100 text-purple-700',
  IA: 'bg-amber-100 text-amber-700',
};

// ── Status styles ──────────────────────────────────────────────────────────────
const statusConfig: Record<string, { bg: string; dot: string; text: string }> = {
  active: { bg: 'bg-emerald-500', dot: 'bg-emerald-300', text: 'text-white' },
  pending: { bg: 'bg-amber-400', dot: 'bg-amber-200', text: 'text-white' },
  upcoming: { bg: 'bg-blue-500', dot: 'bg-blue-300', text: 'text-white' },
};

// ── Component ──────────────────────────────────────────────────────────────────
const CahierDesChargesPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const project = projectsData[Number(id)] ?? projectsData[2];
  const st = statusConfig[project.status];

  return (
    <div className="min-h-full bg-white animate-fadeIn">
      {/* ── Breadcrumb ── */}
      <div className="px-8 pt-6 pb-4 flex items-center gap-1.5 text-sm text-gray-400">
        <button
          onClick={() => navigate('/')}
          className="hover:text-blue-600 transition-colors"
        >
          Projets
        </button>
        <ChevronRight size={14} />
        <span className="text-gray-700 font-medium truncate">{project.title}</span>
      </div>

      {/* ── Title block ── */}
      <div className="px-8 pb-6">
        <h1 className="text-3xl font-extrabold text-gray-900 mb-3 leading-tight">
          {project.title}
        </h1>
        <div className="flex items-center gap-3 flex-wrap">
          <span className="text-[11px] font-bold tracking-widest uppercase bg-gray-100 text-gray-600 px-3 py-1 rounded-full border border-gray-200">
            {project.badge}
          </span>
          <Circle size={4} className="text-gray-300 fill-gray-300" />
          <span className="text-sm text-gray-400">{project.promo}</span>
        </div>
      </div>

      {/* ── Body ── */}
      <div className="px-8 pb-12 flex flex-col lg:flex-row gap-6">

        {/* ── Main content ── */}
        <div className="flex-1 min-w-0">
          <div className="bg-gray-50 rounded-2xl border border-gray-100 p-8">

            {/* Section header */}
            <div className="flex items-center gap-2 mb-5">
              <FileText size={20} className="text-gray-500" />
              <h2 className="text-xl font-bold text-gray-900">Cahier des charges</h2>
            </div>

            {/* Description */}
            <p className="text-gray-600 leading-relaxed text-[15px] mb-8">
              {project.description}
            </p>

            {/* Objectifs Techniques */}
            <div className="mb-8">
              <h3 className="text-base font-bold text-gray-800 mb-4">Objectifs Techniques</h3>
              <ul className="space-y-3">
                {project.objectifsTechniques.map((obj, i) => (
                  <li key={i} className="flex items-start gap-3 text-[14px] text-gray-600 leading-relaxed">
                    <CheckCircle2 size={16} className="mt-0.5 flex-shrink-0 text-emerald-500" />
                    <span>{obj}</span>
                  </li>
                ))}
              </ul>
            </div>

            {/* Objectifs Fonctionnels */}
            <div>
              <h3 className="text-base font-bold text-gray-800 mb-4">Objectifs Fonctionnels</h3>
              <ul className="space-y-3">
                {project.objectifsFonctionnels.map((obj, i) => (
                  <li key={i} className="flex items-start gap-3 text-[14px] text-gray-600 leading-relaxed">
                    <CheckCircle2 size={16} className="mt-0.5 flex-shrink-0 text-blue-500" />
                    <span>{obj}</span>
                  </li>
                ))}
              </ul>
            </div>
          </div>
        </div>

        {/* ── Right sidebar ── */}
        <div className="w-full lg:w-72 flex-shrink-0 space-y-5">

          {/* Status */}
          <div className="bg-white rounded-2xl border border-gray-100 p-5">
            <p className="text-[10px] font-bold tracking-widest text-gray-400 uppercase mb-3">
              Statut du projet
            </p>
            <div className={`flex items-center gap-2.5 px-4 py-3 rounded-xl ${st.bg}`}>
              <span className={`w-2 h-2 rounded-full ${st.dot} animate-pulse`} />
              <span className={`text-sm font-bold ${st.text}`}>{project.statusLabel}</span>
            </div>
          </div>

          {/* Members */}
          <div className="bg-white rounded-2xl border border-gray-100 p-5">
            <p className="text-[10px] font-bold tracking-widest text-gray-400 uppercase mb-4">
              Membres du groupe
            </p>
            <div className="space-y-3">
              {project.members.map((m, i) => (
                <div key={i} className="flex items-center justify-between">
                  <span className={`text-sm font-semibold ${m.name === 'Vous' ? 'text-gray-900' : 'text-gray-600'}`}>
                    {m.name}
                  </span>
                  <span className={`text-[10px] font-bold px-2.5 py-1 rounded-md ${roleStyles[m.role]}`}>
                    {m.role}
                  </span>
                </div>
              ))}
            </div>
          </div>

          {/* Key dates */}
          <div className="bg-white rounded-2xl border border-gray-100 p-5">
            <p className="text-[10px] font-bold tracking-widest text-gray-400 uppercase mb-4">
              Dates clés
            </p>
            <div className="flex gap-3 mb-3">
              {/* Start */}
              <div className="flex-1 bg-gray-50 rounded-xl p-3 text-center border border-gray-100">
                <p className="text-2xl font-extrabold text-gray-900 leading-none">
                  {project.dateDebut.day}
                </p>
                <p className="text-[10px] font-bold text-gray-400 uppercase mt-1">
                  {project.dateDebut.month}
                </p>
              </div>
              {/* End */}
              <div className="flex-1 bg-gray-50 rounded-xl p-3 text-center border border-gray-100">
                <p className="text-2xl font-extrabold text-gray-900 leading-none">
                  {project.dateFin.day}
                </p>
                <p className="text-[10px] font-bold text-gray-400 uppercase mt-1">
                  {project.dateFin.month}
                </p>
              </div>
            </div>
            <div className="flex items-center gap-1.5 text-xs text-gray-400">
              <CalendarDays size={12} />
              <span>{project.soutenance}</span>
            </div>
          </div>



          {/* Timer indicator */}
          <div className="bg-white rounded-2xl border border-gray-100 p-5 flex items-center gap-3 text-gray-500">
            <Clock size={16} className="text-gray-400 flex-shrink-0" />
            <p className="text-xs leading-snug">
              Dernière modification il y a <span className="font-semibold text-gray-700">2 heures</span>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CahierDesChargesPage;
