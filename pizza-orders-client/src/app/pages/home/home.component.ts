import { Component } from '@angular/core';
import {PizzaGridComponent} from "../../components/pizza-grid/pizza-grid.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    PizzaGridComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
