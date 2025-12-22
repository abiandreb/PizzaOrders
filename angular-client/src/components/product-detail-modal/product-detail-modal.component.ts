import { Component, ChangeDetectionStrategy, input, output, signal, computed, effect } from '@angular/core';
import { Product, ProductType, Topping } from '../../models/product.model';
import { NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-product-detail-modal',
  templateUrl: './product-detail-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgOptimizedImage],
})
export class ProductDetailModalComponent {
  product = input.required<Product>();
  closeModal = output<void>();
  addToCart = output<{ product: Product, quantity: number, toppings: number[] }>();

  productTypes = ProductType;
  quantity = signal(1);
  selectedToppings = signal<Set<number>>(new Set());
  
  totalPrice = computed(() => {
    const basePrice = this.product().price;
    const toppingsPrice = (this.product().toppings ?? [])
      .filter(t => this.selectedToppings().has(t.id))
      .reduce((sum, topping) => sum + topping.price, 0);
    return (basePrice + toppingsPrice) * this.quantity();
  });

  constructor() {
    // This effect handles the case where the input product changes, resetting the state.
    effect(() => {
      this.product(); // depend on product input
      this.quantity.set(1);
      this.selectedToppings.set(new Set());
    });
  }

  onCloseModal() {
    this.closeModal.emit();
  }

  incrementQuantity() {
    this.quantity.update(q => q + 1);
  }

  decrementQuantity() {
    this.quantity.update(q => (q > 1 ? q - 1 : 1));
  }

  toggleTopping(id: number) {
    this.selectedToppings.update(currentToppings => {
      const newToppings = new Set(currentToppings);
      if (newToppings.has(id)) {
        newToppings.delete(id);
      } else {
        newToppings.add(id);
      }
      return newToppings;
    });
  }

  onAddToCart() {
    this.addToCart.emit({
      product: this.product(),
      quantity: this.quantity(),
      toppings: Array.from(this.selectedToppings()),
    });
  }
}
