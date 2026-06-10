import { useState, useEffect, useCallback } from 'react';
import { authService } from '../services/api';

export function useAuth() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Check if user is already logged in
    const token = authService.getToken();
    setIsAuthenticated(!!token);
    setLoading(false);
  }, []);

  const logout = useCallback(() => {
    authService.logout();
    setIsAuthenticated(false);
  }, []);

  const login = useCallback(() => {
    setIsAuthenticated(true);
  }, []);

  return {
    isAuthenticated,
    loading,
    login,
    logout,
  };
}
