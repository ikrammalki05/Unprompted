import React, { useState, useEffect } from 'react';
import {
  Calendar, CheckCircle2, Circle, FileText, GitCommit, MessageSquare, Bot, Clock
} from 'lucide-react';

import projetService from '../services/projetService';
import iaService from '../services/iaService';

/* ── Types ── */
interface Step { id: number; label: string; status: 'done'|'in-progress'|'todo' }

/* ── Mock Data for static parts ── */
const mockSteps: Step[] = [
  { id:1, label:'Preprocessing', status:'done' },
  { id:2, label:'Model Setup', status:'done' },
  { id:3, label:'Training', status:'in-progress' },
  { id:4, label:'Evaluation', status:'todo' },
];

const TABS: {key:TabKey;label:string}[] = [
  { key:'overview', label:"Vue d'ensemble" },
  { key:'activity', label:'Activité' },
  { key:'commits', label:'Historique des commits' },
];

type TabKey = 'overview'|'activity'|'commits';

/* ── Sub-components ── */
const StepBadge: React.FC<{status:Step['status']}> = ({status}) => {
  const m = { done:{l:'TERMINÉ',c:'bg-[#dcfce7] text-[#16a34a]'}, 'in-progress':{l:'EN COURS',c:'bg-[#dbeafe] text-[#2563eb]'}, todo:{l:'À FAIRE',c:'bg-slate-100 text-slate-400'} } as const;
  const {l,c} = m[status];
  return <span className={`text-[10px] font-black px-2.5 py-1 rounded-full uppercase tracking-wider ${c}`}>{l}</span>;
};
const StepIcon: React.FC<{status:Step['status']}> = ({status}) => {
  if (status==='done') return <CheckCircle2 className="w-5 h-5 text-[#22c55e]" fill="#dcfce7" />;
  if (status==='in-progress') return <Circle className="w-5 h-5 text-[#3b82f6]" fill="#dbeafe" />;
  return <Circle className="w-5 h-5 text-slate-300" />;
};

/* ── Tabs ── */
const OverviewTab: React.FC<{project: any, note:string;setNote:(v:string)=>void}> = ({project, note,setNote}) => (
  <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
    <div className="lg:col-span-2 flex flex-col gap-6">
      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-[15px] font-black text-slate-900">Progression</h2>
          <span className="text-[13px] font-black text-blue-600">{project?.progression || 0}% complété</span>
        </div>
        <div className="w-full h-2.5 bg-slate-100 rounded-full mb-6 overflow-hidden">
          <div className="h-full bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full transition-all duration-700" style={{width:`${project?.progression || 0}%`}} />
        </div>
        <div className="grid grid-cols-2 gap-3">
          {mockSteps.map(s=>(
            <div key={s.id} className="flex items-center gap-3 p-3 bg-slate-50 rounded-2xl border border-slate-100/60">
              <StepIcon status={s.status}/><span className="flex-1 text-[13px] font-semibold text-slate-700">{s.label}</span><StepBadge status={s.status}/>
            </div>
          ))}
        </div>
      </div>

      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <h2 className="text-[15px] font-black text-slate-900 mb-5">Description du projet</h2>
        <p className="text-[14px] text-slate-600 leading-relaxed">
            {project?.description || "Aucune description fournie pour ce projet."}
        </p>
      </div>
    </div>

    <div className="flex flex-col gap-6">
      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <div className="flex items-center gap-2 mb-5"><FileText className="w-4 h-4 text-slate-400"/><h2 className="text-[15px] font-black text-slate-900">Notes de l'enseignant</h2></div>
        <textarea className="w-full h-36 resize-none bg-slate-50 border border-slate-100 rounded-2xl p-4 text-[13px] text-slate-700 placeholder-slate-300 font-medium focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all" placeholder="Ajouter une observation..." value={note} onChange={e=>setNote(e.target.value)}/>
        <button className="mt-4 w-full py-3 bg-slate-900 hover:bg-slate-800 text-white text-[13px] font-black rounded-2xl transition-all shadow-md shadow-slate-900/10 active:scale-[0.98]">Enregistrer</button>
      </div>
    </div>
  </div>
);

const ActivityTab: React.FC<{prompts: any[]}> = ({prompts}) => {
    const getInitials = (name: string) => name.split(' ').map(n => n[0]).join('').toUpperCase().substring(0, 2);

    return (
        <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7 min-h-[400px]">
            <div className="flex items-center justify-between mb-8">
                <h2 className="text-[15px] font-black text-slate-900 flex items-center gap-2">
                    <MessageSquare className="w-4 h-4 text-blue-600" />
                    Flux d'activité IA
                </h2>
                <span className="text-[11px] font-black text-slate-400 uppercase tracking-widest">{prompts.length} Échanges</span>
            </div>

            <div className="flex flex-col gap-8">
                {prompts.map((p, idx) => (
                    <div key={idx} className="relative pl-8 border-l-2 border-slate-50 space-y-4">
                        {/* Dot on the line */}
                        <div className="absolute -left-[9px] top-0 w-4 h-4 bg-white border-2 border-blue-600 rounded-full shadow-sm" />
                        
                        {/* Student Prompt */}
                        <div className="flex flex-col gap-3">
                            <div className="flex items-center gap-3">
                                <div className="w-8 h-8 rounded-full bg-slate-900 text-white flex items-center justify-center text-[10px] font-black">
                                    {getInitials(p.nomEtudiant)}
                                </div>
                                <div className="flex flex-col">
                                    <span className="text-[13px] font-black text-slate-900">{p.nomEtudiant}</span>
                                    <span className="text-[10px] text-slate-400 font-bold flex items-center gap-1">
                                        <Clock className="w-3 h-3" /> {p.datePrompt ? new Date(p.datePrompt).toLocaleString() : 'Date inconnue'}
                                    </span>
                                </div>
                            </div>
                            <div className="bg-slate-50 p-5 rounded-[24px] border border-slate-100 ml-11">
                                <p className="text-[14px] text-slate-700 leading-relaxed font-medium italic">
                                    "{p.contenu}"
                                </p>
                            </div>
                        </div>

                        {/* AI Response(s) */}
                        {p.reponses && p.reponses.map((rep: any, rIdx: number) => (
                            <div key={rIdx} className="flex flex-col gap-3 ml-11">
                                <div className="flex items-center gap-2">
                                    <div className="w-6 h-6 bg-blue-100 text-blue-600 rounded-lg flex items-center justify-center">
                                        <Bot className="w-3.5 h-3.5" />
                                    </div>
                                    <span className="text-[11px] font-black text-blue-600 uppercase tracking-widest">Réponse de l'IA ({rep.modeleIa || 'GPT-4'})</span>
                                </div>
                                <div className="bg-blue-50/30 p-5 rounded-[24px] border border-blue-100/50">
                                    <p className="text-[13px] text-slate-600 leading-relaxed">
                                        {rep.contenuReponse}
                                    </p>
                                </div>
                            </div>
                        ))}
                    </div>
                ))}

                {prompts.length === 0 && (
                    <div className="flex flex-col items-center justify-center py-20 text-center">
                        <div className="w-16 h-16 bg-slate-50 rounded-2xl flex items-center justify-center mb-4">
                            <MessageSquare className="w-8 h-8 text-slate-200" />
                        </div>
                        <p className="text-slate-400 font-medium">Aucune activité IA enregistrée pour ce projet.</p>
                    </div>
                )}
            </div>
        </div>
    );
};

const CommitsTab: React.FC<{commits: any[]}> = ({commits}) => (
    <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <h2 className="text-[15px] font-black text-slate-900 mb-5">{commits.length} commit{commits.length>1?'s':''}</h2>
        <div className="overflow-x-auto">
          <table className="w-full text-[12px]">
            <thead><tr className="border-b border-slate-100">
              {['HASH','MESSAGE','DATE'].map(h=><th key={h} className="pb-3 text-left font-black text-slate-400 uppercase tracking-widest pr-4 last:pr-0">{h}</th>)}
            </tr></thead>
            <tbody className="divide-y divide-slate-50">
              {commits.map((c,i)=>(
                <tr key={i} className="group hover:bg-slate-50/60 transition-colors">
                  <td className="py-3.5 pr-4"><code className="bg-slate-100 text-slate-600 px-2 py-0.5 rounded-lg text-[11px] font-mono font-bold">{c.hashCommit?.substring(0,7) || '---'}</code></td>
                  <td className="py-3.5 pr-4 font-semibold text-slate-700 flex items-center gap-2"><GitCommit className="w-3.5 h-3.5 text-slate-300 flex-shrink-0"/>{c.messageCommit}</td>
                  <td className="py-3.5 pr-4 text-slate-500">{new Date(c.dateCommit).toLocaleDateString()}</td>
                </tr>
              ))}
              {commits.length === 0 && (
                  <tr>
                      <td colSpan={3} className="py-10 text-center text-slate-400 font-medium">Aucun commit trouvé pour ce projet.</td>
                  </tr>
              )}
            </tbody>
          </table>
        </div>
    </div>
);

/* ── Main Component ── */
const ProjetDetails: React.FC<{id?: number | null}> = ({id}) => {
  const [project, setProject] = useState<any>(null);
  const [commits, setCommits] = useState<any[]>([]);
  const [prompts, setPrompts] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [note, setNote] = useState('');
  const [activeTab, setActiveTab] = useState<TabKey>('overview');

  useEffect(() => {
    if (!id) return;

    const fetchData = async () => {
      try {
        setLoading(true);
        // 1. Details du projet
        const pData = await projetService.getById(id);
        setProject(pData);
        setNote(pData.notesEnseignant || '');

        // 2. Commits
        const cData = await projetService.getContributions(id);
        setCommits(cData);

        // 3. Prompts IA
        const prData = await iaService.getPrompts();
        // Optionnel: filtrer les prompts par projet si l'API ne le fait pas déjà
        // Pour l'instant on les affiche tous ou on filtre côté client
        setPrompts(prData);

      } catch (error) {
        console.error("Erreur lors du chargement des détails du projet", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  if (!id) {
    return (
      <div className="flex flex-col items-center justify-center min-h-[60vh] text-center p-8">
        <div className="w-20 h-20 bg-slate-50 text-slate-300 rounded-3xl flex items-center justify-center mb-6">
            <FolderRight className="w-10 h-10" />
        </div>
        <h2 className="text-xl font-black text-slate-900 mb-2">Aucun projet sélectionné</h2>
        <p className="text-slate-500 max-w-sm">Veuillez sélectionner un projet dans le tableau de bord pour voir ses détails et son activité.</p>
      </div>
    );
  }

  if (loading) {
      return (
          <div className="flex items-center justify-center min-h-[60vh]">
              <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
          </div>
      );
  }

  const renderTab = () => {
    switch(activeTab) {
      case 'overview': return <OverviewTab project={project} note={note} setNote={setNote}/>;
      case 'activity': return <ActivityTab prompts={prompts}/>;
      case 'commits': return <CommitsTab commits={commits}/>;
    }
  };

  return (
    <div className="p-8 pb-20 max-w-7xl mx-auto animate-in fade-in duration-500">
      <div className="flex items-center gap-2 text-[12px] font-semibold text-slate-400 mb-4 uppercase tracking-widest">
        <span className="cursor-pointer hover:text-blue-600 transition-colors">PROJETS</span><span>›</span><span className="text-slate-600 uppercase">{project?.titre || 'DÉTAILS'}</span>
      </div>
      
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-8">
        <h1 className="text-3xl font-black text-slate-900 tracking-tight">{project?.titre}</h1>
        <div className="flex items-center gap-3">
          <span className="flex items-center gap-1.5 px-3 py-1.5 bg-[#dcfce7] text-[#16a34a] text-[11px] font-black rounded-full uppercase tracking-wide">
              <span className="w-1.5 h-1.5 rounded-full bg-[#22c55e] inline-block"/>{project?.status || 'En cours'}
          </span>
          <span className="flex items-center gap-1.5 px-3 py-1.5 bg-white border border-slate-100 rounded-full text-[11px] font-semibold text-slate-500 shadow-sm">
              <Calendar className="w-3.5 h-3.5"/>Deadline: {project?.dateFin ? new Date(project.dateFin).toLocaleDateString() : 'Non définie'}
          </span>
        </div>
      </div>

      <div className="flex gap-6 border-b border-slate-100 mb-8">
        {TABS.map(tab=>(
          <button key={tab.key} onClick={()=>setActiveTab(tab.key)} className={`pb-3 text-[13px] font-bold tracking-tight transition-colors border-b-2 ${activeTab===tab.key?'border-blue-600 text-blue-600':'border-transparent text-slate-400 hover:text-slate-600'}`}>{tab.label}</button>
        ))}
      </div>

      <div className="animate-in fade-in duration-300">{renderTab()}</div>
    </div>
  );
};

// Simple Icon for empty state
const FolderRight = ({className}: {className?: string}) => (
    <svg className={className} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
        <path d="M4 20h16a2 2 0 0 0 2-2V8a2 2 0 0 0-2-2h-7.93a2 2 0 0 1-1.66-.9l-.82-1.2A2 2 0 0 0 7.93 3H4a2 2 0 0 0-2 2v13c0 1.1.9 2 2 2Z" />
        <path d="M12 10l3 3-3 3" /><path d="M8 13h7" />
    </svg>
);

export default ProjetDetails;
