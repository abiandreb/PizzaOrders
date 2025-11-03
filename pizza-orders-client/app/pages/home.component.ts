import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ApiService } from '../services/api.service';
import { Product } from '../types';
import { ProductCardComponent } from '../components/product-card.component';
import { LoadingSpinnerComponent } from '../components/loading-spinner.component';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink, ProductCardComponent, LoadingSpinnerComponent],
  template: `
    <!-- Hero Section -->
    <div class="bg-white">
      <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-24 md:py-32 text-center">
        <h1 class="text-4xl md:text-6xl font-extrabold text-gray-900">
          <span class="text-blue-600">Fast, Fresh, &</span> Fantastic Pizza.
        </h1>
        <p class="mt-4 max-w-2xl mx-auto text-lg md:text-xl text-gray-500">
          Order your favorite pizza online and get it delivered to your door in minutes.
        </p>
        <div class="mt-8">
          <a routerLink="/menu" class="inline-block bg-red-600 text-white font-bold py-3 px-8 rounded-full text-lg hover:bg-red-700 transition duration-300">
            Order Now
          </a>
        </div>
      </div>
    </div>

    <!-- Featured Products -->
    <div class="bg-slate-50 py-16">
      <div class="container mx-auto px-4 sm:px-6 lg:px-8">
        <h2 class="text-3xl font-bold text-center mb-8">Featured Pizzas</h2>
        @if(featuredProducts$ | async; as products) {
            <div class="grid md:grid-cols-2 lg:grid-cols-4 gap-8">
                @for(product of products; track product.id) {
                    <app-product-card [product]="product"></app-product-card>
                }
            </div>
        } @else {
            <app-loading-spinner></app-loading-spinner>
        }
      </div>
    </div>
  `,
})
export class HomeComponent implements OnInit {
  apiService = inject(ApiService);
  featuredProducts$!: Observable<Product[]>;

  ngOnInit() {
    this.featuredProducts$ = this.apiService.getProducts().pipe(
        map(products => products.filter(p => p.category !== 'drink').slice(0, 4))
    );
  }
}
