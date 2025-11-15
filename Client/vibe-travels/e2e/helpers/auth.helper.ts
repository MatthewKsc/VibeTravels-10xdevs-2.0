import { Page } from '@playwright/test';

export interface TestUser {
  email: string;
  password: string;
}


export function createTestUser(): TestUser {
  const timestamp = Date.now();
  return {
    email: `test${timestamp}@example.com`,
    password: 'ValidPassword123!',
  };
}

export async function registerUser(page: Page, user: TestUser): Promise<void> {
  await page.goto('/auth/sign-up');
  
  await page.locator('input[type="email"], input[formControlName="email"]').fill(user.email);
  await page.locator('input[formControlName="password"]').first().fill(user.password);
  await page.locator('input[formControlName="confirmPassword"]').fill(user.password);
  await page.getByRole('button', { name: /create account|sign up/i }).click();
  
  const snackbar = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container');
  await snackbar.waitFor({ state: 'visible', timeout: 5000 });
  await page.waitForURL(/\/auth\/sign-in/, { timeout: 10000 });
}

export async function loginUser(page: Page, user: TestUser): Promise<void> {
  await page.goto('/auth/sign-in');
  
  await page.locator('input[type="email"], input[name="email"]').fill(user.email);
  await page.locator('input[type="password"], input[name="password"]').fill(user.password);
  await page.getByRole('button', { name: /sign in|login/i }).click();
  
  await page.waitForURL(/\/(notes|dashboard)/, { timeout: 10000 });
}
