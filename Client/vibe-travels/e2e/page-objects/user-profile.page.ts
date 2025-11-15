import { Page, Locator } from '@playwright/test';
import { BasePage } from './base.page';

/**
 * User Profile Page Object Model
 * Represents the user travel preferences profile page
 */
export class UserProfilePage extends BasePage {
  // Locators
  readonly backButton: Locator;
  readonly pageTitle: Locator;
  readonly loadingSpinner: Locator;
  
  // Form fields
  readonly travelStyleSelect: Locator;
  readonly accommodationSelect: Locator;
  readonly climateSelect: Locator;
  readonly updateButton: Locator;

  constructor(page: Page) {
    super(page);
    
    // Initialize locators
    this.backButton = page.getByRole('button', { name: /go back/i });
    this.pageTitle = page.locator('.profile-header h1');
    this.loadingSpinner = page.locator('.loading-container mat-spinner');
    
    // Form field locators
    this.travelStyleSelect = page.locator('mat-select[formControlName="travelStyle"]');
    this.accommodationSelect = page.locator('mat-select[formControlName="accommodationPreference"]');
    this.climateSelect = page.locator('mat-select[formControlName="climatePreference"]');
    this.updateButton = page.getByRole('button', { name: /update preferences/i });
  }

  override async goto() {
    await super.goto('/user/profile');
  }

  /**
   * Click back button to navigate to notes
   */
  async clickBack() {
    await this.backButton.click();
  }

  /**
   * Select travel style from dropdown
   */
  async selectTravelStyle(style: string) {
    await this.travelStyleSelect.click();
    await this.page.locator(`mat-option`).filter({ hasText: new RegExp(`^${style}$`) }).click();
  }

  /**
   * Select accommodation preference from dropdown
   */
  async selectAccommodation(accommodation: string) {
    await this.accommodationSelect.click();
    await this.page.locator(`mat-option`).filter({ hasText: new RegExp(`^${accommodation}$`) }).click();
  }

  /**
   * Select climate preference from dropdown
   */
  async selectClimate(climate: string) {
    await this.climateSelect.click();
    await this.page.locator(`mat-option`).filter({ hasText: new RegExp(`^${climate}$`) }).click();
  }

  /**
   * Fill complete profile form
   */
  async fillProfile(travelStyle: string, accommodation: string, climate: string) {
    await this.selectTravelStyle(travelStyle);
    await this.selectAccommodation(accommodation);
    await this.selectClimate(climate);
  }

  /**
   * Click update preferences button
   */
  async clickUpdate() {
    await this.updateButton.click();
  }

  /**
   * Update profile with new preferences
   */
  async updateProfile(travelStyle: string, accommodation: string, climate: string) {
    await this.fillProfile(travelStyle, accommodation, climate);
    await this.clickUpdate();
  }

  /**
   * Get current selected travel style
   */
  async getSelectedTravelStyle(): Promise<string> {
    return await this.travelStyleSelect.locator('.mat-mdc-select-value-text').textContent() || '';
  }

  /**
   * Get current selected accommodation
   */
  async getSelectedAccommodation(): Promise<string> {
    return await this.accommodationSelect.locator('.mat-mdc-select-value-text').textContent() || '';
  }

  /**
   * Get current selected climate
   */
  async getSelectedClimate(): Promise<string> {
    return await this.climateSelect.locator('.mat-mdc-select-value-text').textContent() || '';
  }

  /**
   * Check if update button is disabled
   */
  async isUpdateButtonDisabled(): Promise<boolean> {
    return await this.updateButton.isDisabled();
  }

  /**
   * Check if form is loading
   */
  async isLoading(): Promise<boolean> {
    return await this.loadingSpinner.isVisible();
  }

  /**
   * Get validation error for a field
   */
  async getFieldError(fieldName: 'travelStyle' | 'accommodationPreference' | 'climatePreference'): Promise<string | null> {
    const errorLocator = this.page.locator(`mat-form-field:has(mat-select[formControlName="${fieldName}"]) mat-error`);
    return await errorLocator.textContent();
  }

  /**
   * Check if field has error
   */
  async hasFieldError(fieldName: 'travelStyle' | 'accommodationPreference' | 'climatePreference'): Promise<boolean> {
    const errorLocator = this.page.locator(`mat-form-field:has(mat-select[formControlName="${fieldName}"]) mat-error`);
    return await errorLocator.isVisible();
  }
}
