import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-order-confirmation',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-24 text-center">
        <div class="max-w-md mx-auto bg-white p-8 rounded-lg shadow-lg">
            <svg class="w-16 h-16 mx-auto text-green-500" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
            <h1 class="text-3xl font-bold text-gray-800 mt-4">Thank You for Your Order!</h1>
            <p class="text-gray-600 mt-2">Your order has been placed successfully. You can track its status in your "My Orders" page.</p>
            <p class="mt-4 font-semibold">Order ID: <span class="text-blue-600">{{ orderId }}</span></p>
            <div class="mt-8">
                <a routerLink="/my-orders" class="inline-block bg-blue-600 text-white font-bold py-3 px-8 rounded-full text-lg hover:bg-blue-700 transition duration-300">
                    View My Orders
                </a>
            </div>
        </div>
    </div>
  `,
})
export class OrderConfirmationComponent {
  route = inject(ActivatedRoute);
  orderId: string | null = this.route.snapshot.paramMap.get('id');
}
