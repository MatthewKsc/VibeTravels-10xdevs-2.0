import { Routes } from "@angular/router";

export const PLANS_ROUTE: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/profile-root/profile-root').then(m => m.ProfileRoot)
  },
  {
    path: '**',
    redirectTo: ''
  }
];