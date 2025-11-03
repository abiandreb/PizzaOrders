import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../services/api.service';
import { Product, ProductCategory } from '../types';
import { ProductCardComponent } from '../components/product-card.component';
import { LoadingSpinnerComponent } from '../components/loading-spinner.component';

type FilterType = 'all' | ProductCategory;

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, ProductCardComponent, LoadingSpinnerComponent],
  template: `
    <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <h1 class="text-4xl font-bold text-center mb-8">Our Menu</h1>
      
      <!-- Filter Buttons -->
      <div class="flex justify-center space-x-2 md:space-x-4 mb-12">
        <button (click)="activeFilter.set('all')" [class.bg-blue-600]="activeFilter() === 'all'" [class.text-white]="activeFilter() === 'all'"
          class="px-4 py-2 font-medium rounded-full transition duration-300 hover:bg-blue-500 hover:text-white">All</button>
        <button (click)="activeFilter.set('veggie')" [class.bg-blue-600]="activeFilter() === 'veggie'" [class.text-white]="activeFilter() === 'veggie'"
          class="px-4 py-2 font-medium rounded-full transition duration-300 hover:bg-blue-500 hover:text-white">Veggie</button>
        <button (click)="activeFilter.set('non-veggie')" [class.bg-blue-600]="activeFilter() === 'non-veggie'" [class.text-white]="activeFilter() === 'non-veggie'"
          class="px-4 py-2 font-medium rounded-full transition duration-300 hover:bg-blue-500 hover:text-white">Non-Veggie</button>
        <button (click)="activeFilter.set('drink')" [class.bg-blue-600]="activeFilter() === 'drink'" [class.text-white]="activeFilter() === 'drink'"
          class="px-4 py-2 font-medium rounded-full transition duration-300 hover:bg-blue-500 hover:text-white">Drinks</button>
      </div>

      <!-- Products Grid -->
       @if(loading()) {
            <app-loading-spinner></app-loading-spinner>
        } @else {
            <div class="grid md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
                 @for(product of filteredProducts(); track product.id) {
                    <app-product-card [product]="product"></app-product-card>
                }
            </div>
            @if(filteredProducts().length === 0){
                <p class="text-center text-gray-500">No products found for this category.</p>
            }
        }
    </div>
  `,
})
export class MenuComponent implements OnInit {
  apiService = inject(ApiService);
  
  products = signal<Product[]>([]);
  loading = signal<boolean>(true);
  activeFilter = signal<FilterType>('all');
  
  filteredProducts = computed(() => {
    const filter = this.activeFilter();
    if (filter === 'all') {
      return this.products();
    }
    return this.products().filter(p => p.category === filter);
  });

  ngOnInit() {
    this.apiService.getProducts().subscribe(data => {
      this.products.set(data);
      this.loading.set(false);
    });
  }
}
