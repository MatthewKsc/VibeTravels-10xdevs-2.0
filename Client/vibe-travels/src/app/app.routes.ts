import { Routes } from '@angular/router';
import { authGuard } from './core';

export const routes: Routes = [
    {
        path: 'auth',
        loadChildren: () => import('./modules/auth/auth.route').then(m => m.AUTH_ROUTES)
    },
    {
        path: 'home',
        loadComponent: () => import('./modules/home/home').then(m => m.Home)
    },
    {
        path: 'notes',
        canActivate: [authGuard],
        loadChildren: () => import('./modules/notes/notes.route').then(m => m.NOTES_ROUTES),
    },
    {
        path: 'plans',
        canActivate: [authGuard],
        loadChildren: () => import('./modules/plans/plans.route').then(m => m.PLANS_ROUTE),
    },
    {
        path: 'user',
        canActivate: [authGuard],
        loadChildren: () => import('./modules/user/user.route').then(m => m.USER_ROUTE),
    },
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: '**',
        redirectTo: 'home'
    }
];
