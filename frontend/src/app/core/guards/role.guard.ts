import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthService } from '../auth/auth.service';

/**
 * Restricts a route to the roles listed on its `data.roles`:
 *
 * ```ts
 * { path: 'users', canActivate: [roleGuard], data: { roles: ['Administrator'] } }
 * ```
 *
 * A route with no roles is treated as "any authenticated user". This is a
 * navigation convenience only — the API enforces the real rules.
 */
export const roleGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (!auth.isAuthenticated()) {
    return router.createUrlTree(['/auth/login'], {
      queryParams: { returnUrl: state.url },
    });
  }

  const roles = (route.data?.['roles'] as string[] | undefined) ?? [];

  return auth.hasAnyRole(roles) ? true : router.createUrlTree(['/forbidden']);
};
