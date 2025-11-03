export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
}

export interface Pizza {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
}

export interface Extra {
    id: number;
    name: string;
    price: number;
    type: 'drink' | 'extra';
    imageUrl?: string;
}

export interface CartItem {
  product: Pizza | Extra;
  quantity: number;
}

export interface User {
  id: string;
  name: string;
  email: string;
  password?: string; // Should not be stored in frontend state long-term
  roles: ('user' | 'admin')[];
  address?: string;
}

export type OrderStatus = 'New' | 'InProgress' | 'Completed' | 'Cancelled';

export interface GuestDetails {
    name: string;
    email: string;
    phone: string;
    address: string;
}

export interface Order {
  id: string;
  userId: string | null;
  date: Date;
  items: CartItem[];
  total: number;
  status: OrderStatus;
  guestDetails: GuestDetails | null;
}
