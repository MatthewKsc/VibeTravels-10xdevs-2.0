import { Routes } from "@angular/router";

export const AUTH_ROUTES: Routes = [
  {
    path: 'sign-in',
    loadComponent: () => import('./components/sign-in-form/sign-in-form').then(m => m.SignInForm)
  },
  {
    path: 'sign-up',
    loadComponent: () => import('./components/sign-up-form/sign-up-form').then(m => m.SignUpForm)
  },
  {
    path: '',
    redirectTo: 'sign-in',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'sign-in'
  }
];
