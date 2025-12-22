import { Injectable, signal, computed, inject } from '@angular/core';
import { Cart, CartItem } from '../models/cart.model';
import { Product, Topping } from '../models/product.model';
import { ApiService } from './api.service';

const TAX_RATE = 0.08;

@Injectable({ providedIn: 'root' })
export class CartService {
  apiService = inject(ApiService);
  cart = signal<Cart | null>(null);

  totalItems = computed(() => {
    return this.cart()?.items.reduce((acc, item) => acc + item.quantity, 0) ?? 0;
  });

  async createCart() {
    // In a real app, this would call api/Cart/create
    const sessionId = `session_${Math.random().toString(36).substring(2, 10)}`;
    this.cart.set({ sessionId, items: [], subtotal: 0, tax: 0, total: 0 });
  }

  async addToCart(product: Product, quantity: number, toppingIds: number[]) {
    if (!this.cart()) return;
    
    const toppings = await this.apiService.getToppings();
    const selectedToppings = toppings.filter(t => toppingIds.includes(t.id));
    const toppingsPrice = selectedToppings.reduce((acc, t) => acc + t.price, 0);
    const itemPrice = product.price + toppingsPrice;
    
    // Create a unique ID for the item based on product and toppings
    const sortedToppingIds = [...toppingIds].sort();
    const itemId = `${product.id}_${sortedToppingIds.join('_')}`;

    this.cart.update(currentCart => {
      if (!currentCart) return null;
      const existingItemIndex = currentCart.items.findIndex(item => item.id === itemId);
      let newItems: CartItem[];

      if (existingItemIndex > -1) {
        newItems = currentCart.items.map((item, index) => 
          index === existingItemIndex ? { ...item, quantity: item.quantity + quantity } : item
        );
      } else {
        const newItem: CartItem = {
          id: itemId,
          product,
          quantity,
          selectedToppings: toppingIds,
          price: itemPrice
        };
        newItems = [...currentCart.items, newItem];
      }
      return this.recalculateCart({ ...currentCart, items: newItems });
    });
  }
  
  updateItemQuantity(itemId: string, newQuantity: number) {
    this.cart.update(currentCart => {
       if (!currentCart) return null;
       let newItems = [...currentCart.items];
       if (newQuantity <= 0) {
         newItems = newItems.filter(item => item.id !== itemId);
       } else {
         newItems = newItems.map(item => item.id === itemId ? {...item, quantity: newQuantity} : item);
       }
       return this.recalculateCart({...currentCart, items: newItems});
    });
  }

  removeItem(itemId: string) {
    this.updateItemQuantity(itemId, 0);
  }

  clearCart() {
    this.cart.update(currentCart => {
       if (!currentCart) return null;
       return this.recalculateCart({...currentCart, items: []});
    });
  }
  
  private recalculateCart(cart: Cart): Cart {
    const subtotal = cart.items.reduce((acc, item) => acc + (item.price * item.quantity), 0);
    const tax = subtotal * TAX_RATE;
    const total = subtotal + tax;
    return { ...cart, subtotal, tax, total };
  }
}
