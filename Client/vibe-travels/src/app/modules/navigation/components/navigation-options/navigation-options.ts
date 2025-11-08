import { ChangeDetectionStrategy, Component, computed, input, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';

import { authenticatedOptions, unauthenticatedOptions } from '../../models/navigation.const';
import type { NavigationOption } from '../../models/navigation.model';

@Component({
  selector: 'app-navigation-options',
  imports: [RouterLink, RouterLinkActive, MatButtonModule, MatIconModule, MatMenuModule],
  templateUrl: './navigation-options.html',
  styleUrl: './navigation-options.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavigationOptions {
  readonly isLoggedIn = input.required<boolean>();

  //TODO In POC we use static options, later replace with dynamic options from API using signals
  readonly authenticatedOptions = signal<NavigationOption[]>(authenticatedOptions);
  readonly unauthenticatedOptions = signal<NavigationOption[]>(unauthenticatedOptions);

  readonly options = computed(() => 
    this.isLoggedIn() ? this.authenticatedOptions() : this.unauthenticatedOptions()
  );

  signOut = (): void => console.log('Sign out clicked');
}
