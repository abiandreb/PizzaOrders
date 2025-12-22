import { Component, ChangeDetectionStrategy, input, output } from '@angular/core';
import { Product, ProductType } from '../../models/product.model';
import { NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgOptimizedImage]
})
export class ProductCardComponent {
  product = input.required<Product>();
  selectProduct = output<Product>();

  productTypes = ProductType;

  onSelectProduct() {
    this.selectProduct.emit(this.product());
  }
}
