import { ErrorHandler, Injectable, inject, isDevMode } from '@angular/core';

import { isApiError } from '../models/api-error';
import { NotificationService } from '../services/notification.service';

/**
 * Last line of defence for errors that escape a component or an unhandled
 * rejection. HTTP failures are already reported by the error interceptor, so
 * they are logged here but not shown twice.
 */
@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  private readonly notifications = inject(NotificationService);

  handleError(error: unknown): void {
    const cause = unwrap(error);

    if (isDevMode()) {
      console.error('[GlobalErrorHandler]', cause);
    }

    if (isApiError(cause)) {
      // Already surfaced by errorInterceptor.
      return;
    }

    this.notifications.error(
      'Something went wrong. The action was not completed.',
      'Unexpected error',
    );
  }
}

function unwrap(error: unknown): unknown {
  return error instanceof Error && 'rejection' in error
    ? (error as Error & { rejection: unknown }).rejection
    : error;
}
