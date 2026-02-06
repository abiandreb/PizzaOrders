import { useCallback } from 'react';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { setCredentials, logout as logoutAction } from '../store/slices/authSlice';
import { api } from '../services/api';
import type { LoginRequest, RegisterRequest } from '../types';

export const useAuth = () => {
  const dispatch = useAppDispatch();
  const { user, isAuthenticated } = useAppSelector((state) => state.auth);

  const login = useCallback(
    async (credentials: LoginRequest) => {
      const response = await api.login(credentials);

      // Decode JWT to get user info
      const tokenPayload = JSON.parse(atob(response.token.split('.')[1]));
      const userObj = {
        email: tokenPayload.email || tokenPayload.sub,
        roles: tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
          ? (Array.isArray(tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
              ? tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
              : [tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']])
          : [],
      };

      dispatch(
        setCredentials({
          user: userObj,
          token: response.token,
          refreshToken: response.refreshToken,
        })
      );

      return response;
    },
    [dispatch]
  );

  const register = useCallback(
    async (data: RegisterRequest) => {
      const response = await api.register(data);

      // Decode JWT to get user info
      const tokenPayload = JSON.parse(atob(response.token.split('.')[1]));
      const userObj = {
        email: tokenPayload.email || tokenPayload.sub,
        roles: tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
          ? (Array.isArray(tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
              ? tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
              : [tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']])
          : [],
      };

      dispatch(
        setCredentials({
          user: userObj,
          token: response.token,
          refreshToken: response.refreshToken,
        })
      );

      return response;
    },
    [dispatch]
  );

  const logout = useCallback(async () => {
    try {
      // Revoke refresh tokens on server if user is authenticated
      if (localStorage.getItem('token')) {
        await api.logout();
      }
    } catch (error) {
      // Even if server logout fails, still clear local state
      console.error('Server logout failed:', error);
    }
    dispatch(logoutAction());
  }, [dispatch]);

  const isAdmin = user?.roles?.includes('Admin') ?? false;

  return {
    user,
    isAuthenticated,
    isAdmin,
    login,
    register,
    logout,
  };
};
