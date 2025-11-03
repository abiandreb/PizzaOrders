import { Injectable, signal, computed, effect } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../types';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private router = new Router();
  user = signal<User | null>(null);
  isAdmin = computed(() => this.user()?.role === 'admin');

  constructor() {
    this.initAuthFromLocalStorage();
    effect(() => {
      const user = this.user();
      if (user) {
        localStorage.setItem('pizzaUser', JSON.stringify(user));
      } else {
        localStorage.removeItem('pizzaUser');
      }
    });
  }

  private initAuthFromLocalStorage() {
    if (typeof window !== 'undefined') {
      const userJson = localStorage.getItem('pizzaUser');
      if (userJson) {
        this.user.set(JSON.parse(userJson));
      }
    }
  }

  login(email: string, _password: string): Promise<void> {
    return new Promise((resolve) => {
        setTimeout(() => {
            const role = email.includes('admin') ? 'admin' : 'user';
            const newUser: User = {
              id: role === 'admin' ? 'admin1' : `usr${Date.now()}`,
              name: email.split('@')[0],
              email,
              role,
            };
            this.user.set(newUser);
            resolve();
        }, 1000);
    });
  }
  
  register(name: string, email: string, _password: string): Promise<void> {
     return new Promise((resolve) => {
        setTimeout(() => {
            const role = email.includes('admin') ? 'admin' : 'user';
            const newUser: User = {
              id: role === 'admin' ? 'admin1' : `usr${Date.now()}`,
              name,
              email,
              role,
            };
            this.user.set(newUser);
            resolve();
        }, 1000);
    });
  }

  logout() {
    this.user.set(null);
    this.router.navigate(['/auth']);
  }
}
