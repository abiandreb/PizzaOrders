import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cart, CartItemRequest, CartItemUpdateRequest } from '../interfaces/cart.interface';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CartApiService {
  private apiUrl = `${environment.apiUrl}/cart`;

  constructor(private http: HttpClient) { }

  createCart(): Observable<Cart> {
    return this.http.post<Cart>(`${this.apiUrl}/create`, {});
  }

  getCart(sessionId: string): Observable<Cart> {
    return this.http.get<Cart>(`${this.apiUrl}/${sessionId}`);
  }

  addToCart(sessionId: string, request: CartItemRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${sessionId}/add`, request);
  }

  updateCart(sessionId: string, request: CartItemUpdateRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${sessionId}/update`, request);
  }

  removeFromCart(sessionId: string, productId: number, toppingIds: number[] = []): Observable<void> {
    let params = new HttpParams();
    params = params.append('productId', productId.toString());
    toppingIds.forEach(id => {
      params = params.append('toppingIds', id.toString());
    });
    return this.http.delete<void>(`${this.apiUrl}/${sessionId}/remove`, { params });
  }

  clearCart(sessionId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${sessionId}`);
  }
}
