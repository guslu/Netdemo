const TOKEN_KEY = 'netdemo.accessToken';
const ORG_KEY = 'netdemo.organizationId';

export const sessionStore = {
  getToken: (): string | null => localStorage.getItem(TOKEN_KEY),
  setToken: (value: string) => localStorage.setItem(TOKEN_KEY, value),
  clearToken: () => localStorage.removeItem(TOKEN_KEY),
  getOrganizationId: (): string | null => localStorage.getItem(ORG_KEY),
  setOrganizationId: (value: string) => localStorage.setItem(ORG_KEY, value),
  clearOrganizationId: () => localStorage.removeItem(ORG_KEY),
  clear: () => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(ORG_KEY);
  }
};
