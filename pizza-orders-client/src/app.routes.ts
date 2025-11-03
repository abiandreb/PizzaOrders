import { Routes } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

const adminGuard = () => {
    const authService = inject(AuthService);
    const router = inject(Router);
    if (authService.isAdmin()) {
        return true;
    }
    // Redirect to the login page
    return router.parseUrl('/login');
};

const authGuard = () => {
    const authService = inject(AuthService);
    const router = inject(Router);
    if (authService.isLoggedIn()) {
        return true;
    }
    // Redirect to the login page
    return router.parseUrl('/login');
};


export const APP_ROUTES: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        loadComponent: () => import('./pages/home/home.component').then(c => c.HomeComponent),
        title: 'Home - Pizza Planet'
    },
    {
        path: 'menu',
        loadComponent: () => import('./pages/menu/menu.component').then(c => c.MenuComponent),
        title: 'Our Menu - Pizza Planet'
    },
    // FIX: Add route for pizza detail page.
    {
        path: 'menu/:id',
        loadComponent: () => import('./pages/pizza-detail/pizza-detail.component').then(c => c.PizzaDetailComponent),
        title: 'Pizza Details - Pizza Planet'
    },
    {
        path: 'build-your-own',
        loadComponent: () => import('./pages/pizza-builder/pizza-builder.component').then(c => c.PizzaBuilderComponent),
        title: 'Build Your Own Pizza - Pizza Planet'
    },
    {
        path: 'cart',
        loadComponent: () => import('./pages/cart/cart.component').then(c => c.CartComponent),
        title: 'Your Cart - Pizza Planet'
    },
    {
        path: 'checkout',
        loadComponent: () => import('./pages/checkout/checkout.component').then(c => c.CheckoutComponent),
        title: 'Checkout - Pizza Planet'
    },
    {
        path: 'order-confirmation/:id',
        loadComponent: () => import('./pages/order-confirmation/order-confirmation.component').then(c => c.OrderConfirmationComponent),
        title: 'Order Confirmed! - Pizza Planet'
    },
    {
        path: 'order-history',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/order-history/order-history.component').then(c => c.OrderHistoryComponent),
        title: 'Order History - Pizza Planet'
    },
    {
        path: 'contact',
        loadComponent: () => import('./pages/contact/contact.component').then(c => c.ContactComponent),
        title: 'Contact Us - Pizza Planet'
    },
    {
        path: 'login',
        loadComponent: () => import('./pages/login/login.component').then(c => c.LoginComponent),
        title: 'Login - Pizza Planet'
    },
    {
        path: 'register',
        loadComponent: () => import('./pages/register/register.component').then(c => c.RegisterComponent),
        title: 'Register - Pizza Planet'
    },
    {
        path: 'registration-confirmation',
        loadComponent: () => import('./pages/registration-confirmation/registration-confirmation.component').then(c => c.RegistrationConfirmationComponent),
        title: 'Registration Successful - Pizza Planet'
    },
    {
        path: 'admin',
        canActivate: [adminGuard],
        loadChildren: () => import('./pages/admin/admin.routes').then(r => r.ADMIN_ROUTES),
    },
    {
        path: '**',
        redirectTo: 'home'
    }
];
