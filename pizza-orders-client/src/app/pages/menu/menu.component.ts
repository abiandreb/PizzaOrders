import { Component } from '@angular/core';
import {PizzaGridComponent} from "../../components/pizza-grid/pizza-grid.component";

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [
    PizzaGridComponent
  ],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {

}
