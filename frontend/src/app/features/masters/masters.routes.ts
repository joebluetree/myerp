import { Routes } from '@angular/router';

/**
 * Routes for the Masters area. Loaded lazily by the root router, so nothing in
 * this folder is downloaded until the user navigates into /masters.
 *
 * Add one child route per feature as features are built, keeping the paths in
 * step with the controllers in the MyErp.Masters backend module.
 */
export const MASTERS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/masters-home/masters-home').then((m) => m.MastersHome),
    title: 'Masters',
  },
];
