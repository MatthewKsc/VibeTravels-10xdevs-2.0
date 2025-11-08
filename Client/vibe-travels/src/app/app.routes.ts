import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'home',
        loadComponent: () => import('./modules/home/home').then(m => m.Home)
    },
    {
        path: 'notes',
        loadChildren: () => import('./modules/notes/notes.route').then(m => m.NOTES_ROUTES)
    },
    {
        path: 'plans',
        loadChildren: () => import('./modules/plans/plans.route').then(m => m.PLANS_ROUTE)
    },
    {
        path: 'user',
        loadChildren: () => import('./modules/user/user.route').then(m => m.USER_ROUTE)
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
