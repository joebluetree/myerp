import { HttpClient, HttpContext, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { PagedRequest, PagedResult } from '../models/paging';

type QueryValue = string | number | boolean | Date | null | undefined;

export type QueryParams = Record<string, QueryValue | QueryValue[]>;

export interface ApiRequestOptions {
  params?: QueryParams;
  /** Carries interceptor flags such as SKIP_LOADING or SKIP_ERROR_NOTIFICATION. */
  context?: HttpContext;
}

/**
 * Transport layer over HttpClient: resolves relative paths against the
 * configured API base URL and normalises query parameters. Feature services
 * build on this rather than injecting HttpClient directly, so the base URL and
 * parameter conventions live in exactly one place.
 */
@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl.replace(/\/+$/, '');

  get<T>(path: string, options?: ApiRequestOptions): Observable<T> {
    return this.http.get<T>(this.url(path), this.request(options));
  }

  getPaged<T>(
    path: string,
    request: PagedRequest,
    options?: ApiRequestOptions,
  ): Observable<PagedResult<T>> {
    return this.get<PagedResult<T>>(path, {
      ...options,
      params: { ...options?.params, ...request },
    });
  }

  post<T>(path: string, body?: unknown, options?: ApiRequestOptions): Observable<T> {
    return this.http.post<T>(this.url(path), body ?? {}, this.request(options));
  }

  put<T>(path: string, body?: unknown, options?: ApiRequestOptions): Observable<T> {
    return this.http.put<T>(this.url(path), body ?? {}, this.request(options));
  }

  patch<T>(path: string, body?: unknown, options?: ApiRequestOptions): Observable<T> {
    return this.http.patch<T>(this.url(path), body ?? {}, this.request(options));
  }

  delete<T>(path: string, options?: ApiRequestOptions): Observable<T> {
    return this.http.delete<T>(this.url(path), this.request(options));
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path.replace(/^\/+/, '')}`;
  }

  private request(options?: ApiRequestOptions): { params: HttpParams; context?: HttpContext } {
    return {
      params: toHttpParams(options?.params),
      context: options?.context,
    };
  }
}

function toHttpParams(params?: QueryParams): HttpParams {
  let httpParams = new HttpParams();
  if (!params) {
    return httpParams;
  }

  for (const [key, value] of Object.entries(params)) {
    for (const item of Array.isArray(value) ? value : [value]) {
      if (item === null || item === undefined || item === '') {
        continue;
      }
      httpParams = httpParams.append(
        key,
        item instanceof Date ? item.toISOString() : String(item),
      );
    }
  }

  return httpParams;
}
