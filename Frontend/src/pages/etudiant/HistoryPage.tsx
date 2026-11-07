import React, { useEffect, useMemo, useState } from 'react';
import { api } from '../../services/api';

interface Historique {
  idEtudiant: number;
  contributionsGit: Array<{
    messageCommit: string;
    dateCommit: string;
    nomProjet: string;
    lignesAjoutees: number;
    lignesSupprimees: number;
  }>;
  interactionsIa: Array<{
    question: string;
    dateQuestion: string;
    nomProjet: string;
    tokensConsommes: number;
  }>;
  evaluations: Array<{
    note: number;
    commentaire: string;
    dateEvaluation: string;
    nomProjet: string;
    nomEnseignant: string;
  }>;
}

type TimelineItem = {
  kind: 'GIT' | 'IA' | 'NOTE';
  date: string;
  title: string;
  detail: string;
};

const HistoryPage: React.FC = () => {
  const [data, setData] = useState<Historique | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const load = async () => {
      try {
        const res = await api.get('/Profil/me/historique');
        setData(res.data);
      } catch (e: any) {
        setError(e?.response?.data?.message || 'Impossible de charger l’historique.');
      } finally {
        setLoading(false);
      }
    };
    void load();
  }, []);

  const timeline = useMemo(() => {
    if (!data) return [] as TimelineItem[];
    const items: TimelineItem[] = [
      ...(data.contributionsGit || []).map((c) => ({
        kind: 'GIT' as const,
        date: c.dateCommit,
        title: c.messageCommit,
        detail: `${c.nomProjet} · +${c.lignesAjoutees}/-${c.lignesSupprimees}`,
      })),
      ...(data.interactionsIa || []).map((p) => ({
        kind: 'IA' as const,
        date: p.dateQuestion,
        title: p.question?.slice(0, 140) || 'Interaction IA',
        detail: `${p.nomProjet} · ${p.tokensConsommes} tokens`,
      })),
      ...(data.evaluations || []).map((e) => ({
        kind: 'NOTE' as const,
        date: e.dateEvaluation,
        title: `Note ${e.note}/20 — ${e.nomProjet}`,
        detail: `${e.nomEnseignant}: ${e.commentaire || 'Sans commentaire'}`,
      })),
    ];
    return items.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
  }, [data]);

  if (loading) return <div className="p-10 text-gray-400">Chargement de l’historique…</div>;
  if (error) return <div className="p-10 text-red-500">{error}</div>;

  return (
    <div className="p-8 max-w-4xl">
      <h1 className="text-3xl font-bold text-gray-900 mb-2">Historique</h1>
      <p className="text-gray-500 mb-6">Commits, interactions IA et évaluations réels.</p>

      <div className="grid grid-cols-3 gap-4 mb-8">
        <div className="bg-white border rounded-2xl p-4">
          <div className="text-2xl font-bold">{data?.contributionsGit?.length ?? 0}</div>
          <div className="text-sm text-gray-500">Commits</div>
        </div>
        <div className="bg-white border rounded-2xl p-4">
          <div className="text-2xl font-bold">{data?.interactionsIa?.length ?? 0}</div>
          <div className="text-sm text-gray-500">Interactions IA</div>
        </div>
        <div className="bg-white border rounded-2xl p-4">
          <div className="text-2xl font-bold">{data?.evaluations?.length ?? 0}</div>
          <div className="text-sm text-gray-500">Évaluations</div>
        </div>
      </div>

      <div className="space-y-3">
        {timeline.length === 0 ? (
          <div className="p-8 border border-dashed rounded-2xl text-gray-400 text-center">
            Aucune activité pour le moment.
          </div>
        ) : (
          timeline.map((item, idx) => (
            <div key={idx} className="bg-white border rounded-2xl p-4">
              <div className="flex items-center gap-2 mb-1">
                <span className="text-[10px] font-bold px-2 py-0.5 rounded-full bg-slate-100 text-slate-600">
                  {item.kind}
                </span>
                <span className="text-xs text-gray-400">
                  {item.date ? new Date(item.date).toLocaleString() : ''}
                </span>
              </div>
              <div className="font-semibold text-gray-900 whitespace-pre-wrap">{item.title}</div>
              <div className="text-sm text-gray-500">{item.detail}</div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};

export default HistoryPage;
