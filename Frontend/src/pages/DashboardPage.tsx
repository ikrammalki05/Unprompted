import { DashboardLayout } from '../layouts/DashboardLayout';
import { DashboardHeader, DashboardGrid } from '../features/admin';

export const DashboardPage = () => {
  return (
    <DashboardLayout>
      <DashboardHeader />
      <DashboardGrid />
    </DashboardLayout>
  );
};