import { Navigate } from 'react-router-dom';
import { tokenStore } from '../lib/tokenStore';

export const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  if (!tokenStore.get()) {
    return <Navigate to="/login" replace />;
  }

  return children;
};
