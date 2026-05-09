import { AdminHeader, RecentActivity, StatCards } from '../../features/admin/dash';



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