
import { Routes } from '@angular/router';

export const ADMIN_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./admin-dashboard/admin-dashboard.component').then(c => c.AdminDashboardComponent),
        children: [
            { path: '', redirectTo: 'orders', pathMatch: 'full' },
            { path: 'orders', loadComponent: () => import('./manage-orders/manage-orders.component').then(c => c.ManageOrdersComponent), title: 'Manage Orders - Admin' },
            { path: 'pizzas', loadComponent: () => import('./manage-pizzas/manage-pizzas.component').then(c => c.ManagePizzasComponent), title: 'Manage Pizzas - Admin' }
        ]
    }
];
