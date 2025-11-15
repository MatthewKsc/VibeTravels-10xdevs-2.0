import { Component, DestroyRef, inject, signal, WritableSignal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';

import { AuthApiService, AuthService, AuthValidators, SignIn } from '../../../../core';
import { NotificationService } from '../../../../shared/services';

@Component({
  selector: 'app-sign-in-form',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatCardModule
  ],
  templateUrl: './sign-in-form.html',
  styleUrl: './sign-in-form.scss'
})
export class SignInForm {
  private formBuilder = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private authApiService = inject(AuthApiService);
  private authService = inject(AuthService);

  processingRequest: WritableSignal<boolean> = signal<boolean>(false);
  hidePassword: WritableSignal<boolean> = signal<boolean>(true);
  isFormValid: WritableSignal<boolean> = signal<boolean>(false);

  signInForm = this.formBuilder.group({
    email: ['', [Validators.required, AuthValidators.email()]],
    password: ['', [Validators.required, AuthValidators.password()]]
  });

  constructor() {
    this.signInForm.statusChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        this.isFormValid.set(this.signInForm.valid);
      });
  }

  get email() { return this.signInForm.get('email'); }
  get password() { return this.signInForm.get('password'); }

  togglePasswordVisibility(): void {
    this.hidePassword.update(value => !value);
  }

  signIn(): void {
    if (this.signInForm.invalid) {
      this.signInForm.markAllAsTouched();
      return;
    }

    this.processingRequest.set(true);

    const command: SignIn = {
      email: this.signInForm.value.email!,
      password: this.signInForm.value.password!
    };

    this.authApiService.signIn(command)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.processingRequest.set(false))
      )
      .subscribe({
        next: (response) => {
          this.authService.setToken(response.accessToken);
          this.notificationService.notifySuccess('Signed in successfully!');
          this.router.navigate(['/notes']);
        },
        error: (error) => this.notificationService.notifyError('Invalid email or password. Please try again.'),
      });
  }
}
