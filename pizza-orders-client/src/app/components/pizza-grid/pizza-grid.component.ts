import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { PizzaDto } from "../../models/pizza-dto";

@Component({
  selector: 'app-pizza-grid',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  template: `
    <div *ngIf="!loading && !error"
         class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
      <div *ngFor="let pizza of pizzas"
           class="bg-white rounded-2xl shadow hover:shadow-lg transition p-4 flex flex-col items-center">

        <h3 class="text-xl font-semibold text-gray-800">{{ pizza.name }}</h3>
        <p class="text-gray-500 text-sm mt-1 mb-3">{{ pizza.description }}</p>
        <span class="text-lg font-bold text-blue-600 mb-3">{{ pizza.price | currency:'USD' }}</span>
        <button class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg">
          Order Now
        </button>
      </div>
    </div>
  `
})
export class PizzaGridComponent implements OnInit {
  pizzas: PizzaDto[] = [];
  loading = true;
  error: string | null = null;

  private readonly apiUrl = 'http://localhost:5062/api/Pizza/';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<PizzaDto[]>(this.apiUrl).subscribe({
      next: (data) => {
        try {
          console.log('DATA:', data);
          this.pizzas = data;
          this.loading = false;
        } catch (err) {
          console.error('Error inside next handler:', err);
        }
      },
      error: (err) => {
        console.error('HTTP ERROR:', err);
      }
    });
  }
}
