import { Page, Locator } from '@playwright/test';
import { BasePage } from './base.page';

/**
 * Notes Page Object Model
 * Represents the notes list and management page
 */
export class NotesPage extends BasePage {
  // Locators
  readonly createNoteButton: Locator;
  readonly notesList: Locator;
  readonly noteItem: Locator;
  readonly searchInput: Locator;
  readonly emptyState: Locator;

  constructor(page: Page) {
    super(page);
    
    // Initialize locators
    this.createNoteButton = page.getByRole('button', { name: /create|add note/i });
    this.notesList = page.locator('[data-testid="notes-list"], .notes-list');
    this.noteItem = page.locator('[data-testid="note-item"], .note-item');
    this.searchInput = page.locator('input[type="search"], input[placeholder*="search" i]');
    this.emptyState = page.locator('[data-testid="empty-state"], .empty-state');
  }

  override async goto() {
    await super.goto('/notes');
  }

  /**
   * Click create note button
   */
  async clickCreateNote() {
    await this.createNoteButton.click();
  }

  /**
   * Get count of notes
   */
  async getNotesCount(): Promise<number> {
    return await this.noteItem.count();
  }

  /**
   * Get note by index
   */
  getNoteByIndex(index: number): Locator {
    return this.noteItem.nth(index);
  }

  /**
   * Get note by title
   */
  getNoteByTitle(title: string): Locator {
    return this.page.locator(`[data-testid="note-item"]:has-text("${title}"), .note-item:has-text("${title}")`);
  }

  /**
   * Check if empty state is visible
   */
  async isEmptyStateVisible(): Promise<boolean> {
    return await this.emptyState.isVisible();
  }

  /**
   * Search for notes
   */
  async searchNotes(query: string) {
    await this.searchInput.fill(query);
  }

  /**
   * Click on a note to view details
   */
  async clickNote(title: string) {
    await this.getNoteByTitle(title).click();
  }
}
