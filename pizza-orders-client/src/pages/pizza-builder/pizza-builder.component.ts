
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { Pizza } from '../../models';

interface Option {
  name: string;
  price: number;
}

@Component({
  selector: 'app-pizza-builder',
  templateUrl: './pizza-builder.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule]
})
export class PizzaBuilderComponent {
  private cartService = inject(CartService);
  private router = inject(Router);

  // Configuration for pizza options
  bases: Option[] = [{ name: 'Classic', price: 6.00 }];
  sizes: Option[] = [{ name: 'Medium (12")', price: 0 }, { name: 'Large (14")', price: 2.00 }];
  sauces: Option[] = [{ name: 'Tomato', price: 0 }, { name: 'BBQ', price: 0.50 }, { name: 'Garlic', price: 0.50 }];
  cheeses: Option[] = [{ name: 'Mozzarella', price: 1.00 }, { name: 'Cheddar', price: 1.00 }];
  toppings: Option[] = [
    { name: 'Pepperoni', price: 1.50 }, { name: 'Mushrooms', price: 1.00 }, { name: 'Onions', price: 0.75 },
    { name: 'Peppers', price: 0.75 }, { name: 'Sausage', price: 1.50 }, { name: 'Olives', price: 1.00 }
  ];

  // Signals for user selections
  selectedBase = signal<Option>(this.bases[0]);
  selectedSize = signal<Option>(this.sizes[0]);
  selectedSauce = signal<Option>(this.sauces[0]);
  selectedCheese = signal<Option>(this.cheeses[0]);
  selectedToppings = signal<Option[]>([]);
  
  // Computed signal for total price
  totalPrice = computed(() => {
    const toppingsPrice = this.selectedToppings().reduce((acc, topping) => acc + topping.price, 0);
    return this.selectedBase().price + this.selectedSize().price + this.selectedSauce().price + this.selectedCheese().price + toppingsPrice;
  });

  toggleTopping(topping: Option) {
    this.selectedToppings.update(currentToppings => {
      const index = currentToppings.findIndex(t => t.name === topping.name);
      if (index > -1) {
        return currentToppings.filter(t => t.name !== topping.name);
      } else {
        return [...currentToppings, topping];
      }
    });
  }

  isToppingSelected(topping: Option): boolean {
    return this.selectedToppings().some(t => t.name === topping.name);
  }
  
  addToCart() {
    const pizzaName = `Custom Pizza (${this.selectedSize().name})`;
    // FIX: Generate a description to conform to the Pizza model.
    const toppingsDescription = this.selectedToppings().map(t => t.name).join(', ') || 'none';
    const description = `A custom pizza with ${this.selectedSauce().name} sauce, ${this.selectedCheese().name} cheese, and toppings: ${toppingsDescription}.`;

    const customPizza: Pizza = {
      id: Date.now(), // Unique ID for this custom creation
      name: pizzaName,
      description: description,
      price: this.totalPrice(),
      imageUrl: 'https://picsum.photos/id/111/400/300' // Placeholder image for custom pizza
    };
    this.cartService.addItem(customPizza);
    this.router.navigate(['/cart']);
  }
}
