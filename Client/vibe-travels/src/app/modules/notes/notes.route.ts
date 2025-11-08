import { Routes } from '@angular/router';


export const NOTES_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./components/notes-root/notes-root').then(m => m.NotesRoot)
    },
    {
        path: '**',
        redirectTo: ''
    }
];