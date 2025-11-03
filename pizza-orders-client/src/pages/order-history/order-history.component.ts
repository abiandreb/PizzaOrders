
import { ChangeDetectionStrategy, Component, inject, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { of, switchMap } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { Order } from '../../models';

@Component({
  selector: 'app-order-history',
  templateUrl: './order-history.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, DatePipe]
})
export class OrderHistoryComponent {
  private apiService = inject(ApiService);
  private authService = inject(AuthService);

  private orders$ = of(this.authService.currentUser()).pipe(
    switchMap(user => {
      if (user) {
        return this.apiService.getOrdersByUserId(user.id);
      }
      return of([]);
    })
  );
  
  orders = toSignal(this.orders$, { initialValue: [] as Order[] });
  
  sortedOrders = computed(() => {
    return this.orders().sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
  });

  getStatusClass(status: string) {
    switch (status) {
      case 'Completed':
        return 'bg-green-100 text-green-800';
      case 'InProgress':
        return 'bg-yellow-100 text-yellow-800';
      case 'New':
        return 'bg-blue-100 text-blue-800';
      case 'Cancelled':
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  }
}
