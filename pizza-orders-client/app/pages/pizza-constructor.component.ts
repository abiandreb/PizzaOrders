import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../services/api.service';
import { CartService } from '../services/cart.service';
import { PizzaConstructorOptions, PizzaOption, Product } from '../types';
import { LoadingSpinnerComponent } from '../components/loading-spinner.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pizza-constructor',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent],
  template: `
    <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <h1 class="text-4xl font-bold text-center mb-8">Build Your Own Pizza</h1>

      @if(loading()) {
        <app-loading-spinner></app-loading-spinner>
      } @else if(options()) {
        <div class="grid lg:grid-cols-3 gap-8">
          <!-- Options Column -->
          <div class="lg:col-span-2 space-y-8">
             <!-- Base -->
             <div>
                <h2 class="text-2xl font-semibold mb-4 border-b pb-2">1. Choose Your Base</h2>
                <div class="space-y-2">
                   @for(base of options()!.bases; track base.name){
                    <label class="flex items-center p-3 rounded-lg border hover:bg-blue-50 cursor-pointer">
                        <input type="radio" name="base" [value]="base.name" [checked]="selectedBase()?.name === base.name" (change)="selectBase(base)" class="h-4 w-4 text-blue-600 border-gray-300 focus:ring-blue-500">
                        <span class="ml-3 text-gray-700">{{ base.name }}</span>
                        <span class="ml-auto font-medium">\${{ base.price.toFixed(2) }}</span>
                    </label>
                   }
                </div>
             </div>
             <!-- Other options similar to Base: Sauce, Cheese, Veggies, Meats -->
             <!-- Sauce -->
             <div>
                <h2 class="text-2xl font-semibold mb-4 border-b pb-2">2. Choose Your Sauce</h2>
                <div class="space-y-2">
                   @for(sauce of options()!.sauces; track sauce.name){
                    <label class="flex items-center p-3 rounded-lg border hover:bg-blue-50 cursor-pointer">
                        <input type="radio" name="sauce" [value]="sauce.name" [checked]="selectedSauce()?.name === sauce.name" (change)="selectSauce(sauce)" class="h-4 w-4 text-blue-600 border-gray-300 focus:ring-blue-500">
                        <span class="ml-3 text-gray-700">{{ sauce.name }}</span>
                        <span class="ml-auto font-medium">\${{ sauce.price.toFixed(2) }}</span>
                    </label>
                   }
                </div>
             </div>
              <!-- Cheese -->
             <div>
                <h2 class="text-2xl font-semibold mb-4 border-b pb-2">3. Choose Your Cheese</h2>
                <div class="space-y-2">
                   @for(cheese of options()!.cheeses; track cheese.name){
                    <label class="flex items-center p-3 rounded-lg border hover:bg-blue-50 cursor-pointer">
                        <input type="radio" name="cheese" [value]="cheese.name" [checked]="selectedCheese()?.name === cheese.name" (change)="selectCheese(cheese)" class="h-4 w-4 text-blue-600 border-gray-300 focus:ring-blue-500">
                        <span class="ml-3 text-gray-700">{{ cheese.name }}</span>
                        <span class="ml-auto font-medium">\${{ cheese.price.toFixed(2) }}</span>
                    </label>
                   }
                </div>
             </div>
             <!-- Veggies -->
             <div>
                <h2 class="text-2xl font-semibold mb-4 border-b pb-2">4. Add Veggies</h2>
                <div class="grid grid-cols-2 gap-2">
                   @for(veggie of options()!.veggies; track veggie.name){
                    <label class="flex items-center p-3 rounded-lg border hover:bg-blue-50 cursor-pointer">
                        <input type="checkbox" [value]="veggie.name" [checked]="isToppingSelected(veggie, 'veggie')" (change)="toggleTopping(veggie, 'veggie')" class="h-4 w-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500">
                        <span class="ml-3 text-gray-700">{{ veggie.name }}</span>
                        <span class="ml-auto font-medium">\${{ veggie.price.toFixed(2) }}</span>
                    </label>
                   }
                </div>
             </div>
             <!-- Meats -->
             <div>
                <h2 class="text-2xl font-semibold mb-4 border-b pb-2">5. Add Meats</h2>
                <div class="grid grid-cols-2 gap-2">
                   @for(meat of options()!.meats; track meat.name){
                    <label class="flex items-center p-3 rounded-lg border hover:bg-blue-50 cursor-pointer">
                        <input type="checkbox" [value]="meat.name" [checked]="isToppingSelected(meat, 'meat')" (change)="toggleTopping(meat, 'meat')" class="h-4 w-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500">
                        <span class="ml-3 text-gray-700">{{ meat.name }}</span>
                        <span class="ml-auto font-medium">\${{ meat.price.toFixed(2) }}</span>
                    </label>
                   }
                </div>
             </div>
          </div>

          <!-- Summary Column -->
          <div class="lg:col-span-1">
            <div class="bg-white p-6 rounded-lg shadow-lg sticky top-24">
              <h2 class="text-2xl font-bold mb-4">Your Custom Pizza</h2>
              <ul class="space-y-2 text-gray-600">
                <li class="flex justify-between"><span>Base:</span> <span>{{ selectedBase()?.name || 'None' }}</span></li>
                <li class="flex justify-between"><span>Sauce:</span> <span>{{ selectedSauce()?.name || 'None' }}</span></li>
                <li class="flex justify-between"><span>Cheese:</span> <span>{{ selectedCheese()?.name || 'None' }}</span></li>
                @if(selectedVeggies().length > 0) {
                  <li class="pt-2 border-t"><strong>Veggies:</strong></li>
                  @for(veg of selectedVeggies(); track veg.name){
                    <li class="flex justify-between pl-2"><span>{{ veg.name }}</span> <span>\${{ veg.price.toFixed(2) }}</span></li>
                  }
                }
                @if(selectedMeats().length > 0) {
                  <li class="pt-2 border-t"><strong>Meats:</strong></li>
                  @for(meat of selectedMeats(); track meat.name){
                    <li class="flex justify-between pl-2"><span>{{ meat.name }}</span> <span>\${{ meat.price.toFixed(2) }}</span></li>
                  }
                }
              </ul>
              <div class="border-t mt-4 pt-4">
                <p class="text-2xl font-bold flex justify-between">
                  <span>Total:</span>
                  <span class="text-blue-600">\${{ totalPrice().toFixed(2) }}</span>
                </p>
              </div>
              <button (click)="addToCart()" [disabled]="!isPizzaValid()" class="mt-6 w-full bg-red-600 text-white font-bold py-3 rounded-full hover:bg-red-700 disabled:bg-gray-400 transition duration-300">
                Add to Cart
              </button>
            </div>
          </div>
        </div>
      }
    </div>
  `,
})
export class PizzaConstructorComponent implements OnInit {
  apiService = inject(ApiService);
  cartService = inject(CartService);
  router = inject(Router);

  loading = signal(true);
  options = signal<PizzaConstructorOptions | null>(null);

  selectedBase = signal<PizzaOption | null>(null);
  selectedSauce = signal<PizzaOption | null>(null);
  selectedCheese = signal<PizzaOption | null>(null);
  selectedVeggies = signal<PizzaOption[]>([]);
  selectedMeats = signal<PizzaOption[]>([]);

  totalPrice = computed(() => {
    let total = 0;
    if (this.selectedBase()) total += this.selectedBase()!.price;
    if (this.selectedSauce()) total += this.selectedSauce()!.price;
    if (this.selectedCheese()) total += this.selectedCheese()!.price;
    this.selectedVeggies().forEach(v => total += v.price);
    this.selectedMeats().forEach(m => total += m.price);
    return total;
  });

  isPizzaValid = computed(() => !!this.selectedBase() && !!this.selectedSauce() && !!this.selectedCheese());
  
  ngOnInit() {
    this.apiService.getPizzaOptions().subscribe(data => {
      this.options.set(data);
      // Set defaults
      this.selectedBase.set(data.bases[0]);
      this.selectedSauce.set(data.sauces[0]);
      this.selectedCheese.set(data.cheeses[0]);
      this.loading.set(false);
    });
  }

  selectBase(base: PizzaOption) { this.selectedBase.set(base); }
  selectSauce(sauce: PizzaOption) { this.selectedSauce.set(sauce); }
  selectCheese(cheese: PizzaOption) { this.selectedCheese.set(cheese); }

  toggleTopping(topping: PizzaOption, type: 'veggie' | 'meat') {
    const signalToUpdate = type === 'veggie' ? this.selectedVeggies : this.selectedMeats;
    signalToUpdate.update(currentToppings => {
      if (currentToppings.some(t => t.name === topping.name)) {
        return currentToppings.filter(t => t.name !== topping.name);
      }
      return [...currentToppings, topping];
    });
  }

  isToppingSelected(topping: PizzaOption, type: 'veggie' | 'meat'): boolean {
    const list = type === 'veggie' ? this.selectedVeggies() : this.selectedMeats();
    return list.some(t => t.name === topping.name);
  }

  addToCart() {
    if (!this.isPizzaValid()) return;
    
    const toppings = [...this.selectedVeggies(), ...this.selectedMeats()].map(t => t.name).join(', ');
    const description = `${this.selectedBase()?.name}, ${this.selectedSauce()?.name}, ${this.selectedCheese()?.name}${toppings ? ', ' + toppings : ''}`;
    
    const customPizza: Product = {
      id: `custom-${Date.now()}`,
      name: 'Custom Pizza',
      description: description,
      price: this.totalPrice(),
      imageUrl: 'https://picsum.photos/seed/custompizza/400/300',
      category: 'non-veggie', // Or determine based on toppings
    };
    
    this.cartService.addItem(customPizza);
    this.router.navigate(['/menu']);
  }
}
