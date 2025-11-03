import { Injectable, signal, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { User, AuthResponse } from '../models';
import { of, Observable, delay, tap, map, catchError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private http = inject(HttpClient);
    private router = inject(Router);
    private baseUrl = 'https://localhost:7258/api/Auth'; // TODO: Move to environment variable

    currentUser = signal<User | null>(this.loadUserFromStorage());
    
    isLoggedIn = computed(() => this.currentUser() !== null);
    isAdmin = computed(() => this.currentUser()?.roles.includes('admin') ?? false);

    private loadUserFromStorage(): User | null {
        if (typeof window !== 'undefined') {
            const token = window.localStorage.getItem('token');
            if (token) {
                // TODO: Decode token to get user info
                // For now, returning a placeholder
                return { id: '1', name: 'User', email: 'user@example.com', roles: ['user'] };
            }
            return null;
        }
        return null;
    }

    private saveTokenToStorage(token: string | null) {
        if (typeof window !== 'undefined') {
            if (token) {
                window.localStorage.setItem('token', token);
            } else {
                window.localStorage.removeItem('token');
            }
        }
    }

    login(email: string, password: string): Observable<{ success: boolean; message: string }> {
        return this.http.post<AuthResponse>(`${this.baseUrl}/login-user`, { email, password }).pipe(
            tap(response => {
                this.saveTokenToStorage(response.token);
                // TODO: Decode token to get user info
                const user: User = { id: '1', name: email, email: email, roles: ['user'] }; // Placeholder
                this.currentUser.set(user);
                this.router.navigateByUrl(this.isAdmin() ? '/admin' : '/order-history');
            }),
            map(() => ({ success: true, message: 'Login successful' })),
            catchError(error => {
                return of({ success: false, message: error.error?.message || 'Invalid credentials' });
            })
        );
    }
    
    register(userData: Omit<User, 'id' | 'roles'>): Observable<{ success: boolean; message: string }> {
        const { name, email, password, address } = userData;
        return this.http.post(`${this.baseUrl}/register-user`, { userName: name, email, password, address }).pipe(
            tap(() => this.router.navigate(['/registration-confirmation'])),
            map(() => ({ success: true, message: 'Registration successful' })),
            catchError(error => {
                return of({ success: false, message: error.error?.message || 'Registration failed' });
            })
        );
    }

    logout() {
        this.currentUser.set(null);
        this.saveTokenToStorage(null);
        this.router.navigate(['/login']);
    }
}
