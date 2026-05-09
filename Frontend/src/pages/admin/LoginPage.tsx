import { AuthLayout } from '../../layouts/admin/AuthLayout';
import { LoginForm } from '../../features/auth/components/LoginForm';

export const LoginPage = () => {
  return (
    <AuthLayout>
      <LoginForm />
    </AuthLayout>
  );
};