import { AuthLayout } from '../layouts'; 
import { LoginForm } from '../features/auth/LoginForm';

export const LoginPage = () => {
    return (
        <AuthLayout>
            <LoginForm />
        </AuthLayout>
    );
};