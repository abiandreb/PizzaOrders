import { Component, ChangeDetectionStrategy, input, output, signal, inject, viewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginModalComponent {
  isVisible = input.required<boolean>();
  closeModal = output<void>();
  loginSuccess = output<void>();

  authService = inject(AuthService);

  email = viewChild<ElementRef<HTMLInputElement>>('email');
  password = viewChild<ElementRef<HTMLInputElement>>('password');

  isProcessing = signal(false);
  error = signal<string | null>(null);

  onClose() {
    if (this.isProcessing()) return;
    this.error.set(null);
    this.closeModal.emit();
  }

  async processLogin(event: Event) {
    event.preventDefault();
    if (this.isProcessing()) return;

    this.isProcessing.set(true);
    this.error.set(null);
    
    const emailVal = this.email()?.nativeElement.value ?? '';
    const passwordVal = this.password()?.nativeElement.value ?? '';

    if (!emailVal) {
      this.error.set('Please enter a valid email address.');
      this.isProcessing.set(false);
      return;
    }

    try {
      await this.authService.login(emailVal, passwordVal);
      this.loginSuccess.emit();
    } catch (err) {
      this.error.set('Login failed. Please try again.');
    } finally {
      this.isProcessing.set(false);
    }
  }
}
