import { Component, ChangeDetectionStrategy, signal, inject } from '@angular/core';
import { CartService } from './services/cart.service';
import { AuthService } from './services/auth.service';
import { HeaderComponent } from './components/header/header.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { CartComponent } from './components/cart/cart.component';
import { ProductDetailModalComponent } from './components/product-detail-modal/product-detail-modal.component';
import { Product } from './models/product.model';
import { AlertComponent } from './components/alert/alert.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [HeaderComponent, ProductListComponent, CartComponent, ProductDetailModalComponent, AlertComponent, AdminPanelComponent]
})
export class AppComponent {
  cartService = inject(CartService);
  authService = inject(AuthService);

  isCartVisible = signal(false);
  isDetailModalVisible = signal(false);
  isAdminPanelVisible = signal(false);
  selectedProduct = signal<Product | null>(null);
  alert = signal<{ message: string; type: 'success' | 'error' } | null>(null);

  constructor() {
    this.cartService.createCart();
  }

  toggleCartVisibility() {
    this.isCartVisible.update(visible => !visible);
  }

  toggleAdminPanel() {
    this.isAdminPanelVisible.update(visible => !visible);
  }

  handleProductSelect(product: Product) {
    this.selectedProduct.set(product);
    this.isDetailModalVisible.set(true);
  }

  closeDetailModal() {
    this.isDetailModalVisible.set(false);
    this.selectedProduct.set(null);
  }

  addItemAndCloseModal(item: { product: Product, quantity: number, toppings: number[] }) {
    this.cartService.addToCart(item.product, item.quantity, item.toppings);
    this.closeDetailModal();
    this.isCartVisible.set(true);
  }

  handleCheckoutSuccess(message: string) {
    this.alert.set({ message, type: 'success' });
  }

  handleAuthSuccess(message: string) {
    this.alert.set({ message, type: 'success' });
  }

  closeAlert() {
    this.alert.set(null);
  }
}
