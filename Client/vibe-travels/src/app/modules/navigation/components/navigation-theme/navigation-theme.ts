import { ChangeDetectionStrategy, Component, effect, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Theme } from '../../models/navigation.type';

@Component({
  selector: 'app-navigation-theme',
  imports: [MatIconModule, MatButtonModule, MatTooltipModule],
  templateUrl: './navigation-theme.html',
  styleUrl: './navigation-theme.scss',
})
export class NavigationTheme {
  readonly currentTheme = signal<Theme>('light');

  constructor() {
    this.setTheme();

    effect(() => {
      const theme = this.currentTheme();

      this.applyTheme(theme);

      localStorage.setItem('theme', theme);
    });
  }

  toggleTheme = (): void => this.currentTheme.update(current => current === 'light' ? 'dark' : 'light');

  private setTheme(): void {
    const savedTheme = localStorage.getItem('theme') as Theme | null;

    if (savedTheme) {
      this.currentTheme.set(savedTheme);
    }
  }

  private applyTheme(theme: Theme): void {
    if (theme === 'dark') {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }
}
