import {Component, OnInit} from '@angular/core';
import {ProductService} from '../services/product-service';
import {ProductsResponse} from '../interfaces/ProductResponse';
import {JsonPipe, NgForOf} from '@angular/common';
import {ProductCardComponent} from "./product-card.component";
import {CartApiService} from "../services/cart-api.service";
import {SessionService} from "../services/session.service";
import {CartItemRequest} from "../interfaces/cart.interface";

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    JsonPipe,
    ProductCardComponent,
    NgForOf
  ],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products implements OnInit {

  public productResponse: ProductsResponse[] = [];

  constructor(private productService: ProductService,
              private cartApiService: CartApiService,
              private sessionService: SessionService) {
  }

  ngOnInit() {
    this.productService.getProductsByType(0).subscribe(products => {
      this.productResponse = products;
    })
  }

  onAddToCart(product: ProductsResponse) {
    const sessionId = this.sessionService.getSessionId();
    if (!sessionId) {
      console.error('Session ID not found. Cannot add to cart.');
      return;
    }

    const cartItem: CartItemRequest = {
      productId: product.id,
      quantity: 1, // Assuming a default quantity of 1
      toppingIds: [] // Assuming no toppings for now
    };

    this.cartApiService.addToCart(sessionId, cartItem).subscribe({
      next: () => {
        console.log('Product added to cart:', product.name);
        // Optionally, update cart display or show a success message
      },
      error: (err) => {
        console.error('Failed to add product to cart:', err);
      }
    });
  }
}

