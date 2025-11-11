import { ChangeDetectionStrategy, Component, computed, inject, input, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';

import { authenticatedOptions, unauthenticatedOptions } from '../../models/navigation.const';
import type { NavigationOption } from '../../models/navigation.model';
import { AuthService } from '../../../../core';

@Component({
  selector: 'app-navigation-options',
  imports: [RouterLink, RouterLinkActive, MatButtonModule, MatIconModule, MatMenuModule],
  templateUrl: './navigation-options.html',
  styleUrl: './navigation-options.scss',
})
export class NavigationOptions {
  private authService = inject(AuthService);
  private router = inject(Router);
  
  readonly isLoggedIn = input.required<boolean>();

  //TODO In POC we use static options, later replace with dynamic options from API using signals
  readonly authenticatedOptions = signal<NavigationOption[]>(authenticatedOptions);
  readonly unauthenticatedOptions = signal<NavigationOption[]>(unauthenticatedOptions);

  readonly options = computed(() => 
    this.isLoggedIn() ? this.authenticatedOptions() : this.unauthenticatedOptions()
  );

  signOut = (): void => {
    this.authService.clearToken();
    this.router.navigate(['/home']);
  };
}
