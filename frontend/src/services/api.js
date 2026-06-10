import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

// Add auth token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const documentService = {
  // Upload a new document
  uploadDocument: (title, file) => {
    const formData = new FormData();
    formData.append('title', title);
    formData.append('file', file);
    return api.post('/documents', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
  },

  // Get all documents
  getAllDocuments: () => api.get('/documents'),

  // Get specific document
  getDocument: (id) => api.get(`/documents/${id}`),

  // Delete document
  deleteDocument: (id) => api.delete(`/documents/${id}`),

  // Download document
  downloadDocument: (id) => api.get(`/documents/${id}/file`, { responseType: 'blob' }),
};

export const summaryService = {
  // Generate summary
  generateSummary: (documentId, request) =>
    api.post(`/documents/${documentId}/summarize`, request),

  // Get summary
  getSummary: (documentId) => api.get(`/documents/${documentId}/summary`),

  // Update/regenerate summary
  updateSummary: (documentId, request) =>
    api.patch(`/documents/${documentId}/summary`, request),
};

export const authService = {
  // Login
  login: (email, password) => api.post('/auth/login', { email, password }),

  // Register
  register: (email, password) => api.post('/auth/register', { email, password }),

  // Logout
  logout: () => {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userId');
  },

  // Store auth token
  setToken: (token) => localStorage.setItem('authToken', token),

  // Get stored token
  getToken: () => localStorage.getItem('authToken'),
};
