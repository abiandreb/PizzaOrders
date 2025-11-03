import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ApiService } from '../services/api.service';
import { Order, OrderStatus } from '../types';
import { LoadingSpinnerComponent } from '../components/loading-spinner.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, DatePipe, LoadingSpinnerComponent, FormsModule],
  template: `
    <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <h1 class="text-4xl font-bold mb-8">Admin Dashboard - All Orders</h1>

      @if(loading()) {
        <app-loading-spinner></app-loading-spinner>
      } @else {
        <div class="bg-white shadow-lg rounded-lg overflow-x-auto">
            <table class="w-full text-sm text-left text-gray-500">
                <thead class="text-xs text-gray-700 uppercase bg-gray-50">
                    <tr>
                        <th scope="col" class="px-6 py-3">Order ID</th>
                        <th scope="col" class="px-6 py-3">Date</th>
                        <th scope="col" class="px-6 py-3">User ID</th>
                        <th scope="col" class="px-6 py-3">Total</th>
                        <th scope="col" class="px-6 py-3">Status</th>
                    </tr>
                </thead>
                <tbody>
                    @for(order of orders(); track order.id) {
                        <tr class="bg-white border-b hover:bg-gray-50">
                            <td class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap">{{ order.id }}</td>
                            <td class="px-6 py-4">{{ order.orderDate | date: 'short' }}</td>
                            <td class="px-6 py-4">{{ order.userId }}</td>
                            <td class="px-6 py-4">\${{ order.total.toFixed(2) }}</td>
                            <td class="px-6 py-4">
                                <select [ngModel]="order.status" (ngModelChange)="updateStatus(order.id, $event)" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5">
                                    @for(status of orderStatuses; track status){
                                        <option [value]="status">{{ status }}</option>
                                    }
                                </select>
                            </td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
      }
    </div>
  `,
})
export class AdminDashboardComponent implements OnInit {
  apiService = inject(ApiService);
  orders = signal<Order[]>([]);
  loading = signal(true);
  orderStatuses: OrderStatus[] = ['Pending', 'Preparing', 'Out for Delivery', 'Delivered', 'Cancelled'];

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.apiService.getAllOrders().subscribe(data => {
      this.orders.set(data.sort((a, b) => b.orderDate.getTime() - a.orderDate.getTime()));
      this.loading.set(false);
    });
  }

  updateStatus(orderId: string, status: OrderStatus) {
    this.apiService.updateOrderStatus(orderId, status).subscribe(updatedOrder => {
      if (updatedOrder) {
        this.orders.update(currentOrders => 
          currentOrders.map(o => o.id === orderId ? updatedOrder : o)
        );
      }
    });
  }
}
