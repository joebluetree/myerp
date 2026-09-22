import { ChangeDetectionStrategy, Component } from '@angular/core';

import { PagePlaceholder } from '@shared';

@Component({
  selector: 'app-inventory-home',
  imports: [PagePlaceholder],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-placeholder
      title="Inventory"
      description="Stock movements, transfers and valuation will live here."
    />
  `,
})
export class InventoryHome {}
