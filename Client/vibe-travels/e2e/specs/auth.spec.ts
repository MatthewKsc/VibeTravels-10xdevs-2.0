import { test, expect } from '../fixtures/page-fixtures';

test.describe('Authentication Flow', () => {
  
  test.describe('Sign In', () => {
    test.beforeEach(async ({ loginPage }) => {
      await loginPage.goto();
    });

    test('should display login page correctly', async ({ loginPage, page }) => {
      await expect(page).toHaveTitle(/VibeTravels|Sign In/i);
      
      await expect(loginPage.emailInput).toBeVisible();
      await expect(loginPage.passwordInput).toBeVisible();
      await expect(loginPage.signInButton).toBeVisible();
    });

    test('should show error message for invalid credentials', async ({ loginPage }) => {
      await loginPage.login('invalid@example.com', 'WrongPass123');

      await expect(loginPage.errorMessage).toBeVisible();
      
      const errorText = await loginPage.getErrorText();
      expect(errorText.toLowerCase()).toContain('invalid');
    });

    test('should navigate to sign up page', async ({ loginPage, page }) => {
      await loginPage.goToSignUp();
      
      await expect(page).toHaveURL(/\/auth\/sign-up/);
    });

    test('should successfully login with valid credentials', async ({ loginPage, page }) => {
      await loginPage.login('testuser@gmail.com', 'cJ6QFLY*ch');

      await expect(page).toHaveURL(/\/(notes|dashboard)/);
    });
  });

  test.describe('Sign Up', () => {
    test.beforeEach(async ({ signUpPage }) => {
      await signUpPage.goto();
    });

    test('should display sign up page correctly', async ({ signUpPage, page }) => {
      await expect(page).toHaveURL(/\/auth\/sign-up/);
      
      await expect(signUpPage.emailInput).toBeVisible();
      await expect(signUpPage.passwordInput).toBeVisible();
      await expect(signUpPage.confirmPasswordInput).toBeVisible();
      await expect(signUpPage.createAccountButton).toBeVisible();
    });

    test('should register a new user and redirect to sign in', async ({ signUpPage, page }) => {
      const timestamp = Date.now();
      const testEmail = `test${timestamp}@example.com`;
      const testPassword = 'ValidPassword123!';

      await signUpPage.signUp(testEmail, testPassword, testPassword);

      await expect(signUpPage.successMessage).toBeVisible();
      
      const successText = await signUpPage.getSuccessText();
      expect(successText.toLowerCase()).toMatch(/success|account created|registered/);

      await expect(page).toHaveURL(/\/auth\/sign-in/, { timeout: 10000 });
    });

    test('should navigate to sign in page', async ({ signUpPage, page }) => {
      await signUpPage.goToSignIn();
      
      await expect(page).toHaveURL(/\/auth\/sign-in/);
    });
  });
});
