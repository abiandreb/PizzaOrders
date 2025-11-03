
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, ReactiveFormsModule, RouterLink]
})
export class LoginComponent {
  // FIX: Explicitly type `fb` as `FormBuilder` to fix type inference issue.
  private fb: FormBuilder = inject(FormBuilder);
  private authService = inject(AuthService);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  loginForm = this.fb.group({
    email: ['admin@example.com', [Validators.required, Validators.email]],
    password: ['admin123', Validators.required]
  });

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const { email, password } = this.loginForm.value;
    
    this.authService.login(email!, password!).subscribe({
      next: (response) => {
        if (!response.success) {
          this.errorMessage.set(response.message);
        }
        // On success, the service handles navigation
      },
      error: (err) => {
        this.errorMessage.set('An unexpected error occurred. Please try again.');
        console.error(err);
      },
      complete: () => {
        this.isLoading.set(false);
      }
    });
  }
}
