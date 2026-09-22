import { Injectable, computed, signal } from '@angular/core';

import { AuthSession, AuthUser } from './auth.models';

const SESSION_STORAGE_KEY = 'myerp.session';

/**
 * Holds the current session. Authentication is not implemented yet: this is the
 * seam the guards and the auth interceptor already depend on, so wiring a real
 * sign-in later is a change to this service only.
 */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly session = signal<AuthSession | null>(readStoredSession());

  readonly user = computed<AuthUser | null>(() => this.session()?.user ?? null);
  readonly roles = computed<string[]>(() => this.session()?.user.roles ?? []);
  readonly isAuthenticated = computed(() => this.session() !== null);

  /** Bearer token for the auth interceptor, or null when signed out. */
  get token(): string | null {
    return this.session()?.token ?? null;
  }

  setSession(session: AuthSession): void {
    this.session.set(session);
    writeStoredSession(session);
  }

  clearSession(): void {
    this.session.set(null);
    writeStoredSession(null);
  }

  hasRole(role: string): boolean {
    return this.roles().includes(role);
  }

  hasAnyRole(roles: readonly string[]): boolean {
    return roles.length === 0 || roles.some((role) => this.hasRole(role));
  }
}

function readStoredSession(): AuthSession | null {
  try {
    const raw = localStorage.getItem(SESSION_STORAGE_KEY);
    return raw ? (JSON.parse(raw) as AuthSession) : null;
  } catch {
    // Private mode or corrupt payload: treat as signed out.
    return null;
  }
}

function writeStoredSession(session: AuthSession | null): void {
  try {
    if (session) {
      localStorage.setItem(SESSION_STORAGE_KEY, JSON.stringify(session));
    } else {
      localStorage.removeItem(SESSION_STORAGE_KEY);
    }
  } catch {
    // Storage unavailable: the session simply does not survive a reload.
  }
}
