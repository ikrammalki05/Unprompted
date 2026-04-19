import { AdminHeader, RecentActivity, StatCards } from '../features/admin';
import { Topbar } from '../features/admin/components/Topbar';



export const DashboardPage = () => {
  return (
    <>
      <div className="m-7">
    
        <AdminHeader />
        <StatCards />
        <RecentActivity />
      </div>
    </>
  );
};