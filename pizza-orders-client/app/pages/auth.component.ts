import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  template: `
    <div class="min-h-[60vh] flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div class="max-w-md w-full space-y-8 bg-white p-10 rounded-xl shadow-lg">
        <div>
          <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
            {{ isLoginView() ? 'Sign in to your account' : 'Create a new account' }}
          </h2>
        </div>
        <form *ngIf="isLoginView()" [formGroup]="loginForm" (ngSubmit)="onLogin()" class="mt-8 space-y-6">
          <div class="rounded-md shadow-sm -space-y-px">
            <div>
              <label for="login-email" class="sr-only">Email address</label>
              <input id="login-email" formControlName="email" type="email" required class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm" placeholder="Email address">
            </div>
            <div>
              <label for="login-password" class="sr-only">Password</label>
              <input id="login-password" formControlName="password" type="password" required class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm" placeholder="Password">
            </div>
          </div>
          <div>
            <button type="submit" [disabled]="loginForm.invalid || isProcessing()" class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:bg-gray-400">
              @if(isProcessing()) {<span class="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></span>} @else {Sign in}
            </button>
          </div>
        </form>

        <form *ngIf="!isLoginView()" [formGroup]="registerForm" (ngSubmit)="onRegister()" class="mt-8 space-y-6">
           <div class="rounded-md shadow-sm -space-y-px">
             <div>
              <label for="register-name" class="sr-only">Full Name</label>
              <input id="register-name" formControlName="name" type="text" required class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm" placeholder="Full Name">
            </div>
            <div>
              <label for="register-email" class="sr-only">Email address</label>
              <input id="register-email" formControlName="email" type="email" required class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm" placeholder="Email address">
            </div>
            <div>
              <label for="register-password" class="sr-only">Password</label>
              <input id="register-password" formControlName="password" type="password" required class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm" placeholder="Password">
            </div>
          </div>
          <div>
            <button type="submit" [disabled]="registerForm.invalid || isProcessing()" class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:bg-gray-400">
              @if(isProcessing()){<span class="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></span>} @else {Create account}
            </button>
          </div>
        </form>
        
        <div class="text-sm text-center">
            <a href="#" (click)="toggleView($event)" class="font-medium text-blue-600 hover:text-blue-500">
               {{ isLoginView() ? "Don't have an account? Sign up" : "Already have an account? Sign in" }}
            </a>
        </div>
      </div>
    </div>
  `,
})
export class AuthComponent {
  authService = inject(AuthService);
  fb = inject(FormBuilder);
  router = inject(Router);

  isLoginView = signal(true);
  isProcessing = signal(false);

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  registerForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });
  
  toggleView(event: MouseEvent) {
    event.preventDefault();
    this.isLoginView.set(!this.isLoginView());
  }

  async onLogin() {
    if (this.loginForm.invalid) return;
    this.isProcessing.set(true);
    const { email, password } = this.loginForm.value;
    await this.authService.login(email!, password!);
    this.isProcessing.set(false);
    this.router.navigate(['/']);
  }

  async onRegister() {
    if (this.registerForm.invalid) return;
    this.isProcessing.set(true);
    const { name, email, password } = this.registerForm.value;
    await this.authService.register(name!, email!, password!);
    this.isProcessing.set(false);
    this.router.navigate(['/']);
  }
}
