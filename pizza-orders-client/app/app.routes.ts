import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home.component';
import { MenuComponent } from './pages/menu.component';
import { PizzaConstructorComponent } from './pages/pizza-constructor.component';
import { CheckoutComponent } from './pages/checkout.component';
import { OrderConfirmationComponent } from './pages/order-confirmation.component';
import { AuthComponent } from './pages/auth.component';
import { MyOrdersComponent } from './pages/my-orders.component';
import { AdminDashboardComponent } from './pages/admin-dashboard.component';
import { ContactComponent } from './pages/contact.component';
import { NotFoundComponent } from './pages/not-found.component';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'menu', component: MenuComponent },
  { path: 'custom-pizza', component: PizzaConstructorComponent },
  { path: 'checkout', component: CheckoutComponent, canActivate: [authGuard] },
  { path: 'order-confirmation/:id', component: OrderConfirmationComponent, canActivate: [authGuard] },
  { path: 'auth', component: AuthComponent },
  { path: 'my-orders', component: MyOrdersComponent, canActivate: [authGuard] },
  { path: 'admin', component: AdminDashboardComponent, canActivate: [authGuard, adminGuard] },
  { path: 'contact', component: ContactComponent },
  { path: '**', component: NotFoundComponent }
];
