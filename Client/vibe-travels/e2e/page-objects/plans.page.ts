import { Page, Locator } from '@playwright/test';
import { BasePage } from './base.page';

/**
 * Plans Page Object Model
 * Represents the travel plans list and management page
 */
export class PlansPage extends BasePage {
  // Locators
  readonly plansList: Locator;
  readonly planCard: Locator;
  readonly emptyState: Locator;
  readonly toolbar: Locator;
  readonly toolbarTitle: Locator;

  constructor(page: Page) {
    super(page);
    
    // Initialize locators
    this.plansList = page.locator('.plans-grid');
    this.planCard = page.locator('.plan-card');
    this.emptyState = page.locator('.empty-state');
    this.toolbar = page.locator('.plans-toolbar');
    this.toolbarTitle = page.locator('.toolbar-title');
  }

  override async goto() {
    await super.goto('/plans');
  }

  /**
   * Get count of plans
   */
  async getPlansCount(): Promise<number> {
    return await this.planCard.count();
  }

  /**
   * Get plan card by index
   */
  getPlanByIndex(index: number): Locator {
    return this.planCard.nth(index);
  }

  /**
   * Get plan card by title
   */
  getPlanByTitle(title: string): Locator {
    return this.planCard.filter({ hasText: title });
  }

  /**
   * Get plan with generating status
   */
  getGeneratingPlan(): Locator {
    return this.planCard.filter({ has: this.page.locator('.status-message.generating') });
  }

  /**
   * Get plan with failed status
   */
  getFailedPlan(): Locator {
    return this.planCard.filter({ has: this.page.locator('.status-message.error') });
  }

  /**
   * Get plan with rejected status
   */
  getRejectedPlan(): Locator {
    return this.planCard.filter({ has: this.page.locator('.status-message.rejected') });
  }

  /**
   * Get plan with accepted status
   */
  getAcceptedPlan(): Locator {
    return this.planCard.filter({ has: this.page.locator('.status-message.accepted') });
  }

  /**
   * Get plan with generated status (ready for decision)
   */
  getGeneratedPlan(): Locator {
    return this.planCard.filter({ 
      has: this.page.getByRole('button', { name: /view & decide/i }) 
    });
  }

  /**
   * Click "View & Decide" button on a plan card
   */
  async clickViewAndDecide(planCard: Locator) {
    await planCard.getByRole('button', { name: /view & decide/i }).click();
  }

  /**
   * Click "View & Edit" button on an accepted plan
   */
  async clickViewAndEdit(planCard: Locator) {
    await planCard.getByRole('button', { name: /view & edit/i }).click();
  }

  /**
   * Click "Retry" button on a failed/rejected plan
   */
  async clickRetry(planCard: Locator) {
    await planCard.getByRole('button', { name: /retry/i }).click();
  }

  /**
   * Click "Delete" button on a plan card
   */
  async clickDelete(planCard: Locator) {
    await planCard.getByRole('button', { name: /delete/i }).click();
  }

  /**
   * Wait for plan generation to complete (status changes from generating)
   * @param timeout Maximum wait time in milliseconds
   */
  async waitForPlanGeneration(timeout: number = 60000): Promise<void> {
    // Wait for the generating status to disappear
    await this.page.waitForFunction(
      () => {
        const generatingCards = document.querySelectorAll('.status-message.generating');
        return generatingCards.length === 0;
      },
      { timeout }
    );
  }

  /**
   * Wait for any plan to have generated status (ready for decision)
   * @param timeout Maximum wait time in milliseconds
   */
  async waitForGeneratedPlan(timeout: number = 60000): Promise<void> {
    await this.page.waitForSelector(
      '.plan-card:has(button:has-text("View & Decide"))',
      { timeout }
    );
  }

  /**
   * Get plan details from card
   */
  async getPlanDetails(planCard: Locator) {
    const title = await planCard.locator('.plan-structure').textContent();
    const detailItems = await planCard.locator('.detail-item span').allTextContents();
    
    return {
      title: title?.trim() || '',
      days: detailItems[0] || '',
      travelers: detailItems[1] || '',
      startDate: detailItems[2] || ''
    };
  }

  /**
   * Check if plan is in generating state
   */
  async isPlanGenerating(planCard: Locator): Promise<boolean> {
    return await planCard.locator('.status-message.generating').isVisible();
  }

  /**
   * Check if plan is in failed state
   */
  async isPlanFailed(planCard: Locator): Promise<boolean> {
    return await planCard.locator('.status-message.error').isVisible();
  }

  /**
   * Check if plan is in rejected state
   */
  async isPlanRejected(planCard: Locator): Promise<boolean> {
    return await planCard.locator('.status-message.rejected').isVisible();
  }

  /**
   * Check if plan is in accepted state
   */
  async isPlanAccepted(planCard: Locator): Promise<boolean> {
    return await planCard.locator('.status-message.accepted').isVisible();
  }
}
