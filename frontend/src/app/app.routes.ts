import { Routes } from '@angular/router';

import { MainLayout } from './layout/main-layout/main-layout';

/**
 * Root routing table. Every business area is a lazily loaded child of the shell,
 * one per backend module, so a module's screens ship in their own chunk.
 *
 * `authGuard` / `roleGuard` from '@core' are ready to be attached here once
 * authentication is wired up — for example:
 *
 * ```ts
 * { path: 'admin', canActivate: [roleGuard], data: { roles: ['Administrator'] }, ... }
 * ```
 */
export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
      {
        path: '',
        pathMatch: 'full',
        loadComponent: () => import('./pages/dashboard/dashboard').then((m) => m.Dashboard),
        title: 'MyErp',
      },
      {
        path: 'masters',
        loadChildren: () => import('./features/masters/masters.routes').then((m) => m.MASTERS_ROUTES),
      },
      {
        path: 'inventory',
        loadChildren: () =>
          import('./features/inventory/inventory.routes').then((m) => m.INVENTORY_ROUTES),
      },
      {
        path: 'accounts',
        loadChildren: () =>
          import('./features/accounts/accounts.routes').then((m) => m.ACCOUNTS_ROUTES),
      },
      {
        path: 'admin',
        loadChildren: () => import('./features/admin/admin.routes').then((m) => m.ADMIN_ROUTES),
      },
      {
        path: 'forbidden',
        loadComponent: () => import('./pages/forbidden/forbidden').then((m) => m.Forbidden),
        title: 'Not allowed',
      },
      {
        path: '**',
        loadComponent: () => import('./pages/not-found/not-found').then((m) => m.NotFound),
        title: 'Page not found',
      },
    ],
  },
];
