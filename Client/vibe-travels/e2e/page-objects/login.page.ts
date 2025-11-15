import { Page, Locator } from '@playwright/test';
import { BasePage } from './base.page';

/**
 * Login Page Object Model
 * Represents the authentication/sign-in page
 */
export class LoginPage extends BasePage {
  // Locators
  readonly emailInput: Locator;
  readonly passwordInput: Locator;
  readonly signInButton: Locator;
  readonly signUpLink: Locator;
  readonly errorMessage: Locator;

  constructor(page: Page) {
    super(page);
    
    this.emailInput = page.locator('input[type="email"], input[name="email"]');
    this.passwordInput = page.locator('input[type="password"], input[name="password"]');
    this.signInButton = page.getByRole('button', { name: /sign in|login/i });
    this.signUpLink = page.getByRole('link', { name: /sign up|register/i });
    this.errorMessage = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container, .snackbar-error');
  }

  override async goto() {
    await super.goto('/auth/sign-in');
  }

  async login(email: string, password: string) {
    await this.emailInput.fill(email);
    await this.passwordInput.fill(password);
    await this.signInButton.click();
  }

  async isErrorVisible(): Promise<boolean> {
    return await this.errorMessage.isVisible();
  }

  async getErrorText(): Promise<string> {
    await this.errorMessage.waitFor({ state: 'visible', timeout: 5000 });
    return await this.errorMessage.textContent() || '';
  }

  async goToSignUp() {
    await this.signUpLink.click();
  }
}
