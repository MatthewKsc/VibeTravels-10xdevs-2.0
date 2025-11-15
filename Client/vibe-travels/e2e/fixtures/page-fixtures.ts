import { test as base, expect } from '@playwright/test';
import { LoginPage } from '../page-objects/login.page';
import { NotesPage } from '../page-objects/notes.page';
import { SignUpPage } from '../page-objects/signup.page';
import { PlansPage } from '../page-objects/plans.page';
import { UserProfilePage } from '../page-objects/user-profile.page';
import { TestUser, createTestUser, registerUser, loginUser } from '../helpers/auth.helper';

type CustomFixtures = {
  loginPage: LoginPage;
  notesPage: NotesPage;
  signUpPage: SignUpPage;
  plansPage: PlansPage;
  userProfilePage: UserProfilePage;
  testUser: TestUser;
  authenticatedPage: void;
};

type WorkerFixtures = {
  workerTestUser: TestUser;
};

export const test = base.extend<CustomFixtures, WorkerFixtures>({
  // Worker-scoped fixture: creates one test user per worker
  workerTestUser: [async ({ browser }, use) => {
    const user = createTestUser();
    const context = await browser.newContext();
    const page = await context.newPage();
    
    // Register the user once per worker
    await registerUser(page, user);
    
    await page.close();
    await context.close();
    
    await use(user);
  }, { scope: 'worker' }],

  // Test-scoped fixture: provides the worker's test user to each test
  testUser: async ({ workerTestUser }, use) => {
    await use(workerTestUser);
  },

  // Fixture that automatically logs in before each test
  authenticatedPage: async ({ page, testUser }, use) => {
    await loginUser(page, testUser);
    await use();
  },

  loginPage: async ({ page }, use) => {
    const loginPage = new LoginPage(page);
    await use(loginPage);
  },

  notesPage: async ({ page }, use) => {
    const notesPage = new NotesPage(page);
    await use(notesPage);
  },

  signUpPage: async ({ page }, use) => {
    const signUpPage = new SignUpPage(page);
    await use(signUpPage);
  },

  plansPage: async ({ page }, use) => {
    const plansPage = new PlansPage(page);
    await use(plansPage);
  },

  userProfilePage: async ({ page }, use) => {
    const userProfilePage = new UserProfilePage(page);
    await use(userProfilePage);
  },
});

export { expect };
