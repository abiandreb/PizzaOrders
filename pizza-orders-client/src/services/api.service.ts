import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay, map } from 'rxjs';
import { Pizza, Extra, Order, OrderStatus } from '../models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:7258/api'; // TODO: Move to environment variable

  private pizzas = signal<Pizza[]>([]);
  private orders = signal<Order[]>([]);

  getPizzas(): Observable<Pizza[]> {
    return this.http.get<Pizza[]>(`${this.baseUrl}/Pizza`);
  }
  
  getPizza(id: number): Observable<Pizza | undefined> {
    return this.http.get<Pizza>(`${this.baseUrl}/Pizza/${id}`);
  }

  getTopPizzas(count = 3): Observable<Pizza[]> {
    return this.getPizzas().pipe(
      map(pizzas => pizzas.slice(0, count))
    );
  }

  createOrder(orderData: Omit<Order, 'id' | 'date' | 'status'>): Observable<Order> {
    return this.http.post<Order>(`${this.baseUrl}/Order`, orderData);
  }

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/Order`);
  }
  
  getOrdersByUserId(userId: string): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/Order/user/${userId}`);
  }
  
  updateOrderStatus(orderId: string, status: OrderStatus): Observable<Order | undefined> {
    return this.http.put<Order>(`${this.baseUrl}/Order/${orderId}`, { status });
  }
}