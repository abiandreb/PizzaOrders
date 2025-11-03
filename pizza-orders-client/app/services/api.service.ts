import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';
import { Product, Order, PizzaConstructorOptions, OrderStatus } from '../types';

const MOCK_PRODUCTS: Product[] = [
  { id: 'p1', name: 'Margherita', description: 'Classic cheese and tomato.', price: 8.99, imageUrl: 'https://picsum.photos/seed/margherita/400/300', category: 'veggie' },
  { id: 'p2', name: 'Pepperoni', description: 'Loaded with spicy pepperoni.', price: 10.99, imageUrl: 'https://picsum.photos/seed/pepperoni/400/300', category: 'non-veggie' },
  { id: 'p3', name: 'Veggie Supreme', description: 'A garden on a pizza.', price: 11.99, imageUrl: 'https://picsum.photos/seed/veggie/400/300', category: 'veggie' },
  { id: 'p4', name: 'Meat Lovers', description: 'All the meats you can handle.', price: 12.99, imageUrl: 'https://picsum.photos/seed/meat/400/300', category: 'non-veggie' },
  { id: 'p5', name: 'Cola', description: 'Refreshing cola.', price: 1.99, imageUrl: 'https://picsum.photos/seed/cola/400/300', category: 'drink' },
  { id: 'p6', name: 'Lemonade', description: 'Tangy and sweet.', price: 2.49, imageUrl: 'https://picsum.photos/seed/lemonade/400/300', category: 'drink' },
];

const MOCK_PIZZA_OPTIONS: PizzaConstructorOptions = {
    bases: [{ name: 'Classic Crust', price: 5.00 }, { name: 'Thin Crust', price: 5.50 }, { name: 'Stuffed Crust', price: 7.00 }],
    sauces: [{ name: 'Tomato Sauce', price: 1.00 }, { name: 'BBQ Sauce', price: 1.50 }, { name: 'White Garlic Sauce', price: 1.50 }],
    cheeses: [{ name: 'Mozzarella', price: 1.50 }, { name: 'Cheddar Blend', price: 1.75 }, { name: 'Vegan Cheese', price: 2.00 }],
    veggies: [{ name: 'Onions', price: 0.50 }, { name: 'Peppers', price: 0.50 }, { name: 'Mushrooms', price: 0.75 }, { name: 'Olives', price: 0.75 }],
    meats: [{ name: 'Pepperoni', price: 1.50 }, { name: 'Sausage', price: 1.50 }, { name: 'Bacon', price: 1.75 }, { name: 'Chicken', price: 2.00 }],
};

let MOCK_ORDERS: Order[] = [
    { id: 'ord1', userId: 'usr1', items: [{ product: MOCK_PRODUCTS[1], quantity: 2 }], subtotal: 21.98, discount: 0, total: 21.98, deliveryAddress: { name: 'John Doe', address: '123 Pizza St' }, status: 'Delivered', orderDate: new Date() },
    { id: 'ord2', userId: 'admin1', items: [{ product: MOCK_PRODUCTS[2], quantity: 1 }, { product: MOCK_PRODUCTS[4], quantity: 2 }], subtotal: 16.97, discount: 0, total: 16.97, deliveryAddress: { name: 'Admin User', address: '456 Admin Ave' }, status: 'Preparing', orderDate: new Date() },
];


@Injectable({ providedIn: 'root' })
export class ApiService {
  getProducts(): Observable<Product[]> {
    return of(MOCK_PRODUCTS).pipe(delay(500));
  }

  getPizzaOptions(): Observable<PizzaConstructorOptions> {
    return of(MOCK_PIZZA_OPTIONS).pipe(delay(500));
  }

  placeOrder(order: Omit<Order, 'id' | 'orderDate' | 'status'>): Observable<Order> {
    const newOrder: Order = {
        ...order,
        id: `ord${Date.now()}`,
        status: 'Pending',
        orderDate: new Date(),
    };
    MOCK_ORDERS.push(newOrder);
    return of(newOrder).pipe(delay(1000));
  }
  
  getUserOrders(userId: string): Observable<Order[]> {
    const userOrders = MOCK_ORDERS.filter(o => o.userId === userId);
    return of(userOrders).pipe(delay(700));
  }
  
  getAllOrders(): Observable<Order[]> {
    return of(MOCK_ORDERS).pipe(delay(700));
  }
  
  updateOrderStatus(orderId: string, status: OrderStatus): Observable<Order> {
    const order = MOCK_ORDERS.find(o => o.id === orderId);
    if (order) {
        order.status = status;
        return of(order).pipe(delay(300));
    }
    return of(null as any);
  }
}
