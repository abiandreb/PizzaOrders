import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  template: `
    <header class="bg-white shadow-md sticky top-0 z-50">
      <nav class="container mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex items-center justify-between h-16">
          <div class="flex items-center">
            <a routerLink="/" class="flex-shrink-0 text-2xl font-bold text-blue-600">
              PizzaDash
            </a>
            <div class="hidden md:block">
              <div class="ml-10 flex items-baseline space-x-4">
                <a routerLink="/menu" routerLinkActive="text-blue-600 border-b-2 border-blue-600" [routerLinkActiveOptions]="{exact: true}" class="text-gray-500 hover:text-blue-600 px-3 py-2 text-sm font-medium">Menu</a>
                <a routerLink="/custom-pizza" routerLinkActive="text-blue-600 border-b-2 border-blue-600" class="text-gray-500 hover:text-blue-600 px-3 py-2 text-sm font-medium">Build Your Pizza</a>
                <a routerLink="/contact" routerLinkActive="text-blue-600 border-b-2 border-blue-600" class="text-gray-500 hover:text-blue-600 px-3 py-2 text-sm font-medium">Contact</a>
              </div>
            </div>
          </div>
          <div class="flex items-center">
            <a routerLink="/checkout" class="relative text-gray-500 hover:text-blue-600 p-2">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
                @if(cartService.itemCount() > 0) {
                    <span class="absolute top-0 right-0 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-red-100 transform translate-x-1/2 -translate-y-1/2 bg-red-600 rounded-full">{{ cartService.itemCount() }}</span>
                }
            </a>
            @if(authService.user(); as user) {
                <div class="ml-4 relative">
                    <button (click)="toggleDropdown()" class="flex items-center space-x-2 text-sm font-medium text-gray-500 hover:text-blue-600">
                        <span>{{ user.name }}</span>
                        <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                           <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
                        </svg>
                    </button>
                    @if(dropdownOpen) {
                        <div class="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5">
                            <a routerLink="/my-orders" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">My Orders</a>
                            @if(authService.isAdmin()){
                                <a routerLink="/admin" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">Admin Dashboard</a>
                            }
                            <a href="#" (click)="logout($event)" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">Logout</a>
                        </div>
                    }
                </div>
            } @else {
                 <a routerLink="/auth" class="ml-4 px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700">Login</a>
            }
          </div>
        </div>
      </nav>
    </header>
  `,
})
export class HeaderComponent {
  authService = inject(AuthService);
  cartService = inject(CartService);
  dropdownOpen = false;

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }
  
  logout(event: MouseEvent) {
    event.preventDefault();
    this.authService.logout();
    this.dropdownOpen = false;
  }
}
