import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiClient } from '../../lib/apiClient';
import { sessionStore } from '../../lib/sessionStore';

type Project = {
  id: string;
  name: string;
  description?: string;
};

type WorkItem = {
  id: string;
  title: string;
  isCompleted: boolean;
};

export const ProjectsPage = () => {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProjectId, setSelectedProjectId] = useState<string | null>(null);
  const [workItems, setWorkItems] = useState<WorkItem[]>([]);

  useEffect(() => {
    const loadProjects = async () => {
      const organizationId = sessionStore.getOrganizationId();
      if (!organizationId) {
        return;
      }

      const response = await apiClient.get<Project[]>(`/api/v1/projects/organization/${organizationId}`);
      setProjects(response.data);
    };

    void loadProjects();
  }, []);

  useEffect(() => {
    const loadWorkItems = async () => {
      if (!selectedProjectId) {
        return;
      }

      const response = await apiClient.get<WorkItem[]>(`/api/v1/projects/${selectedProjectId}/work-items`);
      setWorkItems(response.data);
    };

    void loadWorkItems();
  }, [selectedProjectId]);

  const logout = () => {
    sessionStore.clear();
    navigate('/login');
  };

  return (
    <main className="mx-auto mt-10 grid max-w-5xl grid-cols-2 gap-6">
      <section>
        <div className="mb-4 flex items-center justify-between">
          <h1 className="text-3xl font-bold">Projects</h1>
          <button className="rounded border border-slate-600 px-3 py-1" onClick={logout}>Logout</button>
        </div>
        <ul className="space-y-3">
          {projects.map((project) => (
            <li key={project.id}>
              <button className="w-full rounded border border-slate-700 p-3 text-left" onClick={() => setSelectedProjectId(project.id)}>
                <p className="font-medium">{project.name}</p>
                <p className="text-sm text-slate-300">{project.description ?? 'No description'}</p>
              </button>
            </li>
          ))}
        </ul>
      </section>
      <section>
        <h2 className="mb-4 text-xl font-semibold">Work items</h2>
        {selectedProjectId ? (
          <ul className="space-y-2">
            {workItems.map((item) => (
              <li key={item.id} className="rounded border border-slate-700 p-3">
                <p>{item.title}</p>
                <p className="text-sm text-slate-300">{item.isCompleted ? 'Completed' : 'Open'}</p>
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-slate-300">Select a project to view work items.</p>
        )}
      </section>
    </main>
  );
};
