import { test, expect } from '../fixtures/page-fixtures';

test.describe('Notes Management', () => {
  
  test.beforeEach(async ({ authenticatedPage }) => { });

  test('should display notes list page', async ({ notesPage, page }) => {
    await notesPage.goto();
    await expect(page).toHaveURL(/\/notes/);
    
    await expect(notesPage.createNoteButton).toBeVisible();
  });

  test('should show empty state when no notes exist', async ({ notesPage }) => {
    await notesPage.goto();
    const notesCount = await notesPage.getNotesCount();
    
    if (notesCount === 0) {
      await expect(notesPage.emptyState).toBeVisible();
    }
  });

  test('should open create note dialog', async ({ notesPage, page }) => {
    await notesPage.goto();
    await notesPage.clickCreateNote();

    const dialog = page.locator('mat-dialog-container, [role="dialog"]');
    await expect(dialog).toBeVisible();
  });

  test('should create a new note', async ({ notesPage, page }) => {
    await notesPage.goto();
    await notesPage.clickCreateNote();

    const dialog = page.locator('mat-dialog-container');
    await expect(dialog).toBeVisible();

    await page.locator('input[formControlName="title"]').fill('Test Trip to Paris');
    await page.locator('input[formControlName="location"]').fill('Paris, France');
    await page.locator('textarea[formControlName="content"]').fill('Want to explore the romantic city of Paris. Visit the Eiffel Tower, Louvre Museum, and enjoy authentic French cuisine. Looking for cozy cafes and beautiful architecture.');

    await page.getByRole('button', { name: /add note|save/i }).click();

    const snackbar = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container');
    await expect(snackbar).toBeVisible({ timeout: 5000 });

    await expect(dialog).not.toBeVisible({ timeout: 5000 });
    
    await expect(notesPage.getNoteByTitle('Test Trip to Paris')).toBeVisible({ timeout: 10000 });
  });
});
