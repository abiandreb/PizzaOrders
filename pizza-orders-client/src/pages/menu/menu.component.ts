import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { ApiService } from '../../services/api.service';
import { PizzaCardComponent } from '../../components/pizza-card/pizza-card.component';
import { Pizza } from '../../models';

@Component({
  selector: 'app-menu',
  imports: [CommonModule, PizzaCardComponent],
  template: `
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-4xl font-bold text-center mb-8 text-gray-800">Our Menu</h1>
      
      <!-- Pizzas -->
      <h2 class="text-3xl font-semibold mb-6 text-gray-700 border-b-2 border-gray-200 pb-2">Pizzas</h2>
      @if (pizzas(); as pizzaList) {
        @if (pizzaList.length > 0) {
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            @for (pizza of pizzaList; track pizza.id) {
              <app-pizza-card [pizza]="pizza" />
            }
          </div>
        } @else {
          <p class="text-center text-gray-500">Loading pizzas...</p>
        }
      }
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MenuComponent {
  private apiService = inject(ApiService);
  pizzas = toSignal(this.apiService.getPizzas(), { initialValue: [] as Pizza[] });
}
