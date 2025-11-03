import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';
import { Order } from '../types';
import { LoadingSpinnerComponent } from '../components/loading-spinner.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, DatePipe, LoadingSpinnerComponent, RouterLink],
  template: `
    <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <h1 class="text-4xl font-bold mb-8">My Orders</h1>
      
      @if(loading()) {
        <app-loading-spinner></app-loading-spinner>
      } @else if(orders().length > 0) {
        <div class="space-y-6">
            @for(order of orders(); track order.id) {
                <div class="bg-white p-6 rounded-lg shadow-md">
                   <div class="flex flex-wrap justify-between items-start border-b pb-4 mb-4">
                        <div>
                            <p class="font-semibold">Order ID: <span class="text-blue-600">{{ order.id }}</span></p>
                            <p class="text-sm text-gray-500">Placed on: {{ order.orderDate | date: 'medium' }}</p>
                        </div>
                        <div>
                            <p class="font-semibold">Total: <span class="text-lg">\${{ order.total.toFixed(2) }}</span></p>
                            <p class="text-sm text-right font-medium" [ngClass]="getStatusColor(order.status)">{{ order.status }}</p>
                        </div>
                   </div>
                   <div>
                        <h3 class="font-semibold mb-2">Items:</h3>
                        <ul class="space-y-1">
                            @for(item of order.items; track item.product.id) {
                                <li>{{ item.quantity }} x {{ item.product.name }}</li>
                            }
                        </ul>
                   </div>
                </div>
            }
        </div>
      } @else {
         <div class="text-center py-16">
            <h2 class="text-2xl font-semibold">You haven't placed any orders yet.</h2>
            <p class="text-gray-500 mt-2">Let's change that!</p>
            <a routerLink="/menu" class="mt-6 inline-block bg-red-600 text-white font-bold py-3 px-8 rounded-full text-lg hover:bg-red-700 transition duration-300">
                Browse Menu
            </a>
        </div>
      }
    </div>
  `,
})
export class MyOrdersComponent implements OnInit {
  apiService = inject(ApiService);
  authService = inject(AuthService);
  
  orders = signal<Order[]>([]);
  loading = signal(true);

  ngOnInit() {
    const user = this.authService.user();
    if (user) {
      this.apiService.getUserOrders(user.id).subscribe(data => {
        this.orders.set(data.sort((a, b) => b.orderDate.getTime() - a.orderDate.getTime()));
        this.loading.set(false);
      });
    }
  }

  getStatusColor(status: string) {
    switch (status) {
      case 'Delivered': return 'text-green-600';
      case 'Out for Delivery': return 'text-yellow-600';
      case 'Preparing': return 'text-indigo-600';
      case 'Cancelled': return 'text-red-600';
      default: return 'text-gray-600';
    }
  }
}
