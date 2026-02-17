import { Navigate } from 'react-router-dom';
import { sessionStore } from '../lib/sessionStore';

export const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  if (!sessionStore.getToken()) {
    return <Navigate to="/login" replace />;
  }

  return children;
};
