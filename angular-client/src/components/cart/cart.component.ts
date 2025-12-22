import { Component, ChangeDetectionStrategy, input, output, inject, signal } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { NgOptimizedImage } from '@angular/common';
import { PaymentModalComponent } from '../payment-modal/payment-modal.component';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgOptimizedImage, PaymentModalComponent]
})
export class CartComponent {
  cartService = inject(CartService);
  cart = this.cartService.cart;

  isVisible = input.required<boolean>();
  closeCart = output<void>();
  checkoutSuccess = output<string>();
  
  isPaymentModalVisible = signal(false);

  onCloseCart() {
    this.closeCart.emit();
  }
  
  updateQuantity(itemId: string, quantity: number) {
    this.cartService.updateItemQuantity(itemId, quantity);
  }

  removeItem(itemId: string) {
    this.cartService.removeItem(itemId);
  }
  
  openPaymentModal() {
    if ((this.cart()?.items.length ?? 0) > 0) {
      this.isPaymentModalVisible.set(true);
    }
  }

  handlePaymentSuccess() {
    this.isPaymentModalVisible.set(false);
    this.cartService.clearCart();
    this.onCloseCart();
    this.checkoutSuccess.emit('Payment successful! Your order is on its way.');
  }
}
