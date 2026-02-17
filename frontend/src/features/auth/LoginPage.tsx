import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiClient } from '../../lib/apiClient';
import { sessionStore } from '../../lib/sessionStore';

type LoginResponse = {
  accessToken: string;
  organizationId: string;
};

export const LoginPage = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const onSubmit = async (event: FormEvent) => {
    event.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      const response = await apiClient.post<LoginResponse>('/api/v1/auth/login', { email, password });
      sessionStore.setToken(response.data.accessToken);
      sessionStore.setOrganizationId(response.data.organizationId);
      navigate('/projects');
    } catch {
      setError('Unable to sign in. Please verify your credentials.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <main className="mx-auto mt-24 max-w-md rounded border border-slate-700 p-6">
      <h1 className="mb-4 text-2xl font-semibold">Sign in</h1>
      <form className="space-y-4" onSubmit={onSubmit}>
        <input className="w-full rounded bg-slate-900 p-2" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />
        <input className="w-full rounded bg-slate-900 p-2" type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
        {error && <p className="text-sm text-red-300">{error}</p>}
        <button className="w-full rounded bg-sky-600 p-2 font-medium disabled:opacity-50" type="submit" disabled={isLoading}>
          {isLoading ? 'Signing in…' : 'Sign in'}
        </button>
      </form>
    </main>
  );
};
