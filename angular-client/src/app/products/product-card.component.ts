import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ProductsResponse } from '../interfaces/ProductResponse';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css',
})
export class ProductCardComponent {
  @Input() product!: ProductsResponse;
  @Output() addToCart = new EventEmitter<ProductsResponse>();

  onAddToCart() {
    this.addToCart.emit(this.product);
  }
}
