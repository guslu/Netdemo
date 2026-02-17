import { Navigate, Route, Routes } from 'react-router-dom';
import { LoginPage } from '../features/auth/LoginPage';
import { ProjectsPage } from '../features/projects/ProjectsPage';
import { ProtectedRoute } from '../routes/ProtectedRoute';

export const App = () => (
  <Routes>
    <Route path="/login" element={<LoginPage />} />
    <Route
      path="/projects"
      element={
        <ProtectedRoute>
          <ProjectsPage />
        </ProtectedRoute>
      }
    />
    <Route path="*" element={<Navigate to="/projects" replace />} />
  </Routes>
);
