import { Injectable } from '@angular/core';
import { Product, ProductType, Topping } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private toppings: Topping[] = [
    { id: 1, name: 'Extra Cheese', description: 'Gooey mozzarella cheese', stock: 100, price: 1.50 },
    { id: 2, name: 'Pepperoni', description: 'Spicy pepperoni slices', stock: 100, price: 2.00 },
    { id: 3, name: 'Mushrooms', description: 'Freshly sliced mushrooms', stock: 100, price: 1.00 },
    { id: 4, name: 'Onions', description: 'Crisp red onions', stock: 100, price: 0.75 },
    { id: 5, name: 'Bell Peppers', description: 'Green bell peppers', stock: 100, price: 0.75 },
    { id: 6, name: 'Olives', description: 'Black olives', stock: 100, price: 1.25 },
    { id: 7, name: 'Bacon', description: 'Smoky bacon bits', stock: 100, price: 2.50 },
    { id: 8, name: 'Pineapple', description: 'Sweet and controversial', stock: 100, price: 1.50 },
  ];

  private products: Product[] = [
    // Pizzas
    { id: 1, name: 'Margherita', description: 'Classic tomato, mozzarella, and basil.', price: 12.99, productType: ProductType.Pizza, imageUrl: 'https://picsum.photos/seed/margherita/400', availableSizes: ['Medium', 'Large'], toppings: this.toppings },
    { id: 2, name: 'Pepperoni Feast', description: 'Loaded with spicy pepperoni and mozzarella.', price: 14.99, productType: ProductType.Pizza, imageUrl: 'https://picsum.photos/seed/pepperoni/400', availableSizes: ['Medium', 'Large'], toppings: this.toppings },
    { id: 3, name: 'Veggie Supreme', description: 'A garden of fresh vegetables on a pizza.', price: 13.99, productType: ProductType.Pizza, imageUrl: 'https://picsum.photos/seed/veggie/400', availableSizes: ['Medium', 'Large'], toppings: this.toppings },
    { id: 4, name: 'Hawaiian', description: 'A tropical delight with ham and pineapple.', price: 14.50, productType: ProductType.Pizza, imageUrl: 'https://picsum.photos/seed/hawaiian/400', availableSizes: ['Medium', 'Large'], toppings: this.toppings },
     { id: 5, name: 'BBQ Chicken', description: 'Tangy BBQ sauce, chicken, and red onions.', price: 15.99, productType: ProductType.Pizza, imageUrl: 'https://picsum.photos/seed/bbq/400', availableSizes: ['Medium', 'Large'], toppings: this.toppings },
    { id: 6, name: 'Meat Lover\'s', description: 'For the carnivore in you. All the meats!', price: 16.99, productType: ProductType.Pizza, imageUrl: 'https://picsum.photos/seed/meat/400', availableSizes: ['Medium', 'Large'], toppings: this.toppings },
    
    // Drinks
    { id: 7, name: 'Cola', description: 'Refreshing cola.', price: 2.50, productType: ProductType.Drink, imageUrl: 'https://picsum.photos/seed/cola/400' },
    { id: 8, name: 'Lemonade', description: 'Sweet and tangy.', price: 2.50, productType: ProductType.Drink, imageUrl: 'https://picsum.photos/seed/lemonade/400' },
    { id: 9, name: 'Iced Tea', description: 'Classic iced tea.', price: 2.25, productType: ProductType.Drink, imageUrl: 'https://picsum.photos/seed/icedtea/400' },
    { id: 10, name: 'Water', description: 'Pure and simple.', price: 1.50, productType: ProductType.Drink, imageUrl: 'https://picsum.photos/seed/water/400' },

    // Starters
    { id: 11, name: 'Garlic Bread', description: 'Toasted with garlic butter.', price: 5.99, productType: ProductType.Starter, imageUrl: 'https://picsum.photos/seed/garlicbread/400' },
    { id: 12, name: 'Chicken Wings', description: 'Spicy and savory chicken wings.', price: 8.99, productType: ProductType.Starter, imageUrl: 'https://picsum.photos/seed/wings/400' },
    { id: 13, name: 'Mozzarella Sticks', description: 'Fried cheese sticks with marinara.', price: 7.50, productType: ProductType.Starter, imageUrl: 'https://picsum.photos/seed/mozzarella/400' },
  ];

  getProducts(productType: ProductType): Promise<Product[]> {
    return new Promise(resolve => {
      setTimeout(() => {
        resolve(this.products.filter(p => p.productType === productType));
      }, 500);
    });
  }
  
  getToppings(): Promise<Topping[]> {
     return new Promise(resolve => {
      setTimeout(() => {
        resolve(this.toppings);
      }, 300);
    });
  }
}
