import { Routes } from "@angular/router";

export const PLANS_ROUTE: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/plans-root/plans-root').then(m => m.PlansRoot)
  },
  {
    path: '**',
    redirectTo: ''
  }
];