export interface User {
  id: string;
  name: string;
  email: string;
  role: 'user' | 'admin';
}

export type ProductCategory = 'veggie' | 'non-veggie' | 'drink';

export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  category: ProductCategory;
}

export interface CartItem {
  product: Product;
  quantity: number;
}

export type OrderStatus = 'Pending' | 'Preparing' | 'Out for Delivery' | 'Delivered' | 'Cancelled';

export interface Order {
  id: string;
  userId: string;
  items: CartItem[];
  subtotal: number;
  discount: number;
  total: number;
  deliveryAddress: any;
  status: OrderStatus;
  orderDate: Date;
}

export interface PizzaOption {
  name: string;
  price: number;
}

export interface PizzaConstructorOptions {
  bases: PizzaOption[];
  sauces: PizzaOption[];
  cheeses: PizzaOption[];
  veggies: PizzaOption[];
  meats: PizzaOption[];
}
