/**
 * Normalised shape of a failed API call. Every HTTP failure reaching the
 * application is converted to this by the error interceptor, so callers never
 * have to unpick `HttpErrorResponse` themselves.
 */
export interface ApiError {
  /** HTTP status code, or 0 when the request never reached the server. */
  status: number;
  /** Stable server-side code, e.g. `not_found`. See MyErp.Common ErrorCodes. */
  code: string;
  /** Short heading suitable for a toast title. */
  title: string;
  /** Human readable explanation. */
  detail: string;
  /** Field-level validation messages, keyed by field name. */
  errors?: Record<string, string[]>;
  /** Server correlation id, when the response carried one. */
  traceId?: string;
}

/** Codes the server sends; kept in sync with MyErp.Common ErrorCodes. */
export const ApiErrorCodes = {
  notFound: 'not_found',
  validation: 'validation_failed',
  conflict: 'conflict',
  businessRule: 'business_rule_violated',
  unauthorized: 'unauthorized',
  forbidden: 'forbidden',
  unexpected: 'unexpected_error',
  network: 'network_error',
} as const;

export type ApiErrorCode = (typeof ApiErrorCodes)[keyof typeof ApiErrorCodes];

export function isApiError(value: unknown): value is ApiError {
  return (
    typeof value === 'object' &&
    value !== null &&
    'status' in value &&
    'code' in value &&
    'detail' in value
  );
}
