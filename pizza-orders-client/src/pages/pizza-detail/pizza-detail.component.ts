import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap, map } from 'rxjs/operators';
import { of } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { CartService } from '../../services/cart.service';
import { Pizza } from '../../models';

@Component({
  selector: 'app-pizza-detail',
  imports: [CommonModule, RouterLink],
  template: `
    <div class="container mx-auto px-4 py-8">
      @if (state(); as s) {
        @if (s.pizza; as pizza) {
          <div class="bg-white rounded-lg shadow-lg overflow-hidden md:flex">
            <img [src]="pizza.imageUrl" [alt]="pizza.name" class="w-full md:w-1/2 h-64 md:h-auto object-cover">
            <div class="p-8 md:w-1/2 flex flex-col justify-between">
              <div>
                  <h1 class="text-4xl font-bold text-gray-800 mb-2">{{ pizza.name }}</h1>
                  <p class="text-gray-600 text-lg mb-6">{{ pizza.description }}</p>
              </div>
              <div class="flex justify-between items-center mt-6">
                  <span class="text-3xl font-bold text-gray-800">{{ pizza.price | currency }}</span>
                  <button (click)="addToCart(pizza)" class="bg-orange-600 text-white px-8 py-3 rounded-full text-lg font-semibold hover:bg-orange-700 transition-colors">
                    Add to Cart
                  </button>
              </div>
            </div>
          </div>
        } @else if (s.error) {
          <div class="text-center">
              <h2 class="text-2xl font-semibold text-red-600 mb-4">{{ s.error }}</h2>
              <p class="text-gray-500 mb-6">Please check the URL or go back to the menu.</p>
              <a routerLink="/menu" class="text-orange-600 hover:underline">Return to Menu</a>
          </div>
        }
      } @else {
        <p class="text-center text-gray-500">Loading pizza details...</p>
      }
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PizzaDetailComponent {
  private route = inject(ActivatedRoute);
  private apiService = inject(ApiService);
  private cartService = inject(CartService);

  private state$ = this.route.paramMap.pipe(
    switchMap(params => {
      const id = params.get('id');
      if (!id) {
        return of({ pizza: undefined, error: 'Pizza not found.' });
      }
      return this.apiService.getPizza(+id).pipe(
        map(pizza => ({ pizza, error: pizza ? undefined : 'Pizza not found.' }))
      );
    })
  );
  
  state = toSignal(this.state$);

  addToCart(pizza: Pizza) {
    this.cartService.addItem(pizza);
  }
}