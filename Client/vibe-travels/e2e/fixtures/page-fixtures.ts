import { test as base, expect } from '@playwright/test';
import { LoginPage } from '../page-objects/login.page';
import { NotesPage } from '../page-objects/notes.page';
import { SignUpPage } from '../page-objects/signup.page';

type CustomFixtures = {
  loginPage: LoginPage;
  notesPage: NotesPage;
  signUpPage: SignUpPage;
};

export const test = base.extend<CustomFixtures>({
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
});

export { expect };
