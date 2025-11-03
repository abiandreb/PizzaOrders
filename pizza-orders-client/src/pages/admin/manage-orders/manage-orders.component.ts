import { ChangeDetectionStrategy, Component, inject, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { ApiService } from '../../../services/api.service';
import { Order, OrderStatus } from '../../../models';

@Component({
  selector: 'app-manage-orders',
  templateUrl: './manage-orders.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, DatePipe]
})
export class ManageOrdersComponent {
  private apiService = inject(ApiService);
  orders = toSignal(this.apiService.getOrders(), { initialValue: [] as Order[] });
  
  orderStatuses: OrderStatus[] = ['New', 'InProgress', 'Completed', 'Cancelled'];

  sortedOrders = computed(() => {
    return this.orders().sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
  });

  updateStatus(orderId: string, event: Event) {
    const status = (event.target as HTMLSelectElement).value as OrderStatus;
    this.apiService.updateOrderStatus(orderId, status).subscribe();
  }
  
  getStatusClass(status: string) {
    switch (status) {
      case 'Completed':
        return 'text-green-800';
      case 'InProgress':
        return 'text-yellow-800';
      case 'New':
        return 'text-blue-800';
      case 'Cancelled':
        return 'text-red-800';
      default:
        return 'text-gray-800';
    }
  }
}