
import { ChangeDetectionStrategy, Component, inject, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CartService } from '../../services/cart.service';
import { ApiService } from '../../services/api.service';
import { Order } from '../../models';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, ReactiveFormsModule]
})
export class CheckoutComponent {
  authService = inject(AuthService);
  cartService = inject(CartService);
  // FIX: Explicitly type `fb` as `FormBuilder` to fix type inference issue.
  private fb: FormBuilder = inject(FormBuilder);
  private apiService = inject(ApiService);
  private router = inject(Router);

  isProcessing = signal(false);
  paymentMethod = signal('credit-card');

  checkoutForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: ['', Validators.required],
    address: ['', Validators.required]
  });

  constructor() {
    effect(() => {
      const user = this.authService.currentUser();
      if (user) {
        this.checkoutForm.patchValue({
          name: user.name,
          email: user.email,
          address: user.address || ''
        });
      }
    });
  }
  
  setPaymentMethod(method: string) {
    this.paymentMethod.set(method);
  }

  placeOrder() {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }

    this.isProcessing.set(true);

    const orderData = {
        userId: this.authService.currentUser()?.id || null,
        items: this.cartService.cartItems(),
        total: this.cartService.total(),
        guestDetails: this.authService.isLoggedIn() ? null : this.checkoutForm.value
    };

    this.apiService.createOrder(orderData as Omit<Order, 'id' | 'date' | 'status'>).subscribe({
      next: (newOrder) => {
        this.cartService.clearCart();
        this.router.navigate(['/order-confirmation', newOrder.id]);
      },
      error: (err) => {
        console.error('Failed to create order:', err);
        // Here you would show an error message to the user
        this.isProcessing.set(false);
      },
      complete: () => {
        this.isProcessing.set(false);
      }
    });
  }
}
