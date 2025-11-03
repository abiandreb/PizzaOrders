
import { Injectable, signal, computed } from '@angular/core';
// FIX: Import `Extra` model and correct `Product` type alias for type safety.
import { CartItem, Pizza, Extra } from '../models';

type Product = Pizza | Extra;

@Injectable({ providedIn: 'root' })
export class CartService {
  // Signal to hold the array of cart items
  cartItems = signal<CartItem[]>([]);

  // Computed signal for the total number of items in the cart
  itemCount = computed(() => this.cartItems().reduce((acc, item) => acc + item.quantity, 0));

  // Computed signal for the subtotal of all items in the cart
  subtotal = computed(() => this.cartItems().reduce((acc, item) => acc + item.product.price * item.quantity, 0));

  // Computed signal for tax (e.g., 8%)
  tax = computed(() => this.subtotal() * 0.08);

  // Computed signal for the grand total
  total = computed(() => this.subtotal() + this.tax());

  // Adds a product to the cart or increases its quantity if it already exists
  addItem(product: Product) {
    this.cartItems.update(items => {
      const existingItem = items.find(item => item.product.id === product.id);
      if (existingItem) {
        return items.map(item =>
          item.product.id === product.id ? { ...item, quantity: item.quantity + 1 } : item
        );
      }
      return [...items, { product, quantity: 1 }];
    });
  }

  // Updates the quantity of a specific item in the cart
  updateQuantity(productId: number, quantity: number) {
    this.cartItems.update(items =>
      items.map(item =>
        item.product.id === productId ? { ...item, quantity } : item
      ).filter(item => item.quantity > 0) // Remove if quantity is 0
    );
  }

  // Removes an item completely from the cart
  removeItem(productId: number) {
    this.cartItems.update(items => items.filter(item => item.product.id !== productId));
  }

  // Clears all items from the cart
  clearCart() {
    this.cartItems.set([]);
  }
}
