
import { Component, ChangeDetectionStrategy, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product, Topping, ProductType } from '../../models/product.model';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [CommonModule],
})
export class ProductDetailComponent {
  product = input.required<Product>();
  closeModal = output<void>();

  quantity = signal(1);
  selectedSize = signal<string | undefined>(undefined);
  selectedToppings = signal<Topping[]>([]);
  ProductType = ProductType;

  constructor(private cartService: CartService) {}

  ngOnInit() {
    if (this.product().sizes && this.product().sizes!.length > 0) {
      this.selectedSize.set(this.product().sizes![0]);
    }
  }

  increment(): void {
    this.quantity.update(q => q + 1);
  }

  decrement(): void {
    this.quantity.update(q => Math.max(1, q - 1));
  }

  toggleTopping(topping: Topping, event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked;
    this.selectedToppings.update(toppings => {
      if (isChecked) {
        return [...toppings, topping];
      } else {
        return toppings.filter(t => t.id !== topping.id);
      }
    });
  }

  addToCart(): void {
    this.cartService.addToCart(
      this.product(),
      this.quantity(),
      this.selectedSize(),
      this.selectedToppings()
    );
    this.closeModal.emit();
  }
}
