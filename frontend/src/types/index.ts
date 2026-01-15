export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface Product {
  id: number;
  name: string;
  description?: string;
  basePrice: number;
  hasToppings: boolean;
  productType: ProductType;
  imageUrl?: string;
  properties?: ProductProperties;
}

export interface ProductProperties {
  size?: string;
  weight?: number;
  volume?: number;
}

export const ProductType = {
  Pizza: 0,
  Drink: 1,
  Dessert: 2
} as const;

export type ProductType = typeof ProductType[keyof typeof ProductType];

export interface Topping {
  id: number;
  name: string;
  description: string;
  stock: number;
  price: number;
}

export interface CartItem {
  productId: number;
  productName: string;
  quantity: number;
  basePrice: number;
  toppingIds: number[];
  toppings: CartTopping[];
  totalPrice: number;
}

export interface CartTopping {
  toppingId: number;
  toppingName: string;
  price: number;
}

export interface Cart {
  sessionId: string;
  items: CartItem[];
  totalPrice: number;
}

export interface AddToCartRequest {
  productId: number;
  quantity: number;
  toppingIds?: number[];
}

export interface UpdateCartRequest {
  productId: number;
  quantity: number;
  toppingIds?: number[];
}

export interface CheckoutRequest {
  userId?: string;
}

export interface Order {
  orderId: number;
  userId?: string;
  totalPrice: number;
  orderDate: string;
  status: string;
}

export interface CreateProductRequest {
  name: string;
  description?: string;
  basePrice: number;
  hasToppings: boolean;
  productType: ProductType;
  imageUrl?: string;
  properties?: ProductProperties;
}

export interface UpdateProductRequest extends CreateProductRequest {
  id: number;
}

export interface CreateToppingRequest {
  name: string;
  description: string;
  stock: number;
  price: number;
}

export interface UpdateToppingRequest extends CreateToppingRequest {
  id: number;
}

export interface User {
  email: string;
  roles: string[];
}
