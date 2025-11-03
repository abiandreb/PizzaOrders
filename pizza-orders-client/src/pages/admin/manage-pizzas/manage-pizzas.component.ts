
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { ApiService } from '../../../services/api.service';
import { Pizza } from '../../../models';

@Component({
  selector: 'app-manage-pizzas',
  templateUrl: './manage-pizzas.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule]
})
export class ManagePizzasComponent {
    private apiService = inject(ApiService);
    pizzas = toSignal(this.apiService.getPizzas(), {initialValue: [] as Pizza[]});

    // In a real app, these methods would call the API to persist changes.
    // For this mock, we'll just log the actions.
    addPizza() {
        console.log('Admin wants to add a new pizza.');
        alert('This feature is for demonstration purposes.');
    }

    editPizza(pizza: Pizza) {
        console.log('Admin wants to edit pizza:', pizza);
        alert('This feature is for demonstration purposes.');
    }

    deletePizza(pizzaId: number) {
        console.log('Admin wants to delete pizza with ID:', pizzaId);
        alert('This feature is for demonstration purposes.');
    }
}
