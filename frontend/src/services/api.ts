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
  OrderDetail,
  CreateProductRequest,
  UpdateProductRequest,
  CreateToppingRequest,
  UpdateToppingRequest,
  OrderAdminDto,
  UpdateOrderStatusRequest,
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

              // Update Redux store with new tokens
              const tokenPayload = JSON.parse(atob(response.token.split('.')[1]));
              const userObj = {
                email: tokenPayload.email || tokenPayload.sub,
                roles: tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
                  ? (Array.isArray(tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
                      ? tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
                      : [tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']])
                  : [],
              };

              // Import store dynamically to avoid circular dependencies
              import('../store').then(({ store }) => {
                store.dispatch({
                  type: 'auth/setCredentials',
                  payload: {
                    user: userObj,
                    token: response.token,
                    refreshToken: response.refreshToken,
                  },
                });
              });

              originalRequest.headers.Authorization = `Bearer ${response.token}`;
              return this.client.request(originalRequest);
            } catch (refreshError) {
              console.error('Refresh token failed:', refreshError);
              localStorage.removeItem('token');
              localStorage.removeItem('refreshToken');

              // Clear Redux state
              import('../store').then(({ store }) => {
                store.dispatch({ type: 'auth/logout' });
              });

              // Don't redirect automatically - let ProtectedRoute handle it
              return Promise.reject(refreshError);
            }
          } else {
            // No tokens available - just clear state, don't redirect
            console.warn('No refresh token available for 401 response');
            localStorage.removeItem('token');
            localStorage.removeItem('refreshToken');

            // Clear Redux state
            import('../store').then(({ store }) => {
              store.dispatch({ type: 'auth/logout' });
            });
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

  async logout(): Promise<void> {
    await this.client.post('/auth/logout');
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
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

  // Checkout endpoints
  async checkout(sessionId: string, data?: CheckoutRequest): Promise<Order> {
    // Use authenticated endpoint if user is logged in
    const token = localStorage.getItem('token');
    const endpoint = token
      ? `/checkout/${sessionId}`
      : `/checkout/${sessionId}/guest`;
    const response = await this.client.post<Order>(endpoint, data || {});
    return response.data;
  }

  async guestCheckout(sessionId: string, data?: CheckoutRequest): Promise<Order> {
    const response = await this.client.post<Order>(`/checkout/${sessionId}/guest`, data || {});
    return response.data;
  }

  // User Orders
  async getMyOrders(): Promise<Order[]> {
    const response = await this.client.get<Order[]>('/orders');
    return response.data;
  }

  async getMyOrderById(id: number): Promise<OrderDetail> {
    const response = await this.client.get<OrderDetail>(`/orders/${id}`);
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

  // Admin - Order Management
  async getAllOrdersForAdmin(status?: string): Promise<OrderAdminDto[]> {
    const params = status ? `?status=${status}` : '';
    const response = await this.client.get<OrderAdminDto[]>(`/management/orders${params}`);
    return response.data;
  }

  async getOrderByIdForAdmin(id: number): Promise<OrderAdminDto> {
    const response = await this.client.get<OrderAdminDto>(`/management/orders/${id}`);
    return response.data;
  }

  async updateOrderStatus(id: number, data: UpdateOrderStatusRequest): Promise<OrderAdminDto> {
    const response = await this.client.put<OrderAdminDto>(`/management/orders/${id}/status`, data);
    return response.data;
  }

  async getAvailableOrderStatuses(): Promise<string[]> {
    const response = await this.client.get<string[]>('/management/orders/statuses');
    return response.data;
  }
}

export const api = new ApiClient();
