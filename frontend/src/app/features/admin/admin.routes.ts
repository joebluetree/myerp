import { Routes } from '@angular/router';

/**
 * Routes for the Admin area. Loaded lazily by the root router, so nothing in
 * this folder is downloaded until the user navigates into /admin.
 *
 * Add one child route per feature as features are built, keeping the paths in
 * step with the controllers in the MyErp.Admin backend module.
 */
export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/admin-home/admin-home').then((m) => m.AdminHome),
    title: 'Admin',
  },
];
