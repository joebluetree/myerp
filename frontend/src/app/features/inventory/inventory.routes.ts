import { Routes } from '@angular/router';

/**
 * Routes for the Inventory area. Loaded lazily by the root router, so nothing in
 * this folder is downloaded until the user navigates into /inventory.
 *
 * Add one child route per feature as features are built, keeping the paths in
 * step with the controllers in the MyErp.Inventory backend module.
 */
export const INVENTORY_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/inventory-home/inventory-home').then((m) => m.InventoryHome),
    title: 'Inventory',
  },
];
