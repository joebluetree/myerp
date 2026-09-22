import { ChangeDetectionStrategy, Component } from '@angular/core';

import { PagePlaceholder } from '@shared';

@Component({
  selector: 'app-dashboard',
  imports: [PagePlaceholder],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-placeholder
      title="Dashboard"
      description="Pick a module from the navigation to get started."
    />
  `,
})
export class Dashboard {}
