import React, { createContext, useState, useEffect } from 'react';
import type { ReactNode } from 'react';
import type { User, LoginRequest, RegisterRequest } from '../types';
import { api } from '../services/api';
import {
  getCurrentUser,
  setTokens,
  clearTokens,
  isAdmin as checkIsAdmin,
} from '../services/auth';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isAdmin: boolean;
  loading: boolean;
  login: (data: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const currentUser = getCurrentUser();
    setUser(currentUser);
    setLoading(false);
  }, []);

  const login = async (data: LoginRequest) => {
    const response = await api.login(data);
    setTokens(response.token, response.refreshToken);
    const currentUser = getCurrentUser();
    setUser(currentUser);
  };

  const register = async (data: RegisterRequest) => {
    const response = await api.register(data);
    setTokens(response.token, response.refreshToken);
    const currentUser = getCurrentUser();
    setUser(currentUser);
  };

  const logout = () => {
    clearTokens();
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        isAdmin: checkIsAdmin(user),
        loading,
        login,
        register,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
