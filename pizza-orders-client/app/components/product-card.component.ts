import { Component, Input, inject } from '@angular/core';
import { Product } from '../types';
import { CartService } from '../services/cart.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if(product) {
      <div class="bg-white rounded-lg shadow-lg overflow-hidden transform transition duration-300 hover:scale-105">
        <img [src]="product.imageUrl" [alt]="product.name" class="w-full h-48 object-cover">
        <div class="p-4">
          <h3 class="text-lg font-semibold">{{ product.name }}</h3>
          <p class="text-gray-600 text-sm mt-1">{{ product.description }}</p>
          <div class="flex justify-between items-center mt-4">
            <span class="text-xl font-bold text-blue-600">\${{ product.price.toFixed(2) }}</span>
            <button (click)="addToCart()" class="px-4 py-2 bg-red-600 text-white text-sm font-medium rounded-full hover:bg-red-700 transition duration-300">
              Add to Cart
            </button>
          </div>
        </div>
      </div>
    }
  `,
})
export class ProductCardComponent {
  @Input() product!: Product;
  cartService = inject(CartService);

  addToCart() {
    this.cartService.addItem(this.product);
  }
}
