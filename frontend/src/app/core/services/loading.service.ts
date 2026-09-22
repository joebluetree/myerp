import { Injectable, computed, signal } from '@angular/core';

/**
 * Counts in-flight HTTP requests so the shell can show a single global
 * indicator. A counter rather than a boolean, so overlapping requests do not
 * clear each other's state.
 */
@Injectable({ providedIn: 'root' })
export class LoadingService {
  private readonly pending = signal(0);

  readonly isLoading = computed(() => this.pending() > 0);

  start(): void {
    this.pending.update((count) => count + 1);
  }

  stop(): void {
    this.pending.update((count) => (count > 0 ? count - 1 : 0));
  }

  reset(): void {
    this.pending.set(0);
  }
}
