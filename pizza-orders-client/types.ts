export interface User {
  id: string;
  email: string;
  name: string;
  role: 'user' | 'admin';
}

export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  category: 'veggie' | 'non-veggie' | 'drink' | 'custom';
}

export interface CartItem {
  product: Product;
  quantity: number;
}

export interface Order {
  id: string;
  userId: string;
  items: CartItem[];
  total: number;
  status: 'Received' | 'Preparing' | 'Out for Delivery' | 'Delivered';
  date: string;
  customer: {
    name: string;
    address: string;
    phone: string;
  };
}

export interface ConstructorOption {
  id: string;
  name: string;
  price: number;
  type: 'base' | 'sauce' | 'cheese' | 'veggie' | 'meat';
}

export interface ConstructorOptions {
    bases: ConstructorOption[];
    sauces: ConstructorOption[];
    cheeses: ConstructorOption[];
    veggies: ConstructorOption[];
    meats: ConstructorOption[];
}
