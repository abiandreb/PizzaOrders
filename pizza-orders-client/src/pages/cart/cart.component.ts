
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, RouterLink]
})
export class CartComponent {
  cartService = inject(CartService);

  updateQuantity(id: number, event: Event) {
    const quantity = parseInt((event.target as HTMLInputElement).value, 10);
    if (!isNaN(quantity) && quantity >= 0) {
      this.cartService.updateQuantity(id, quantity);
    }
  }

  removeItem(id: number) {
    this.cartService.removeItem(id);
  }
}
