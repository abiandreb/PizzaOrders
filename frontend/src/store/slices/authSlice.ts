import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import type { User } from '../../types';

interface AuthState {
  user: User | null;
  token: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
}

// Helper function to decode user from token
const getUserFromToken = (token: string | null): User | null => {
  if (!token) return null;

  try {
    const tokenPayload = JSON.parse(atob(token.split('.')[1]));

    // Don't check expiration on initial load - let the axios interceptor handle token refresh
    // If token is expired, the API will return 401 and trigger refresh token flow
    // Only clear tokens if they are malformed or refresh fails (handled in api.ts)

    return {
      email: tokenPayload.email || tokenPayload.sub,
      roles: tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
        ? (Array.isArray(tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
            ? tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
            : [tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']])
        : [],
    };
  } catch (error) {
    console.error('Failed to decode token:', error);
    // Only clear if token is malformed (can't be decoded)
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    return null;
  }
};

const storedToken = localStorage.getItem('token');
const decodedUser = getUserFromToken(storedToken);

const initialState: AuthState = {
  user: decodedUser,
  token: decodedUser ? storedToken : null,
  refreshToken: decodedUser ? localStorage.getItem('refreshToken') : null,
  isAuthenticated: !!decodedUser,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setCredentials: (
      state,
      action: PayloadAction<{ user: User; token: string; refreshToken: string }>
    ) => {
      state.user = action.payload.user;
      state.token = action.payload.token;
      state.refreshToken = action.payload.refreshToken;
      state.isAuthenticated = true;
      localStorage.setItem('token', action.payload.token);
      localStorage.setItem('refreshToken', action.payload.refreshToken);
    },
    logout: (state) => {
      state.user = null;
      state.token = null;
      state.refreshToken = null;
      state.isAuthenticated = false;
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
    },
    setUser: (state, action: PayloadAction<User>) => {
      state.user = action.payload;
    },
  },
});

export const { setCredentials, logout, setUser } = authSlice.actions;
export default authSlice.reducer;
