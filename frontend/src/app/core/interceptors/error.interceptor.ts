import { HttpContextToken, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../auth/auth.service';
import { ApiError, ApiErrorCodes } from '../models/api-error';
import { ProblemDetails } from '../models/problem-details';
import { NotificationService } from '../services/notification.service';

/**
 * Suppresses the automatic toast when a caller wants to render the failure
 * itself (inline form errors, for example):
 *
 * ```ts
 * api.post('item-groups', body, { context: new HttpContext().set(SKIP_ERROR_NOTIFICATION, true) });
 * ```
 */
export const SKIP_ERROR_NOTIFICATION = new HttpContextToken<boolean>(() => false);

/**
 * Converts every HTTP failure into an {@link ApiError}, reports it once, and
 * rethrows it. Downstream code therefore only ever sees the normalised shape.
 */
export const errorInterceptor: HttpInterceptorFn = (request, next) => {
  const notifications = inject(NotificationService);
  const auth = inject(AuthService);
  const router = inject(Router);

  return next(request).pipe(
    catchError((response: unknown) => {
      const error = toApiError(response);

      if (error.status === 401) {
        auth.clearSession();
        void router.navigate(['/auth/login'], {
          queryParams: { returnUrl: router.url },
        });
      } else if (error.status === 403) {
        void router.navigate(['/forbidden']);
      }

      if (!request.context.get(SKIP_ERROR_NOTIFICATION)) {
        notifications.apiError(error);
      }

      return throwError(() => error);
    }),
  );
};

function toApiError(response: unknown): ApiError {
  if (!(response instanceof HttpErrorResponse)) {
    return {
      status: 0,
      code: ApiErrorCodes.unexpected,
      title: 'Unexpected error',
      detail: 'Something went wrong. Please try again.',
    };
  }

  // status 0 means the request never got a response: offline, DNS, CORS, or the API is down.
  if (response.status === 0) {
    return {
      status: 0,
      code: ApiErrorCodes.network,
      title: 'Cannot reach the server',
      detail: 'The API did not respond. Check your connection and try again.',
    };
  }

  const problem: ProblemDetails =
    typeof response.error === 'object' && response.error !== null ? response.error : {};

  return {
    status: response.status,
    code: problem.code ?? defaultCodeFor(response.status),
    title: problem.title ?? defaultTitleFor(response.status),
    detail: problem.detail ?? response.message ?? 'The request could not be completed.',
    errors: problem.errors,
    traceId: problem.traceId,
  };
}

function defaultCodeFor(status: number): string {
  switch (status) {
    case 400:
      return ApiErrorCodes.validation;
    case 401:
      return ApiErrorCodes.unauthorized;
    case 403:
      return ApiErrorCodes.forbidden;
    case 404:
      return ApiErrorCodes.notFound;
    case 409:
      return ApiErrorCodes.conflict;
    case 422:
      return ApiErrorCodes.businessRule;
    default:
      return ApiErrorCodes.unexpected;
  }
}

function defaultTitleFor(status: number): string {
  switch (status) {
    case 400:
      return 'Validation failed';
    case 401:
      return 'Session expired';
    case 403:
      return 'Not allowed';
    case 404:
      return 'Not found';
    case 409:
      return 'Conflict';
    case 422:
      return 'Business rule violated';
    default:
      return status >= 500 ? 'Server error' : 'Request failed';
  }
}
