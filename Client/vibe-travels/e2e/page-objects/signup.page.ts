import { Page, Locator } from '@playwright/test';
import { BasePage } from './base.page';

/**
 * Sign Up Page Object Model
 * Represents the registration/sign-up page
 */
export class SignUpPage extends BasePage {
  // Locators
  readonly emailInput: Locator;
  readonly passwordInput: Locator;
  readonly confirmPasswordInput: Locator;
  readonly createAccountButton: Locator;
  readonly signInLink: Locator;
  readonly successMessage: Locator;

  constructor(page: Page) {
    super(page);
    
    this.emailInput = page.locator('input[type="email"], input[formControlName="email"]');
    this.passwordInput = page.locator('input[formControlName="password"]').first();
    this.confirmPasswordInput = page.locator('input[formControlName="confirmPassword"]');
    this.createAccountButton = page.getByRole('button', { name: /create account|sign up/i });
    // Target the sign-in link in the auth footer specifically
    this.signInLink = page.locator('.auth-footer a[routerLink="/auth/sign-in"]');
    this.successMessage = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container, .snackbar-success');
  }

  override async goto() {
    await super.goto('/auth/sign-up');
  }

  async signUp(email: string, password: string, confirmPassword: string) {
    await this.emailInput.fill(email);
    await this.passwordInput.fill(password);
    await this.confirmPasswordInput.fill(confirmPassword);
    await this.createAccountButton.click();
  }

  async isSuccessVisible(): Promise<boolean> {
    return await this.successMessage.isVisible();
  }

  async getSuccessText(): Promise<string> {
    await this.successMessage.waitFor({ state: 'visible', timeout: 5000 });
    return await this.successMessage.textContent() || '';
  }

  async goToSignIn() {
    await this.signInLink.click();
  }
}
