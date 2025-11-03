
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, ReactiveFormsModule, RouterLink]
})
export class RegisterComponent {
  // FIX: Explicitly type `fb` as `FormBuilder` to fix type inference issue.
  private fb: FormBuilder = inject(FormBuilder);
  private authService = inject(AuthService);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  registerForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });
  
  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
    this.isLoading.set(true);
    this.errorMessage.set(null);
    
    const { name, email, password } = this.registerForm.value;

    this.authService.register({ name: name!, email: email!, password: password! }).subscribe({
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
