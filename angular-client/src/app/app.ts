import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Products } from './products/products';
import { SessionService } from './services/session.service';
import { CartApiService } from './services/cart-api.service';
import { CommonModule } from '@angular/common'; // Import CommonModule for ngIf, ngFor etc.

@Component({
  selector: 'app-root',
  standalone: true, // Assuming this is a standalone component
  imports: [RouterOutlet, Products, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('angular-client');

  constructor(private sessionService: SessionService, private cartApiService: CartApiService) { }

  ngOnInit(): void {
    let sessionId = this.sessionService.getSessionId();
    if (sessionId) {
      this.cartApiService.getCart(sessionId).subscribe({
        next: (cart) => {
          console.log('Existing cart loaded:', cart);
          // Here you might want to store the cart in a shared state or service
        },
        error: (err) => {
          console.error('Failed to load existing cart, creating new one:', err);
          this.createAndStoreNewCart();
        }
      });
    } else {
      this.createAndStoreNewCart();
    }
  }

  private createAndStoreNewCart(): void {
    this.cartApiService.createCart().subscribe({
      next: (cart) => {
        this.sessionService.setSessionId(cart.sessionId);
        console.log('New cart created and session ID stored:', cart);
        // Here you might want to store the cart in a shared state or service
      },
      error: (err) => {
        console.error('Failed to create new cart:', err);
      }
    });
  }
}
