import axios from 'axios';
import type { AxiosInstance, InternalAxiosRequestConfig } from 'axios';
import type {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  Product,
  Topping,
  Cart,
  AddToCartRequest,
  UpdateCartRequest,
  CheckoutRequest,
  Order,
  CreateProductRequest,
  UpdateProductRequest,
  CreateToppingRequest,
  UpdateToppingRequest,
} from '../types';

const API_BASE_URL = '/api';

class ApiClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    this.client.interceptors.request.use(
      (config: InternalAxiosRequestConfig) => {
        const token = localStorage.getItem('token');
        if (token && config.headers) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => Promise.reject(error)
    );

    this.client.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
          originalRequest._retry = true;

          const refreshToken = localStorage.getItem('refreshToken');
          const token = localStorage.getItem('token');

          if (refreshToken && token) {
            try {
              const response = await this.refreshToken(refreshToken);
              localStorage.setItem('token', response.token);
              localStorage.setItem('refreshToken', response.refreshToken);
              originalRequest.headers.Authorization = `Bearer ${response.token}`;
              return this.client.request(originalRequest);
            } catch (refreshError) {
              localStorage.removeItem('token');
              localStorage.removeItem('refreshToken');
              window.location.href = '/login';
              return Promise.reject(refreshError);
            }
          } else {
            // No tokens available, redirect to login
            localStorage.removeItem('token');
            localStorage.removeItem('refreshToken');
            window.location.href = '/login';
          }
        }
        return Promise.reject(error);
      }
    );
  }

  // Auth endpoints
  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await this.client.post<AuthResponse>('/auth/register-user', data);
    return response.data;
  }

  async login(data: LoginRequest): Promise<AuthResponse> {
    const response = await this.client.post<AuthResponse>('/auth/login-user', data);
    return response.data;
  }

  async refreshToken(refreshToken: string): Promise<AuthResponse> {
    const response = await this.client.post<AuthResponse>('/auth/refresh-token', {
      token: localStorage.getItem('token'),
      refreshToken,
    });
    return response.data;
  }

  // Product endpoints
  async getProductsByType(productType: number): Promise<Product[]> {
    const response = await this.client.get<Product[]>(`/product?productType=${productType}`);
    return response.data;
  }

  async getProductById(id: number): Promise<Product> {
    const response = await this.client.get<Product>(`/product/${id}`);
    return response.data;
  }

  // Cart endpoints
  async createCart(): Promise<Cart> {
    const response = await this.client.post<Cart>('/cart/create');
    return response.data;
  }

  async getCart(sessionId: string): Promise<Cart> {
    const response = await this.client.get<Cart>(`/cart/${sessionId}`);
    return response.data;
  }

  async addToCart(sessionId: string, data: AddToCartRequest): Promise<void> {
    await this.client.post(`/cart/${sessionId}/add`, data);
  }

  async updateCart(sessionId: string, data: UpdateCartRequest): Promise<void> {
    await this.client.put(`/cart/${sessionId}/update`, data);
  }

  async removeFromCart(sessionId: string, productId: number, toppingIds?: number[]): Promise<void> {
    const params = new URLSearchParams({ productId: productId.toString() });
    if (toppingIds && toppingIds.length > 0) {
      toppingIds.forEach(id => params.append('toppingIds', id.toString()));
    }
    await this.client.delete(`/cart/${sessionId}/remove?${params.toString()}`);
  }

  async clearCart(sessionId: string): Promise<void> {
    await this.client.delete(`/cart/${sessionId}`);
  }

  // Checkout endpoint
  async checkout(sessionId: string, data?: CheckoutRequest): Promise<Order> {
    const response = await this.client.post<Order>(`/checkout/${sessionId}`, data || {});
    return response.data;
  }

  // Admin - Product Management
  async getAllProductsForAdmin(): Promise<Product[]> {
    const response = await this.client.get<Product[]>('/management/products');
    return response.data;
  }

  async createProduct(data: CreateProductRequest): Promise<Product> {
    const response = await this.client.post<Product>('/management/products', data);
    return response.data;
  }

  async updateProduct(data: UpdateProductRequest): Promise<Product> {
    const response = await this.client.put<Product>('/management/products', data);
    return response.data;
  }

  async deleteProduct(id: number): Promise<void> {
    await this.client.delete(`/management/products/${id}`);
  }

  // Admin - Topping Management
  async getAllToppings(): Promise<Topping[]> {
    const response = await this.client.get<Topping[]>('/management/toppings');
    return response.data;
  }

  async createTopping(data: CreateToppingRequest): Promise<Topping> {
    const response = await this.client.post<Topping>('/management/toppings', data);
    return response.data;
  }

  async updateTopping(data: UpdateToppingRequest): Promise<Topping> {
    const response = await this.client.put<Topping>('/management/toppings', data);
    return response.data;
  }

  async deleteTopping(id: number): Promise<void> {
    await this.client.delete(`/management/toppings/${id}`);
  }
}

export const api = new ApiClient();
