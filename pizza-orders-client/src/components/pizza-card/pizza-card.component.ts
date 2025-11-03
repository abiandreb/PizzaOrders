import { ChangeDetectionStrategy, Component, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Pizza } from '../../models';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-pizza-card',
  imports: [CommonModule, RouterLink],
  template: `
    @if (pizza()) {
      <div class="bg-white rounded-lg shadow-md overflow-hidden transition-transform duration-300 hover:scale-105 flex flex-col">
        <a [routerLink]="['/menu', pizza()!.id]">
          <img [src]="pizza()!.imageUrl" [alt]="pizza()!.name" class="w-full h-48 object-cover">
        </a>
        <div class="p-4 flex-grow flex flex-col">
          <h3 class="text-xl font-semibold mb-2">{{ pizza()!.name }}</h3>
          <p class="text-gray-600 mb-4 flex-grow">{{ pizza()!.description }}</p>
          <div class="flex justify-between items-center mt-auto">
            <span class="text-2xl font-bold text-gray-800">{{ pizza()!.price | currency }}</span>
            <button (click)="addToCart()" class="bg-orange-600 text-white px-4 py-2 rounded-full hover:bg-orange-700 transition-colors">
              Add to Cart
            </button>
          </div>
        </div>
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PizzaCardComponent {
  pizza = input.required<Pizza>();
  private cartService = inject(CartService);

  addToCart() {
    this.cartService.addItem(this.pizza()!);
  }
}