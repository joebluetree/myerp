import { ChangeDetectionStrategy, Component } from '@angular/core';

import { PagePlaceholder } from '@shared';

@Component({
  selector: 'app-masters-home',
  imports: [PagePlaceholder],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-placeholder
      title="Masters"
      description="Shared reference data such as item groups, units and parties will live here."
    />
  `,
})
export class MastersHome {}
