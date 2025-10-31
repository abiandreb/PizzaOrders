
import { User } from '../types';

// This is a mock service. In a real app, you would make API calls to your backend.
// We're using localStorage to simulate JWT and refresh tokens.

const MOCK_USERS: User[] = [
    { id: '1', name: 'Admin User', email: 'admin@example.com', role: 'admin' },
    { id: '2', name: 'Regular User', email: 'user@example.com', role: 'user' },
];

const ACCESS_TOKEN_KEY = 'pizza_access_token';
const REFRESH_TOKEN_KEY = 'pizza_refresh_token';

const createToken = (user: User, expiresIn: number) => {
    const expiry = new Date().getTime() + expiresIn;
    return btoa(JSON.stringify({ user, expiry }));
};

export const authService = {
    login: async (email: string, pass: string): Promise<User> => {
        console.log(`Attempting login for ${email} with password ${pass}`);
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const user = MOCK_USERS.find(u => u.email === email);
                if (user) {
                    const accessToken = createToken(user, 15 * 60 * 1000); // 15 mins
                    const refreshToken = createToken(user, 7 * 24 * 60 * 60 * 1000); // 7 days
                    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
                    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
                    resolve(user);
                } else {
                    reject(new Error('Invalid credentials'));
                }
            }, 500);
        });
    },

    register: async (name: string, email: string, pass: string): Promise<User> => {
        console.log(`Attempting registration for ${name}, ${email} with password ${pass}`);
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                if (MOCK_USERS.some(u => u.email === email)) {
                    reject(new Error('User with this email already exists'));
                }
                const newUser: User = { id: String(MOCK_USERS.length + 1), name, email, role: 'user' };
                MOCK_USERS.push(newUser);

                const accessToken = createToken(newUser, 15 * 60 * 1000); // 15 mins
                const refreshToken = createToken(newUser, 7 * 24 * 60 * 60 * 1000); // 7 days
                localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
                localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
                resolve(newUser);
            }, 500);
        });
    },

    logout: () => {
        localStorage.removeItem(ACCESS_TOKEN_KEY);
        localStorage.removeItem(REFRESH_TOKEN_KEY);
    },

    getCurrentUser: async (): Promise<User> => {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                try {
                    const token = localStorage.getItem(ACCESS_TOKEN_KEY);
                    if (!token) return reject('No token');

                    const { user, expiry } = JSON.parse(atob(token));
                    if (new Date().getTime() > expiry) {
                        // In a real app, you'd use the refresh token here.
                        // For this mock, we'll just log out.
                        authService.logout();
                        return reject('Token expired');
                    }
                    resolve(user as User);
                } catch (e) {
                    reject('Invalid token');
                }
            }, 200);
        });
    },
    
    // Mock for the concept of an interceptor
    getAuthHeader: (): Record<string, string> => {
        const token = localStorage.getItem(ACCESS_TOKEN_KEY);
        if (token) {
            return { 'Authorization': `Bearer ${token}` };
        }
        return {};
    }
};
