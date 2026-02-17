import axios from 'axios';
import { sessionStore } from './sessionStore';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 10000
});

apiClient.interceptors.request.use((config) => {
  const token = sessionStore.getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
