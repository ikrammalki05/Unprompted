import React, { useState } from 'react';
import {
  Calendar, CheckCircle2, Circle, FileText, GitCommit, Clock,
  Search, RefreshCw, User, MessageSquare, Bot, AlertTriangle,
  ChevronDown, ChevronRight, Zap, Brain, Eye,
} from 'lucide-react';

/* ── Types ── */
interface Step { id: number; label: string; status: 'done'|'in-progress'|'todo' }
interface Prompt {
  id: number; student: string; initials: string; color: string;
  prompt: string; model: string; time: string; date: string;
  tokens: number; flagged: boolean; response: string;
}
interface Commit {
  hash: string; message: string; author: string;
  date: string; branch: string; branchColor: string;
}

/* ── Data ── */
const steps: Step[] = [
  { id:1, label:'Preprocessing', status:'done' },
  { id:2, label:'Model Setup', status:'done' },
  { id:3, label:'Training', status:'in-progress' },
  { id:4, label:'Evaluation', status:'todo' },
];

const prompts: Prompt[] = [
  { id:1, student:'Marie Curie', initials:'MC', color:'#4f6ef7',
    prompt:'Explique-moi comment fonctionne la quantization dans les LLM et comment l\'implémenter en PyTorch',
    model:'GPT-4', time:'il y a 2h', date:'10/05/2024', tokens:342, flagged:false,
    response:'La quantization est une technique qui réduit la précision des poids du modèle...' },
  { id:2, student:'Jean Dupont', initials:'JD', color:'#22c55e',
    prompt:'Génère le code complet pour le module de preprocessing des données textuelles',
    model:'Claude 3.5', time:'il y a 3h', date:'10/05/2024', tokens:891, flagged:true,
    response:'Voici le code complet pour le preprocessing...' },
  { id:3, student:'Marie Curie', initials:'MC', color:'#4f6ef7',
    prompt:'Quelles sont les meilleures pratiques pour fine-tuner un modèle de langage sur un dataset spécifique ?',
    model:'GPT-4', time:'il y a 5h', date:'10/05/2024', tokens:256, flagged:false,
    response:'Pour fine-tuner efficacement un LLM, voici les étapes recommandées...' },
  { id:4, student:'Jean Dupont', initials:'JD', color:'#22c55e',
    prompt:'Compare les architectures Transformer et LSTM pour le traitement du langage naturel',
    model:'Gemini Pro', time:'il y a 1j', date:'09/05/2024', tokens:478, flagged:false,
    response:'Les Transformers et les LSTM sont deux architectures fondamentales...' },
  { id:5, student:'Marie Curie', initials:'MC', color:'#4f6ef7',
    prompt:'Écris-moi un script Python pour évaluer les performances du modèle avec BLEU et ROUGE',
    model:'GPT-4', time:'il y a 1j', date:'09/05/2024', tokens:623, flagged:false,
    response:'Voici un script complet d\'évaluation utilisant les métriques BLEU et ROUGE...' },
  { id:6, student:'Jean Dupont', initials:'JD', color:'#22c55e',
    prompt:'Donne-moi la solution complète du TP3 sur les réseaux de neurones récurrents',
    model:'Claude 3.5', time:'il y a 2j', date:'08/05/2024', tokens:1205, flagged:true,
    response:'Voici les réponses aux exercices du TP3...' },
];

const commits: Commit[] = [
  { hash:'a3f2c1d', message:'Feat: Add quantization layer', author:'Marie Curie', date:'10/05/2024', branch:'main', branchColor:'#6366f1' },
  { hash:'b7e4f9a', message:'Fix: Model inference bug', author:'Jean Dupont', date:'09/05/2024', branch:'dev', branchColor:'#f59e0b' },
  { hash:'c1d8e3b', message:'Refactor: Clean up data pipeline', author:'Marie Curie', date:'08/05/2024', branch:'main', branchColor:'#6366f1' },
  { hash:'d4a9f2c', message:'Docs: Update README with new setup', author:'Jean Dupont', date:'07/05/2024', branch:'dev', branchColor:'#f59e0b' },
  { hash:'e5b0c3d', message:'Feat: Implement attention mechanism', author:'Marie Curie', date:'06/05/2024', branch:'main', branchColor:'#6366f1' },
  { hash:'f6c1d4e', message:'Fix: Memory leak in training loop', author:'Jean Dupont', date:'05/05/2024', branch:'dev', branchColor:'#f59e0b' },
];

const progress = 65;
type TabKey = 'overview'|'activity'|'commits';
const TABS: {key:TabKey;label:string}[] = [
  { key:'overview', label:"Vue d'ensemble" },
  { key:'activity', label:'Activité' },
  { key:'commits', label:'Historique des commits' },
];

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

/* ── Vue d'ensemble ── */
const OverviewTab: React.FC<{note:string;setNote:(v:string)=>void}> = ({note,setNote}) => (
  <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
    <div className="lg:col-span-2 flex flex-col gap-6">
      {/* Progression */}
      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-[15px] font-black text-slate-900">Progression</h2>
          <span className="text-[13px] font-black text-blue-600">{progress}% complété</span>
        </div>
        <div className="w-full h-2.5 bg-slate-100 rounded-full mb-6 overflow-hidden">
          <div className="h-full bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full transition-all duration-700" style={{width:`${progress}%`}} />
        </div>
        <div className="grid grid-cols-2 gap-3">
          {steps.map(s=>(
            <div key={s.id} className="flex items-center gap-3 p-3 bg-slate-50 rounded-2xl border border-slate-100/60">
              <StepIcon status={s.status}/><span className="flex-1 text-[13px] font-semibold text-slate-700">{s.label}</span><StepBadge status={s.status}/>
            </div>
          ))}
        </div>
      </div>

      {/* Activité récente - prompts */}
      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <h2 className="text-[15px] font-black text-slate-900 mb-5">Activité récente</h2>
        <div className="space-y-4">
          {prompts.slice(0,2).map(p=>(
            <div key={p.id} className="flex items-start gap-3 group">
              <div className="w-8 h-8 rounded-full flex items-center justify-center text-white text-[10px] font-black flex-shrink-0 mt-0.5" style={{background:p.color}}>{p.initials}</div>
              <div className="flex-1 flex items-start justify-between gap-2">
                <div>
                  <p className="text-[13px] text-slate-600 leading-snug">
                    <span className="font-black text-slate-900">{p.student}</span> a envoyé un prompt
                    {p.flagged && <span className="ml-1.5 inline-flex items-center gap-0.5 text-[10px] font-black text-amber-600 bg-amber-50 px-1.5 py-0.5 rounded-full"><AlertTriangle className="w-2.5 h-2.5"/>Suspect</span>}
                  </p>
                  <p className="text-[12px] text-slate-400 mt-1 line-clamp-1 italic">"{p.prompt}"</p>
                </div>
                <span className="flex items-center gap-1 text-[11px] text-slate-400 font-semibold whitespace-nowrap flex-shrink-0">
                  <Clock className="w-3 h-3"/>{p.time}
                </span>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Commits summary */}
      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <h2 className="text-[15px] font-black text-slate-900 mb-5">Historique des commits</h2>
        <div className="overflow-x-auto">
          <table className="w-full text-[12px]">
            <thead><tr className="border-b border-slate-100">
              {['MESSAGE','AUTEUR','DATE','BRANCHE'].map(h=><th key={h} className="pb-3 text-left font-black text-slate-400 uppercase tracking-widest pr-4 last:pr-0">{h}</th>)}
            </tr></thead>
            <tbody className="divide-y divide-slate-50">
              {commits.slice(0,2).map((c,i)=>(
                <tr key={i} className="group hover:bg-slate-50/60 transition-colors">
                  <td className="py-3.5 pr-4 font-semibold text-slate-700 flex items-center gap-2"><GitCommit className="w-3.5 h-3.5 text-slate-300 flex-shrink-0"/>{c.message}</td>
                  <td className="py-3.5 pr-4 text-slate-600 font-medium">{c.author}</td>
                  <td className="py-3.5 pr-4 text-slate-500">{c.date}</td>
                  <td className="py-3.5"><span className="px-2.5 py-1 rounded-lg text-white text-[10px] font-black" style={{background:c.branchColor}}>{c.branch}</span></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>

    {/* Right col */}
    <div className="flex flex-col gap-6">
      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <div className="flex items-center gap-2 mb-5"><FileText className="w-4 h-4 text-slate-400"/><h2 className="text-[15px] font-black text-slate-900">Notes de l'enseignant</h2></div>
        <textarea className="w-full h-36 resize-none bg-slate-50 border border-slate-100 rounded-2xl p-4 text-[13px] text-slate-700 placeholder-slate-300 font-medium focus:outline-none focus:ring-2 focus:ring-blue-100 focus:bg-white transition-all" placeholder="Ajouter une observation..." value={note} onChange={e=>setNote(e.target.value)}/>
        <button className="mt-4 w-full py-3 bg-slate-900 hover:bg-slate-800 text-white text-[13px] font-black rounded-2xl transition-all shadow-md shadow-slate-900/10 active:scale-[0.98]">Enregistrer</button>
      </div>
    </div>
  </div>
);

/* ── Activité (Prompts IA) ── */
const ActivityTab: React.FC = () => {
  const [search, setSearch] = useState('');
  const [expandedId, setExpandedId] = useState<number|null>(null);

  const filtered = prompts.filter(p => {
    return !search || p.prompt.toLowerCase().includes(search.toLowerCase()) || p.student.toLowerCase().includes(search.toLowerCase());
  });
  const flaggedCount = prompts.filter(p=>p.flagged).length;
  const totalTokens = prompts.reduce((s,p)=>s+p.tokens,0);

  return (
    <div className="flex flex-col gap-6 max-w-5xl">
      {/* Stats */}
      <div className="grid grid-cols-4 gap-4">
        {[
          { label:'Total prompts', count:prompts.length, icon:<MessageSquare className="w-4 h-4"/>, color:'#4f6ef7' },
          { label:'Prompts suspects', count:flaggedCount, icon:<AlertTriangle className="w-4 h-4"/>, color:'#ef4444' },
          { label:'Tokens utilisés', count:totalTokens, icon:<Zap className="w-4 h-4"/>, color:'#f59e0b' },
          { label:'Modèles utilisés', count:[...new Set(prompts.map(p=>p.model))].length, icon:<Brain className="w-4 h-4"/>, color:'#8b5cf6' },
        ].map(s=>(
          <div key={s.label} className="bg-white rounded-[20px] border border-slate-100 shadow-sm p-5 flex items-center gap-4">
            <div className="w-10 h-10 rounded-xl flex items-center justify-center text-white" style={{background:s.color}}>{s.icon}</div>
            <div><p className="text-xl font-black text-slate-900">{s.count.toLocaleString()}</p><p className="text-[11px] font-semibold text-slate-400">{s.label}</p></div>
          </div>
        ))}
      </div>

      {/* Toolbar */}
      <div className="flex items-center gap-3 flex-wrap">
        <div className="flex items-center gap-2 bg-white border border-slate-200 rounded-2xl px-4 py-2.5 flex-1 max-w-sm shadow-sm">
          <Search className="w-4 h-4 text-slate-400"/>
          <input type="text" placeholder="Rechercher un prompt..." className="bg-transparent text-[13px] text-slate-700 placeholder-slate-400 outline-none w-full font-medium" value={search} onChange={e=>setSearch(e.target.value)}/>
        </div>
      </div>

      {/* Prompts list */}
      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <h2 className="text-[15px] font-black text-slate-900 mb-6">Prompts envoyés par les étudiants</h2>
        <div className="space-y-3">
          {filtered.map(p=>(
            <div key={p.id} className={`rounded-2xl border transition-all ${p.flagged?'border-amber-200 bg-amber-50/30':'border-slate-100 bg-slate-50/50 hover:bg-slate-50'}`}>
              <button onClick={()=>setExpandedId(expandedId===p.id?null:p.id)} className="w-full flex items-start gap-3 p-4 text-left">
                <div className="w-9 h-9 rounded-full flex items-center justify-center text-white text-[10px] font-black flex-shrink-0 mt-0.5" style={{background:p.color}}>{p.initials}</div>
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-1 flex-wrap">
                    <span className="text-[13px] font-black text-slate-900">{p.student}</span>
                    <span className="text-[10px] font-bold text-slate-400 bg-slate-100 px-2 py-0.5 rounded-full flex items-center gap-1"><Bot className="w-2.5 h-2.5"/>{p.model}</span>
                    <span className="text-[10px] font-semibold text-slate-400">{p.tokens} tokens</span>
                    {p.flagged && <span className="text-[10px] font-black text-amber-600 bg-amber-100 px-2 py-0.5 rounded-full flex items-center gap-1"><AlertTriangle className="w-2.5 h-2.5"/>Suspect</span>}
                  </div>
                  <p className="text-[13px] text-slate-600 leading-snug line-clamp-2">"{p.prompt}"</p>
                  <span className="flex items-center gap-1 text-[11px] text-slate-400 font-semibold mt-1.5"><Clock className="w-3 h-3"/>{p.time} · {p.date}</span>
                </div>
                {expandedId===p.id?<ChevronDown className="w-4 h-4 text-slate-400 flex-shrink-0 mt-1"/>:<ChevronRight className="w-4 h-4 text-slate-400 flex-shrink-0 mt-1"/>}
              </button>
              {expandedId===p.id && (
                <div className="px-4 pb-4 pl-16">
                  <div className="bg-white rounded-xl border border-slate-200 p-4">
                    <div className="flex items-center gap-2 mb-2"><Bot className="w-4 h-4 text-indigo-500"/><span className="text-[12px] font-black text-slate-700">Réponse de {p.model}</span></div>
                    <p className="text-[13px] text-slate-600 leading-relaxed">{p.response}</p>
                    <div className="flex items-center gap-3 mt-3 pt-3 border-t border-slate-100">
                      <button className="text-[11px] font-bold text-blue-600 flex items-center gap-1 hover:text-blue-700"><Eye className="w-3 h-3"/>Voir la réponse complète</button>
                    </div>
                  </div>
                </div>
              )}
            </div>
          ))}
          {filtered.length===0 && (
            <div className="text-center py-12"><MessageSquare className="w-10 h-10 text-slate-200 mx-auto mb-3"/><p className="text-[13px] font-semibold text-slate-400">Aucun prompt trouvé</p></div>
          )}
        </div>
      </div>
    </div>
  );
};

/* ── Historique des commits ── */
const CommitsTab: React.FC = () => {
  const [search, setSearch] = useState('');
  const [branchFilter, setBranchFilter] = useState<string|null>(null);
  const branches = [...new Set(commits.map(c=>c.branch))];
  const filtered = commits.filter(c=>{
    const ms = !search || c.message.toLowerCase().includes(search.toLowerCase()) || c.author.toLowerCase().includes(search.toLowerCase());
    const mb = !branchFilter || c.branch===branchFilter;
    return ms&&mb;
  });

  return (
    <div className="flex flex-col gap-6 max-w-5xl">
      <div className="flex items-center gap-3 flex-wrap">
        <div className="flex items-center gap-2 bg-white border border-slate-200 rounded-2xl px-4 py-2.5 flex-1 max-w-sm shadow-sm">
          <Search className="w-4 h-4 text-slate-400"/>
          <input type="text" placeholder="Rechercher un commit..." className="bg-transparent text-[13px] text-slate-700 placeholder-slate-400 outline-none w-full font-medium" value={search} onChange={e=>setSearch(e.target.value)}/>
        </div>
        <div className="flex items-center gap-2">
          <button onClick={()=>setBranchFilter(null)} className={`px-3 py-2 rounded-xl text-[12px] font-bold transition-colors ${!branchFilter?'bg-slate-900 text-white shadow-md':'bg-white border border-slate-200 text-slate-600 hover:bg-slate-50'}`}>Toutes</button>
          {branches.map(b=>{
            const bc=commits.find(c=>c.branch===b)?.branchColor??'#94a3b8';
            return <button key={b} onClick={()=>setBranchFilter(branchFilter===b?null:b)} className={`px-3 py-2 rounded-xl text-[12px] font-bold transition-colors flex items-center gap-1.5 ${branchFilter===b?'text-white shadow-md':'bg-white border border-slate-200 text-slate-600 hover:bg-slate-50'}`} style={branchFilter===b?{background:bc}:{}}><span className="w-2 h-2 rounded-full" style={{background:bc}}/>{b}</button>;
          })}
        </div>
        <button className="flex items-center gap-2 px-4 py-2.5 bg-white border border-slate-200 rounded-2xl text-[13px] font-semibold text-slate-600 hover:bg-slate-50 transition-colors shadow-sm ml-auto"><RefreshCw className="w-3.5 h-3.5"/>Actualiser</button>
      </div>

      <div className="bg-white rounded-[28px] border border-slate-100 shadow-sm p-7">
        <div className="flex items-center justify-between mb-5">
          <h2 className="text-[15px] font-black text-slate-900">{filtered.length} commit{filtered.length>1?'s':''}</h2>
          <span className="text-[11px] font-semibold text-slate-400">Dernière mise à jour: {commits[0]?.date}</span>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-[12px]">
            <thead><tr className="border-b border-slate-100">
              {['HASH','MESSAGE','AUTEUR','DATE','BRANCHE'].map(h=><th key={h} className="pb-3 text-left font-black text-slate-400 uppercase tracking-widest pr-4 last:pr-0">{h}</th>)}
            </tr></thead>
            <tbody className="divide-y divide-slate-50">
              {filtered.map((c,i)=>(
                <tr key={i} className="group hover:bg-slate-50/60 transition-colors">
                  <td className="py-3.5 pr-4"><code className="bg-slate-100 text-slate-600 px-2 py-0.5 rounded-lg text-[11px] font-mono font-bold">{c.hash}</code></td>
                  <td className="py-3.5 pr-4 font-semibold text-slate-700 flex items-center gap-2"><GitCommit className="w-3.5 h-3.5 text-slate-300 flex-shrink-0"/>{c.message}</td>
                  <td className="py-3.5 pr-4 text-slate-600 font-medium"><span className="flex items-center gap-1.5"><User className="w-3 h-3 text-slate-400"/>{c.author}</span></td>
                  <td className="py-3.5 pr-4 text-slate-500">{c.date}</td>
                  <td className="py-3.5"><span className="px-2.5 py-1 rounded-lg text-white text-[10px] font-black" style={{background:c.branchColor}}>{c.branch}</span></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        {filtered.length===0 && <div className="text-center py-12"><GitCommit className="w-10 h-10 text-slate-200 mx-auto mb-3"/><p className="text-[13px] font-semibold text-slate-400">Aucun commit trouvé</p></div>}
      </div>
    </div>
  );
};

/* ── Main ── */
const ProjetDetails: React.FC = () => {
  const [note, setNote] = useState('');
  const [activeTab, setActiveTab] = useState<TabKey>('overview');

  const renderTab = () => {
    switch(activeTab) {
      case 'overview': return <OverviewTab note={note} setNote={setNote}/>;
      case 'activity': return <ActivityTab/>;
      case 'commits': return <CommitsTab/>;
    }
  };

  return (
    <div className="p-8 pb-20 max-w-7xl mx-auto animate-in fade-in duration-500">
      <div className="flex items-center gap-2 text-[12px] font-semibold text-slate-400 mb-4 uppercase tracking-widest">
        <span className="cursor-pointer hover:text-blue-600 transition-colors">PROJETS</span><span>›</span><span className="text-slate-600">OPTIMISATION DE LLM LOCAL</span>
      </div>
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-8">
        <h1 className="text-3xl font-black text-slate-900 tracking-tight">Optimisation de LLM local</h1>
        <div className="flex items-center gap-3">
          <span className="flex items-center gap-1.5 px-3 py-1.5 bg-[#dcfce7] text-[#16a34a] text-[11px] font-black rounded-full uppercase tracking-wide"><span className="w-1.5 h-1.5 rounded-full bg-[#22c55e] inline-block"/>En cours</span>
          <span className="flex items-center gap-1.5 px-3 py-1.5 bg-white border border-slate-100 rounded-full text-[11px] font-semibold text-slate-500 shadow-sm"><Calendar className="w-3.5 h-3.5"/>Deadline: 15 Oct 2024</span>
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

export default ProjetDetails;
