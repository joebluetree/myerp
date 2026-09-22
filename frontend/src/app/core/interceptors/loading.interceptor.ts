import { HttpContextToken, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';

import { LoadingService } from '../services/loading.service';

/**
 * Opt out for background polling that should not flicker the global indicator:
 *
 * ```ts
 * api.get('items', { context: new HttpContext().set(SKIP_LOADING, true) });
 * ```
 */
export const SKIP_LOADING = new HttpContextToken<boolean>(() => false);

export const loadingInterceptor: HttpInterceptorFn = (request, next) => {
  if (request.context.get(SKIP_LOADING)) {
    return next(request);
  }

  const loading = inject(LoadingService);
  loading.start();

  return next(request).pipe(finalize(() => loading.stop()));
};
