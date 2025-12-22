import { Component, ChangeDetectionStrategy, output, inject, signal, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Product, Topping } from '../../models/product.model';
import { NgOptimizedImage } from '@angular/common';

type AdminTab = 'products' | 'toppings';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgOptimizedImage]
})
export class AdminPanelComponent implements OnInit {
  closePanel = output<void>();
  apiService = inject(ApiService);

  activeTab = signal<AdminTab>('products');
  products = signal<Product[]>([]);
  toppings = signal<Topping[]>([]);
  isLoading = signal(true);

  async ngOnInit() {
    this.isLoading.set(true);
    // Fetch all products by calling the service for each type
    const pizzaPromise = this.apiService.getProducts(0); // Pizza
    const drinkPromise = this.apiService.getProducts(1); // Drink
    const starterPromise = this.apiService.getProducts(2); // Starter
    const toppingsPromise = this.apiService.getToppings();

    const [pizzas, drinks, starters, toppings] = await Promise.all([pizzaPromise, drinkPromise, starterPromise, toppingsPromise]);
    
    this.products.set([...pizzas, ...starters, ...drinks]);
    this.toppings.set(toppings);
    this.isLoading.set(false);
  }

  selectTab(tab: AdminTab) {
    this.activeTab.set(tab);
  }

  editItem(item: Product | Topping) {
    alert(`Editing ${item.name} (functionality is a mock-up).`);
  }

  deleteItem(item: Product | Topping) {
    if (confirm(`Are you sure you want to delete ${item.name}?`)) {
      alert(`${item.name} deleted (functionality is a mock-up).`);
    }
  }

  getProductTypeName(type: number): string {
    switch (type) {
      case 0: return 'Pizza';
      case 1: return 'Drink';
      case 2: return 'Starter';
      default: return 'Unknown';
    }
  }
}
