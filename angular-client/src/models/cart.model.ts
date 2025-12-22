import { Product } from './product.model';

export interface CartItem {
  id: string; // Composite ID like 'productId_topping1_topping2'
  product: Product;
  quantity: number;
  selectedToppings: number[];
  price: number; // price for one item, including toppings
}

export interface Cart {
  sessionId: string;
  items: CartItem[];
  subtotal: number;
  tax: number;
  total: number;
}
