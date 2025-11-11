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

import { AuthApiService, AuthValidators, SignUp } from '../../../../core';
import { NotificationService } from '../../../../shared/services';

@Component({
  selector: 'app-sign-up-form',
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
  templateUrl: './sign-up-form.html',
  styleUrl: './sign-up-form.scss'
})
export class SignUpForm {
  private formBuilder = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private authApiService = inject(AuthApiService);

  processingRequest: WritableSignal<boolean> = signal<boolean>(false);
  hidePassword: WritableSignal<boolean> = signal<boolean>(true);
  hideConfirmPassword: WritableSignal<boolean> = signal<boolean>(true);
  isFormValid: WritableSignal<boolean> = signal<boolean>(false);
  passwordsMatch: WritableSignal<boolean> = signal<boolean>(true);

  signUpForm = this.formBuilder.group({
    email: ['', [Validators.required, AuthValidators.email()]],
    password: ['', [Validators.required, AuthValidators.password()]],
    confirmPassword: ['', [Validators.required]]
  });

  constructor() {
    this.signUpForm.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        const password = this.signUpForm.value.password;
        const confirmPassword = this.signUpForm.value.confirmPassword;
        const doPasswordsMatch = !confirmPassword || password === confirmPassword;
        
        this.passwordsMatch.set(doPasswordsMatch);
        this.isFormValid.set(this.signUpForm.valid && doPasswordsMatch);
      });
  }

  get email() { return this.signUpForm.get('email'); }
  get password() { return this.signUpForm.get('password'); }
  get confirmPassword() { return this.signUpForm.get('confirmPassword'); }

  togglePasswordVisibility(): void {
    this.hidePassword.update(value => !value);
  }

  toggleConfirmPasswordVisibility(): void {
    this.hideConfirmPassword.update(value => !value);
  }

  signUp(): void {
    if (this.signUpForm.invalid || !this.passwordsMatch()) {
      this.signUpForm.markAllAsTouched();
      return;
    }

    this.processingRequest.set(true);

    const command: SignUp = {
      email: this.signUpForm.value.email!,
      password: this.signUpForm.value.password!
    };

    this.authApiService.signUp(command)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.processingRequest.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.notifySuccess('Account created successfully! Please sign in.');
          this.router.navigate(['/auth/sign-in']);
        },
        error: (error) => this.notificationService.notifyError('Failed to create account. Please try again.')
      });
  }
}
