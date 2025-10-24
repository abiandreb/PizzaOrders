import { Component } from '@angular/core';
import {CommonModule, NgOptimizedImage} from "@angular/common";

@Component({
  selector: 'app-pizza-list',
  standalone: true,
  imports: [CommonModule, NgOptimizedImage],
  templateUrl: './pizza-list.component.html',
  styleUrl: './pizza-list.component.css'
})

export class PizzaListComponent {
  pizzas = [
    {
      name: 'Margherita',
      description: 'Classic tomato sauce, mozzarella cheese, and fresh basil.',
      price: 8.99,
      image: 'https://images.unsplash.com/photo-1601924582971-d4a4eb2b1c7b?auto=format&fit=crop&w=400&q=80'
    },
    {
      name: 'Pepperoni',
      description: 'Loaded with pepperoni slices and extra mozzarella.',
      price: 10.49,
      image: 'https://images.unsplash.com/photo-1601924593865-7c2a5d61d7b6?auto=format&fit=crop&w=400&q=80'
    },
    {
      name: 'BBQ Chicken',
      description: 'Tangy BBQ sauce, grilled chicken, and red onions.',
      price: 11.99,
      image: 'https://images.unsplash.com/photo-1628840042765-356cda07504e?auto=format&fit=crop&w=400&q=80'
    },
    {
      name: 'Veggie Delight',
      description: 'Fresh peppers, onions, olives, mushrooms, and sweetcorn.',
      price: 9.49,
      image: 'https://images.unsplash.com/photo-1579751626657-72bc17010498?auto=format&fit=crop&w=400&q=80'
    },
    {
      name: 'Hawaiian',
      description: 'Juicy pineapple chunks and smoky ham with mozzarella.',
      price: 10.29,
      image: 'https://images.unsplash.com/photo-1604382354936-07c5f282c80f?auto=format&fit=crop&w=400&q=80'
    },
    {
      name: 'Meat Lovers',
      description: 'Pepperoni, sausage, bacon, ham, and beef crumbles.',
      price: 12.49,
      image: 'https://images.unsplash.com/photo-1628840042765-356cda07504e?auto=format&fit=crop&w=400&q=80'
    },
    {
      name: 'Four Cheese',
      description: 'A creamy mix of mozzarella, parmesan, cheddar, and gorgonzola.',
      price: 10.99,
      image: 'https://images.unsplash.com/photo-1594007654869-6e1b67b1a4c0?auto=format&fit=crop&w=400&q=80'
    },
    {
      name: 'Spicy Diablo',
      description: 'Hot chili flakes, jalapeños, and spicy Italian sausage.',
      price: 11.49,
      image: 'https://images.unsplash.com/photo-1571091655789-405eb7a3a4e4?auto=format&fit=crop&w=400&q=80'
    }
  ];


}
