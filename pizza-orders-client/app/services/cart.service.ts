import { Injectable, signal, computed, effect } from '@angular/core';
import { CartItem, Product } from '../types';

const PROMO_CODES: { [key: string]: number } = {
  PROMO10: 0.10,
  PIZZADAY: 0.20,
};

@Injectable({ providedIn: 'root' })
export class CartService {
  items = signal<CartItem[]>([]);
  promoCode = signal<string | null>(null);

  subtotal = computed(() => this.items().reduce((acc, item) => acc + item.product.price * item.quantity, 0));
  discount = computed(() => {
    const code = this.promoCode();
    if (code && PROMO_CODES[code]) {
      return this.subtotal() * PROMO_CODES[code];
    }
    return 0;
  });
  total = computed(() => this.subtotal() - this.discount());
  itemCount = computed(() => this.items().reduce((acc, item) => acc + item.quantity, 0));

  constructor() {
    this.initCartFromLocalStorage();
    effect(() => {
      localStorage.setItem('pizzaCart', JSON.stringify(this.items()));
    });
  }

  private initCartFromLocalStorage() {
     if (typeof window !== 'undefined') {
        const cartJson = localStorage.getItem('pizzaCart');
        if (cartJson) {
            this.items.set(JSON.parse(cartJson));
        }
     }
  }
  
  addItem(product: Product, quantity: number = 1) {
    this.items.update(items => {
      const existingItem = items.find(i => i.product.id === product.id);
      if (existingItem) {
        return items.map(i => i.product.id === product.id ? { ...i, quantity: i.quantity + quantity } : i);
      }
      return [...items, { product, quantity }];
    });
  }
  
  updateQuantity(productId: string, quantity: number) {
    this.items.update(items => {
        if (quantity <= 0) {
            return items.filter(i => i.product.id !== productId);
        }
        return items.map(i => i.product.id === productId ? { ...i, quantity } : i);
    });
  }

  removeItem(productId: string) {
    this.items.update(items => items.filter(i => i.product.id !== productId));
  }
  
  applyPromoCode(code: string): boolean {
    if (PROMO_CODES[code.toUpperCase()]) {
      this.promoCode.set(code.toUpperCase());
      return true;
    }
    this.promoCode.set(null);
    return false;
  }
  
  clearCart() {
    this.items.set([]);
    this.promoCode.set(null);
  }
}
