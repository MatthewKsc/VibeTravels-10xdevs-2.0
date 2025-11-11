import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.authState$.pipe(
    map(isAuthenticated => {
      if (!isAuthenticated) {
        router.navigate(['/auth/sign-in']);
        return false;
      }
      return true;
    })
  );
};
