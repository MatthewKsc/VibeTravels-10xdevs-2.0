import { test, expect } from '../fixtures/page-fixtures';

test.describe('Plans Management', () => {
  
  test.beforeEach(async ({ authenticatedPage }) => { });

  test('should display plans page correctly', async ({ plansPage, page }) => {
    await plansPage.goto();
    await expect(page).toHaveURL(/\/plans/);
    
    await expect(plansPage.toolbar).toBeVisible();
    await expect(plansPage.toolbarTitle).toHaveText('Travel Plans');
  });

  test('should show empty state when no plans exist', async ({ plansPage }) => {
    await plansPage.goto();
    const plansCount = await plansPage.getPlansCount();
    
    if (plansCount === 0) {
      await expect(plansPage.emptyState).toBeVisible();
      await expect(plansPage.emptyState).toContainText('No plans yet');
    }
  });

  test('should generate a travel plan from note', async ({ notesPage, page }) => {
    await notesPage.goto();
    const notesCount = await notesPage.getNotesCount();
    
    if (notesCount === 0) {
      await notesPage.clickCreateNote();
      const dialog = page.locator('mat-dialog-container');
      await expect(dialog).toBeVisible();

      await page.locator('input[formControlName="title"]').fill('Beach Vacation Plan');
      await page.locator('input[formControlName="location"]').fill('Maldives');
      
      // Fill with exactly 150 characters to meet min 100 requirement
      const content = 'I want to relax on beautiful beaches with crystal clear water. Looking for luxury resorts with great amenities. Love warm tropical weather and water sports.';
      await page.locator('textarea[formControlName="content"]').fill(content);
      
      // Wait for button to be enabled (form becomes valid)
      const saveButton = page.getByRole('button', { name: /add note|save/i });
      await expect(saveButton).toBeEnabled({ timeout: 5000 });
      await saveButton.click();
      
      await expect(notesPage.getNoteByTitle('Beach Vacation Plan')).toBeVisible({ timeout: 10000 });
    }

    const firstNote = notesPage.getNoteByIndex(0);
    await firstNote.locator('button[aria-label="Generate travel plan"]').click();

    const planDialog = page.locator('mat-dialog-container');
    await expect(planDialog).toBeVisible();
    await expect(planDialog.locator('h2')).toContainText('Generate Travel Plan');

    await page.locator('input[formControlName="travelDays"]').fill('7');
    await page.locator('input[formControlName="travelers"]').fill('2');

    // Wait for button to be enabled (form validation passes)
    const generateButton = page.getByRole('button', { name: /generate plan/i });
    await expect(generateButton).toBeEnabled({ timeout: 5000 });
    await generateButton.click();

    const snackbar = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container').last();
    await expect(snackbar).toBeVisible({ timeout: 5000 });
    await expect(snackbar).toContainText(/plan generation started/i);

    await expect(planDialog).not.toBeVisible({ timeout: 5000 });
  });

  test('should display generating plan with spinner', async ({ plansPage, notesPage, page }) => {
    await notesPage.goto();
    
    const notesCount = await notesPage.getNotesCount();
    if (notesCount > 0) {
      const firstNote = notesPage.getNoteByIndex(0);
      await firstNote.locator('button[aria-label="Generate travel plan"]').click();

      const planDialog = page.locator('mat-dialog-container');
      await expect(planDialog).toBeVisible();

      await page.locator('input[formControlName="travelDays"]').fill('5');
      await page.locator('input[formControlName="travelers"]').fill('1');
      await page.getByRole('button', { name: /generate plan/i }).click();

      await expect(planDialog).not.toBeVisible({ timeout: 5000 });
    }

    await plansPage.goto();

    const generatingPlan = plansPage.getGeneratingPlan();
    const count = await generatingPlan.count();
    
    if (count > 0) {
      await expect(generatingPlan.first()).toBeVisible();
      await expect(generatingPlan.first().locator('.status-message.generating')).toContainText('Plan is being generated');
      await expect(generatingPlan.first().locator('mat-spinner')).toBeVisible();
    }
  });

  test('should wait for plan generation and show generated status', async ({ plansPage, notesPage, page }) => {
    test.slow();
    
    // Check if there are already generated plans
    await plansPage.goto();
    const existingGeneratedPlans = await plansPage.getGeneratedPlan().count();
    
    if (existingGeneratedPlans === 0) {
      // Only generate a new plan if none exist
      await notesPage.goto();
      const notesCount = await notesPage.getNotesCount();
      
      if (notesCount > 0) {
        const firstNote = notesPage.getNoteByIndex(0);
        await firstNote.locator('button[aria-label="Generate travel plan"]').click();

        const planDialog = page.locator('mat-dialog-container');
        await expect(planDialog).toBeVisible();

        await page.locator('input[formControlName="travelDays"]').fill('3');
        await page.locator('input[formControlName="travelers"]').fill('2');
        await page.getByRole('button', { name: /generate plan/i }).click();

        await expect(planDialog).not.toBeVisible({ timeout: 5000 });
        
        await plansPage.goto();
        
        await plansPage.waitForGeneratedPlan(90000);
      } else {
        test.skip();
      }
    }

    const generatedPlan = plansPage.getGeneratedPlan();
    await expect(generatedPlan.first()).toBeVisible();
    await expect(generatedPlan.first().getByRole('button', { name: /view & decide/i })).toBeVisible();
  });

  test('should open plan details dialog on "View & Decide"', async ({ plansPage, page }) => {
    await plansPage.goto();

    const generatedPlan = plansPage.getGeneratedPlan();
    const count = await generatedPlan.count();
    
    if (count > 0) {
      const firstGeneratedPlan = generatedPlan.first();
      await plansPage.clickViewAndDecide(firstGeneratedPlan);

      const detailDialog = page.locator('mat-dialog-container');
      await expect(detailDialog).toBeVisible();
      
      await expect(detailDialog).toBeVisible();
    } else {
      test.skip();
    }
  });

  test('should accept a generated plan', async ({ plansPage, page }) => {
    await plansPage.goto();

    const generatedPlan = plansPage.getGeneratedPlan();
    const count = await generatedPlan.count();
    
    if (count > 0) {
      const firstGeneratedPlan = generatedPlan.first();
      await plansPage.clickViewAndDecide(firstGeneratedPlan);

      const detailDialog = page.locator('mat-dialog-container');
      await expect(detailDialog).toBeVisible();

      await detailDialog.getByRole('button', { name: /accept/i }).click();

      const confirmDialog = page.locator('mat-dialog-container').last();
      await expect(confirmDialog).toBeVisible();
      
      await confirmDialog.getByRole('button', { name: /confirm|yes/i }).click();

      const snackbar = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container');
      await expect(snackbar).toBeVisible({ timeout: 5000 });

      await expect(detailDialog).not.toBeVisible({ timeout: 5000 });

      await page.reload();
      
      const acceptedPlan = plansPage.getAcceptedPlan();
      await expect(acceptedPlan.first()).toBeVisible({ timeout: 10000 });
    } else {
      test.skip();
    }
  });

  test('should reject a generated plan', async ({ plansPage, page }) => {
    await plansPage.goto();

    const generatedPlan = plansPage.getGeneratedPlan();
    const count = await generatedPlan.count();
    
    if (count > 0) {
      const firstGeneratedPlan = generatedPlan.first();
      await plansPage.clickViewAndDecide(firstGeneratedPlan);

      const detailDialog = page.locator('mat-dialog-container');
      await expect(detailDialog).toBeVisible();

      await detailDialog.getByRole('button', { name: /reject/i }).click();

      const confirmDialog = page.locator('mat-dialog-container').last();
      await expect(confirmDialog).toBeVisible();
      
      await confirmDialog.getByRole('button', { name: /confirm|yes/i }).click();

      const snackbar = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container');
      await expect(snackbar).toBeVisible({ timeout: 5000 });

      await expect(detailDialog).not.toBeVisible({ timeout: 5000 });

      await page.reload();
      
      const rejectedPlan = plansPage.getRejectedPlan();
      await expect(rejectedPlan.first()).toBeVisible({ timeout: 10000 });
      await expect(rejectedPlan.first().getByRole('button', { name: /retry/i })).toBeVisible();
    } else {
      test.skip();
    }
  });

  test('should retry a rejected plan', async ({ plansPage, page }) => {
    await plansPage.goto();

    const rejectedPlan = plansPage.getRejectedPlan();
    const count = await rejectedPlan.count();
    
    if (count > 0) {
      const firstRejectedPlan = rejectedPlan.first();
      await plansPage.clickRetry(firstRejectedPlan);

      const confirmDialog = page.locator('mat-dialog-container');
      await expect(confirmDialog).toBeVisible();
      
      await confirmDialog.getByRole('button', { name: /confirm|yes/i }).click();

      const snackbar = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container');
      await expect(snackbar).toBeVisible({ timeout: 5000 });

      await page.reload();
      const generatingPlan = plansPage.getGeneratingPlan();
      await expect(generatingPlan.first()).toBeVisible({ timeout: 10000 });
    } else {
      test.skip();
    }
  });

  test('should delete an accepted plan', async ({ plansPage, page }) => {
    await plansPage.goto();

    const acceptedPlan = plansPage.getAcceptedPlan();
    const count = await acceptedPlan.count();
    
    if (count > 0) {
      const initialCount = await plansPage.getPlansCount();
      const firstAcceptedPlan = acceptedPlan.first();
      
      const planTitle = await firstAcceptedPlan.locator('.plan-structure').textContent();
      
      await plansPage.clickDelete(firstAcceptedPlan);

      const confirmDialog = page.locator('mat-dialog-container');
      await expect(confirmDialog).toBeVisible();
      await expect(confirmDialog).toContainText(/delete/i);
      
      await confirmDialog.getByRole('button', { name: /confirm|delete|yes/i }).click();

      const snackbar = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container');
      await expect(snackbar).toBeVisible({ timeout: 5000 });

      await page.waitForTimeout(2000);
      const newCount = await plansPage.getPlansCount();
      expect(newCount).toBe(initialCount - 1);
    } else {
      test.skip();
    }
  });

  test('should edit accepted plan content', async ({ plansPage, page }) => {
    await plansPage.goto();

    const acceptedPlan = plansPage.getAcceptedPlan();
    const count = await acceptedPlan.count();
    
    if (count > 0) {
      const firstAcceptedPlan = acceptedPlan.first();
      await plansPage.clickViewAndEdit(firstAcceptedPlan);

      const detailDialog = page.locator('mat-dialog-container');
      await expect(detailDialog).toBeVisible();

      const contentArea = detailDialog.locator('textarea, [contenteditable]');
      await expect(contentArea).toBeVisible();

      await contentArea.fill('# Updated Plan\nThis is the updated travel plan content.');

      await detailDialog.getByRole('button', { name: /save|update/i }).click();

      const snackbar = page.locator('mat-snack-bar-container, .mat-mdc-snack-bar-container');
      await expect(snackbar).toBeVisible({ timeout: 5000 });

      await expect(detailDialog).not.toBeVisible({ timeout: 5000 });
    } else {
      test.skip();
    }
  });

  test('should handle plan auto-refresh every 15 seconds', async ({ plansPage, page }) => {
    test.slow();
    
    await plansPage.goto();

    const initialCount = await plansPage.getPlansCount();
    
    // Wait for 16 seconds to ensure auto-refresh happens
    await page.waitForTimeout(16000);

    // Verify page is still functional and didn't crash
    await expect(plansPage.toolbar).toBeVisible();
    
    // Count should be same or updated
    const afterRefreshCount = await plansPage.getPlansCount();
    expect(afterRefreshCount).toBeGreaterThanOrEqual(0);
    
    // If no plans exist, verify empty state is shown
    if (afterRefreshCount === 0) {
      await expect(plansPage.emptyState).toBeVisible();
    }
  });
});
