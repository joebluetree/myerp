import { ChangeDetectionStrategy, Component } from '@angular/core';

import { PagePlaceholder } from '@shared';

@Component({
  selector: 'app-accounts-home',
  imports: [PagePlaceholder],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-placeholder
      title="Accounts"
      description="Ledgers, vouchers and financial reporting will live here."
    />
  `,
})
export class AccountsHome {}
