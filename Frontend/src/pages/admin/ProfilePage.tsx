import { useState } from 'react';
import { 
  ProfileBanner, 
  PersonalInfoCard, 
  InstitutionCard, 
  initialAdminData
} from '../../features/admin/profile';
import type {AdminData} from '../../features/admin/profile';


export const ProfilePage = () => {
  const [editing, setEditing] = useState(false);
  const [data, setData] = useState<AdminData>(initialAdminData);
  const [draft, setDraft] = useState<AdminData>(initialAdminData);
  const [saved, setSaved] = useState(false);

  const handleEdit = () => {
    setDraft({ ...data });
    setEditing(true);
  };

  

  const handleSave = () => {
    // Ici, tu pourras ajouter un appel axios (api.put('/profile', draft)) plus tard
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
    <div className="flex-1 p-[28px_40px] overflow-y-auto w-full flex flex-col gap-6">
      
      <ProfileBanner 
        nom={data.nom} 
        saved={saved} 
        editing={editing} 
        onEdit={handleEdit} 
        onSave={handleSave} 
        onCancel={handleCancel} 
      />

      <div className="grid grid-cols-1 min-[1100px]:grid-cols-2 gap-6 pb-10">
        <PersonalInfoCard 
          data={data} 
          draft={draft} 
          editing={editing} 
          setDraft={setDraft} 
        />
        <InstitutionCard 
          data={data} 
          draft={draft} 
          editing={editing} 
          setDraft={setDraft} 
        />
      </div>

    </div>
  );
}