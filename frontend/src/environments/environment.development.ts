export const environment = {
  production: false,
  /**
   * Relative on purpose: `ng serve` proxies /api to the .NET host (see proxy.conf.json),
   * which keeps the browser on one origin and avoids CORS during development.
   */
  apiBaseUrl: '/api',
};
