import { Injectable, inject } from '@angular/core';
import { MessageService } from 'primeng/api';

import { ApiError } from '../models/api-error';

/**
 * Single place the application raises user-visible feedback. Wrapping PrimeNG's
 * MessageService keeps toast keys, severities and lifetimes consistent, and
 * means feature code never talks to the toast widget directly.
 */
@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly messages = inject(MessageService);

  success(detail: string, summary = 'Success'): void {
    this.messages.add({ severity: 'success', summary, detail, life: 3000 });
  }

  info(detail: string, summary = 'Information'): void {
    this.messages.add({ severity: 'info', summary, detail, life: 4000 });
  }

  warn(detail: string, summary = 'Warning'): void {
    this.messages.add({ severity: 'warn', summary, detail, life: 6000 });
  }

  error(detail: string, summary = 'Error'): void {
    this.messages.add({ severity: 'error', summary, detail, life: 8000 });
  }

  /** Shows an API failure, expanding field-level validation messages. */
  apiError(error: ApiError): void {
    const fieldMessages = Object.values(error.errors ?? {}).flat();
    const detail = fieldMessages.length ? fieldMessages.join(' ') : error.detail;
    this.error(detail, error.title);
  }

  clear(): void {
    this.messages.clear();
  }
}
