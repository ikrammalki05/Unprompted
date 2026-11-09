import { useEffect, useState } from 'react';
import {
  ProfileBanner,
  PersonalInfoCard,
  InstitutionCard,
  initialAdminData,
} from '../features/admin/profile';
import type { AdminData } from '../features/admin/profile';
import { api } from '../services/api';
import { useCurrentUserDisplay } from '../hooks/useCurrentUserDisplay';

export const ProfilePage = () => {
  const user = useCurrentUserDisplay();
  const [editing, setEditing] = useState(false);
  const [data, setData] = useState<AdminData>({
    ...initialAdminData,
    nom: user.fullName || initialAdminData.nom,
    email: user.email || initialAdminData.email,
  });
  const [draft, setDraft] = useState<AdminData>(data);
  const [saved, setSaved] = useState(false);

  useEffect(() => {
    const load = async () => {
      try {
        const res = await api.get('/Profil/me');
        const me = res.data || {};
        const next: AdminData = {
          nom: me.nomComplet || me.nom || user.fullName || initialAdminData.nom,
          email: me.email || user.email || initialAdminData.email,
          telephone: me.telephone || initialAdminData.telephone,
          role: me.role || 'Administration Centrale',
          institution: me.institution || initialAdminData.institution,
          localisation: me.localisation || initialAdminData.localisation,
        };
        setData(next);
        setDraft(next);
      } catch {
        const next = {
          ...initialAdminData,
          nom: user.fullName || initialAdminData.nom,
          email: user.email || initialAdminData.email,
        };
        setData(next);
        setDraft(next);
      }
    };
    void load();
  }, [user.fullName, user.email]);

  const handleEdit = () => {
    setDraft({ ...data });
    setEditing(true);
  };

  const handleSave = () => {
    setData({ ...draft });
    setEditing(false);
    setSaved(true);
    setTimeout(() => setSaved(false), 2500);
  };

  const handleCancel = () => {
    setEditing(false);
    setDraft({ ...data });
  };

  return (
    <div className="flex-1 p-6 md:p-8 overflow-y-auto w-full flex flex-col gap-6 max-w-6xl mx-auto">
      <ProfileBanner
        nom={data.nom}
        saved={saved}
        editing={editing}
        onEdit={handleEdit}
        onSave={handleSave}
        onCancel={handleCancel}
      />

      <div className="grid grid-cols-1 min-[1100px]:grid-cols-2 gap-6 pb-10">
        <PersonalInfoCard data={data} draft={draft} editing={editing} setDraft={setDraft} />
        <InstitutionCard data={data} draft={draft} editing={editing} setDraft={setDraft} />
      </div>
    </div>
  );
};
