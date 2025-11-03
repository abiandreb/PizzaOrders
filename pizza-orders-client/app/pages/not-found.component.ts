import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-24 text-center">
        <h1 class="text-9xl font-extrabold text-blue-600 tracking-widest">404</h1>
        <div class="bg-red-600 px-2 text-sm rounded rotate-12 absolute" style="left: 50%; transform: translateX(-50%) translateY(-150%);">
            Page Not Found
        </div>
        <p class="mt-4 text-lg text-gray-600">Oops! The page you’re looking for doesn’t exist.</p>
        <div class="mt-8">
            <a routerLink="/" class="inline-block bg-blue-600 text-white font-bold py-3 px-8 rounded-full text-lg hover:bg-blue-700 transition duration-300">
                Go Home
            </a>
        </div>
    </div>
  `,
})
export class NotFoundComponent {}
