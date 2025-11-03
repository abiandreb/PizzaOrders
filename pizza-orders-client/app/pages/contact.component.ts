import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <div class="bg-white">
      <div class="container mx-auto px-4 sm:px-6 lg:px-8 py-16">
        <div class="text-center">
          <h1 class="text-4xl font-bold">Get in Touch</h1>
          <p class="mt-2 text-lg text-gray-600">We'd love to hear from you. Send us a message!</p>
        </div>
        
        <div class="mt-12 grid lg:grid-cols-2 gap-12">
          <!-- Contact Form -->
          <div class="bg-slate-50 p-8 rounded-lg">
            <form [formGroup]="contactForm" (ngSubmit)="sendMessage()">
              <div class="space-y-6">
                <div>
                  <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
                  <input type="text" id="name" formControlName="name" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500">
                </div>
                 <div>
                  <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
                  <input type="email" id="email" formControlName="email" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500">
                </div>
                 <div>
                  <label for="message" class="block text-sm font-medium text-gray-700">Message</label>
                  <textarea id="message" formControlName="message" rows="4" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"></textarea>
                </div>
              </div>
              <div class="mt-6">
                <button type="submit" [disabled]="contactForm.invalid" class="w-full bg-blue-600 text-white font-bold py-3 rounded-md hover:bg-blue-700 disabled:bg-gray-400">
                  Send Message
                </button>
              </div>
            </form>
          </div>
          
          <!-- Map & Info -->
          <div>
            <div class="aspect-w-16 aspect-h-9 rounded-lg overflow-hidden">
               <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3153.019597992983!2d-122.4217283846816!3d37.78825197975661!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x80858097d73b02c1%3A0x87cfc1d318e8f85e!2sSan%20Francisco%20City%20Hall!5e0!3m2!1sen!2sus!4v1620930211516!5m2!1sen!2sus" width="100%" height="100%" style="border:0;" allowfullscreen="" loading="lazy"></iframe>
            </div>
            <div class="mt-6 text-gray-600">
                <p><strong>Address:</strong> 123 Pizza Lane, San Francisco, CA 94102</p>
                <p><strong>Phone:</strong> (123) 456-7890</p>
                <p><strong>Email:</strong> contact@pizzadash.com</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class ContactComponent {
  contactForm = new FormBuilder().group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    message: ['', Validators.required],
  });

  sendMessage() {
    if (this.contactForm.valid) {
      console.log('Message sent:', this.contactForm.value);
      alert('Thank you for your message!');
      this.contactForm.reset();
    }
  }
}
