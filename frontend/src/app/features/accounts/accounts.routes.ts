import { Routes } from '@angular/router';

/**
 * Routes for the Accounts area. Loaded lazily by the root router, so nothing in
 * this folder is downloaded until the user navigates into /accounts.
 *
 * Add one child route per feature as features are built, keeping the paths in
 * step with the controllers in the MyErp.Accounts backend module.
 */
export const ACCOUNTS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/accounts-home/accounts-home').then((m) => m.AccountsHome),
    title: 'Accounts',
  },
];
