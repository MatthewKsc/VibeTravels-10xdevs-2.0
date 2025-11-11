import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NavigationOptions } from '../navigation-options/navigation-options';
import { NavigationTheme } from '../navigation-theme/navigation-theme';

@Component({
  selector: 'app-navigation-root',
  imports: [RouterLink, NavigationOptions, NavigationTheme],
  templateUrl: './navigation-root.html',
  styleUrl: './navigation-root.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavigationRoot {
  // TODO: Replace with actual auth service
  readonly isLoggedIn = signal(true);
  readonly appName = 'VibeTravels';
}
