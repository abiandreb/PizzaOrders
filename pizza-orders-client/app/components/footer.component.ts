import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  template: `
    <footer class="bg-white border-t">
      <div class="container mx-auto py-8 px-4 sm:px-6 lg:px-8">
        <div class="md:flex md:justify-between">
          <div class="mb-6 md:mb-0">
            <a href="#" class="text-2xl font-bold text-blue-600">PizzaDash</a>
            <p class="text-gray-500 mt-2">© {{ currentYear }} PizzaDash. All rights reserved.</p>
          </div>
          <div class="grid grid-cols-2 gap-8 sm:gap-6 sm:grid-cols-3">
             <div>
                <h2 class="mb-6 text-sm font-semibold text-gray-900 uppercase">Company</h2>
                <ul class="text-gray-500">
                    <li class="mb-4"><a href="#" class="hover:underline">About</a></li>
                    <li><a href="#" class="hover:underline">Careers</a></li>
                </ul>
            </div>
            <div>
                <h2 class="mb-6 text-sm font-semibold text-gray-900 uppercase">Legal</h2>
                <ul class="text-gray-500">
                    <li class="mb-4"><a href="#" class="hover:underline">Privacy Policy</a></li>
                    <li><a href="#" class="hover:underline">Terms & Conditions</a></li>
                </ul>
            </div>
          </div>
        </div>
      </div>
    </footer>
  `,
})
export class FooterComponent {
  currentYear = new Date().getFullYear();
}
