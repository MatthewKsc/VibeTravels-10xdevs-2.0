import { Routes } from "@angular/router";

export const USER_ROUTE: Routes = [
  {
    path: 'profile',
    loadComponent: () => import('./components/user-profile/user-profile').then(m => m.UserProfile)
  },
  {
    path: '**',
    redirectTo: 'profile'
  }
];