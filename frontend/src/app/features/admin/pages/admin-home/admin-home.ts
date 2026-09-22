import { ChangeDetectionStrategy, Component } from '@angular/core';

import { PagePlaceholder } from '@shared';

@Component({
  selector: 'app-admin-home',
  imports: [PagePlaceholder],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-placeholder
      title="Admin"
      description="Users, roles and system configuration will live here."
    />
  `,
})
export class AdminHome {}
