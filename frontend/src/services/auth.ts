import type { User } from '../types';

export const getToken = (): string | null => {
  return localStorage.getItem('token');
};

export const getRefreshToken = (): string | null => {
  return localStorage.getItem('refreshToken');
};

export const setTokens = (token: string, refreshToken: string): void => {
  localStorage.setItem('token', token);
  localStorage.setItem('refreshToken', refreshToken);
};

export const clearTokens = (): void => {
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
};

export const decodeToken = (token: string): User | null => {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    const payload = JSON.parse(jsonPayload);

    return {
      email: payload.email || payload.sub,
      roles: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
        ? (Array.isArray(payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
            ? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
            : [payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']])
        : []
    };
  } catch {
    return null;
  }
};

export const isTokenExpired = (token: string): boolean => {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    const payload = JSON.parse(jsonPayload);

    if (!payload.exp) return true;

    return Date.now() >= payload.exp * 1000;
  } catch {
    return true;
  }
};

export const getCurrentUser = (): User | null => {
  const token = getToken();
  if (!token || isTokenExpired(token)) {
    return null;
  }
  return decodeToken(token);
};

export const isAdmin = (user: User | null): boolean => {
  return user?.roles?.includes('Admin') || false;
};
