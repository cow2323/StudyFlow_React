import {Navigate, Outlet} from 'react-router-dom';
import {useAuth} from './AuthContext';

export default function AdminRoute(){
    const {token, isAdmin, isLoading} = useAuth();
    if (isLoading) return null; 
    if (!token) return <Navigate to="/login" replace/>
    if (!isAdmin) return <Navigate to="/" replace />
    return <Outlet />;
}