import { Injectable, signal, computed } from '@angular/core';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  currentUser = signal<User | null>(null);
  
  isAdmin = computed(() => this.currentUser()?.role === 'Admin');

  async login(email: string, password?: string): Promise<User> {
    // Mock API call
    await new Promise(resolve => setTimeout(resolve, 1000));

    if (email.toLowerCase() === 'admin@pizzaria.com') {
      const adminUser: User = { name: 'Admin User', email, role: 'Admin' };
      this.currentUser.set(adminUser);
      return adminUser;
    }
    
    const regularUser: User = { name: 'John Doe', email, role: 'User' };
    this.currentUser.set(regularUser);
    return regularUser;
  }

  async signUp(details: { name: string, email: string, password?: string }): Promise<User> {
     // Mock API call
    await new Promise(resolve => setTimeout(resolve, 1000));
    const newUser: User = { name: details.name, email: details.email, role: 'User' };
    this.currentUser.set(newUser);
    return newUser;
  }

  logout() {
    this.currentUser.set(null);
  }
}
