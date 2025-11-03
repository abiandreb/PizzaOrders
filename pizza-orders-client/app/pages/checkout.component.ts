import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CartService } from '../services/cart.service';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';
import { Order } from '../types';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  template: `
    <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <h1 class="text-4xl font-bold text-center mb-8">Checkout</h1>
      
      @if(cartService.items().length > 0) {
        <div class="grid lg:grid-cols-5 gap-12">
          <!-- Form -->
          <div class="lg:col-span-3">
            <form [formGroup]="checkoutForm" (ngSubmit)="placeOrder()" class="bg-white p-8 rounded-lg shadow-lg">
              <h2 class="text-2xl font-semibold mb-6">Delivery Details</h2>
              <div class="space-y-4">
                 <div>
                    <label for="name" class="block text-sm font-medium text-gray-700">Full Name</label>
                    <input type="text" id="name" formControlName="name" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm">
                 </div>
                 <div>
                    <label for="address" class="block text-sm font-medium text-gray-700">Address</label>
                    <input type="text" id="address" formControlName="address" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm">
                 </div>
                 <div>
                    <label for="phone" class="block text-sm font-medium text-gray-700">Phone Number</label>
                    <input type="tel" id="phone" formControlName="phone" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm">
                 </div>
              </div>

              <div class="mt-8">
                 <button type="submit" [disabled]="checkoutForm.invalid || isProcessing()" class="w-full bg-blue-600 text-white font-bold py-3 rounded-md hover:bg-blue-700 disabled:bg-gray-400 transition duration-300 flex items-center justify-center">
                    @if(isProcessing()) {
                        <div class="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></div>
                        <span class="ml-2">Processing...</span>
                    } @else {
                       <span>Place Order</span>
                    }
                 </button>
              </div>
            </form>
          </div>

          <!-- Order Summary -->
          <div class="lg:col-span-2">
            <div class="bg-white p-6 rounded-lg shadow-lg">
               <h2 class="text-2xl font-semibold mb-4">Order Summary</h2>
                @for(item of cartService.items(); track item.product.id) {
                    <div class="flex justify-between items-center py-2 border-b">
                       <div>
                          <p class="font-medium">{{ item.product.name }}</p>
                          <p class="text-sm text-gray-500">Qty: {{ item.quantity }}</p>
                       </div>
                       <p class="font-medium">\${{ (item.product.price * item.quantity).toFixed(2) }}</p>
                    </div>
                }
               <div class="space-y-2 mt-4">
                  <div class="flex justify-between">
                     <p>Subtotal</p>
                     <p>\${{ cartService.subtotal().toFixed(2) }}</p>
                  </div>
                  @if(cartService.discount() > 0) {
                     <div class="flex justify-between text-green-600">
                        <p>Discount ({{ cartService.promoCode() }})</p>
                        <p>-\${{ cartService.discount().toFixed(2) }}</p>
                     </div>
                  }
                  <div class="flex justify-between font-bold text-xl pt-2 border-t">
                     <p>Total</p>
                     <p>\${{ cartService.total().toFixed(2) }}</p>
                  </div>
               </div>
               
               <!-- Promo Code -->
               <div class="mt-6">
                    <label for="promo" class="block text-sm font-medium text-gray-700">Promo Code</label>
                    <div class="mt-1 flex rounded-md shadow-sm">
                        <input type="text" #promoInput id="promo" class="block w-full rounded-l-md border-gray-300 focus:border-blue-500 focus:ring-blue-500 sm:text-sm">
                        <button (click)="applyPromo(promoInput.value)" class="bg-gray-200 px-4 py-2 rounded-r-md text-sm font-medium text-gray-700 hover:bg-gray-300">Apply</button>
                    </div>
               </div>

            </div>
          </div>
        </div>
      } @else {
        <div class="text-center">
            <h2 class="text-2xl font-semibold">Your cart is empty.</h2>
            <p class="text-gray-500 mt-2">Add some delicious pizza to your cart from our menu!</p>
            <a routerLink="/menu" class="mt-6 inline-block bg-blue-600 text-white font-bold py-3 px-8 rounded-full text-lg hover:bg-blue-700 transition duration-300">
                Go to Menu
            </a>
        </div>
      }
    </div>
  `,
})
export class CheckoutComponent {
  cartService = inject(CartService);
  apiService = inject(ApiService);
  authService = inject(AuthService);
  fb = inject(FormBuilder);
  router = inject(Router);

  isProcessing = signal(false);

  checkoutForm = this.fb.group({
    name: [this.authService.user()?.name || '', Validators.required],
    address: ['', Validators.required],
    phone: ['', [Validators.required, Validators.pattern('^[0-9]{10,15}$')]],
  });

  applyPromo(code: string) {
    this.cartService.applyPromoCode(code);
  }

  placeOrder() {
    if (this.checkoutForm.invalid || !this.authService.user()) return;

    this.isProcessing.set(true);
    
    const orderData: Omit<Order, 'id' | 'orderDate' | 'status'> = {
        userId: this.authService.user()!.id,
        items: this.cartService.items(),
        subtotal: this.cartService.subtotal(),
        discount: this.cartService.discount(),
        total: this.cartService.total(),
        deliveryAddress: this.checkoutForm.value
    };

    this.apiService.placeOrder(orderData).subscribe(newOrder => {
        this.isProcessing.set(false);
        this.cartService.clearCart();
        this.router.navigate(['/order-confirmation', newOrder.id]);
    });
  }
}
