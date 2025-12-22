import { Component, ChangeDetectionStrategy, output, inject, signal } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';
import { LoginModalComponent } from '../login-modal/login-modal.component';
import { SignupModalComponent } from '../signup-modal/signup-modal.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [LoginModalComponent, SignupModalComponent]
})
export class HeaderComponent {
  cartClick = output<void>();
  adminPanelClick = output<void>();
  authSuccess = output<string>();

  cartService = inject(CartService);
  authService = inject(AuthService);
  
  totalItems = this.cartService.totalItems;
  currentUser = this.authService.currentUser;
  isAdmin = this.authService.isAdmin;

  isLoginModalVisible = signal(false);
  isSignupModalVisible = signal(false);

  onCartClick() {
    this.cartClick.emit();
  }

  logout() {
    this.authService.logout();
    this.authSuccess.emit('You have been successfully logged out.');
  }

  handleLoginSuccess() {
    this.isLoginModalVisible.set(false);
    this.authSuccess.emit(`Welcome back, ${this.currentUser()?.name}!`);
  }
  
  handleSignupSuccess() {
    this.isSignupModalVisible.set(false);
    this.authSuccess.emit(`Welcome, ${this.currentUser()?.name}! Your account is ready.`);
  }
}
