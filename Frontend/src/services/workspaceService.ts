import { api } from './api';

export interface TreeNode {
  id: number;
  name: string;
  type: 'folder' | 'file' | string;
  children?: TreeNode[];
}

export interface FichierDto {
  id: number;
  nom: string;
  extension: string;
  contenu: string;
  version?: number;
  derniereModification?: string;
}

export interface AiAskResponse {
  response: string;
  tokensUsed: number;
  dependencyScore: number;
  status: string;
  accessLevel: string;
}

export interface ExecutionStatus {
  idExecution: string;
  statut: string;
  sortie?: string;
}

const workspaceService = {
  getTree: async (projectId: number) => {
    const res = await api.get<TreeNode[]>(`/projects/${projectId}/tree`);
    return res.data;
  },

  getFile: async (fileId: number) => {
    const res = await api.get<FichierDto>(`/fichier/${fileId}`);
    return res.data;
  },

  autosave: async (fileId: number, contenu: string) => {
    await api.post(`/fichier/${fileId}/autosave`, { contenu });
  },

  createFile: async (payload: {
    nom: string;
    extension: string;
    contenu?: string;
    idProjet: number;
    idDossier?: number | null;
  }) => {
    const res = await api.post<FichierDto>('/fichier', {
      nom: payload.nom,
      extension: payload.extension,
      contenu: payload.contenu ?? '',
      idProjet: payload.idProjet,
      idDossier: payload.idDossier ?? null,
    });
    return res.data;
  },

  createFolder: async (nom: string, idProjet: number, parentId?: number | null) => {
    const res = await api.post('/dossier', {
      nom,
      idProjet,
      dossierParentId: parentId ?? null,
    });
    return res.data;
  },

  run: async (langage: string, fichiers: { nom: string; contenu: string }[]) => {
    const res = await api.post<{ idExecution: string; statut: string }>('/execution', {
      langage,
      fichiers,
    });
    return res.data;
  },

  getExecution: async (idExecution: string) => {
    const res = await api.get<ExecutionStatus>(`/execution/${idExecution}`);
    return res.data;
  },

  createTerminalSession: async (idProjet?: number) => {
    const res = await api.post<{ idSession: string }>(
      '/terminal/session',
      null,
      { params: idProjet ? { idProjet } : undefined }
    );
    return res.data;
  },

  runTerminalCommand: async (idSession: string, commande: string) => {
    const res = await api.post<{ sortie: string }>(`/terminal/${idSession}/command`, { commande });
    return res.data;
  },

  closeTerminalSession: async (idSession: string) => {
    await api.delete(`/terminal/${idSession}`);
  },

  askAi: async (idProjet: number, message: string) => {
    const res = await api.post<AiAskResponse>('/ai/ask', { idProjet, message });
    return res.data;
  },

  commit: async (idProjet: number, messageCommit: string, lignesAjoutees?: number, lignesSupprimees?: number) => {
    const res = await api.post(`/projet/${idProjet}/contributions`, {
      messageCommit,
      lignesAjoutees: lignesAjoutees ?? 0,
      lignesSupprimees: lignesSupprimees ?? 0,
    });
    return res.data;
  },

  getProject: async (id: number) => {
    const res = await api.get(`/projet/${id}`);
    return res.data;
  },
};

export default workspaceService;
