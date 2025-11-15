import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { NavigationOptions } from '../navigation-options/navigation-options';
import { NavigationTheme } from '../navigation-theme/navigation-theme';
import { AuthService } from '../../../../core';

@Component({
  selector: 'app-navigation-root',
  imports: [RouterLink, NavigationOptions, NavigationTheme],
  templateUrl: './navigation-root.html',
  styleUrl: './navigation-root.scss',
})
export class NavigationRoot {
  private authService = inject(AuthService);
  
  readonly isLoggedIn = toSignal(this.authService.authState$, { initialValue: false });
  readonly appName = 'VibeTravels';
}
