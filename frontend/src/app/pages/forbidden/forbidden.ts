import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';

import { PagePlaceholder } from '@shared';

@Component({
  selector: 'app-forbidden',
  imports: [PagePlaceholder, ButtonModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-placeholder
      title="Not allowed"
      description="Your account does not have access to this area."
    >
      <p-button label="Back to dashboard" routerLink="/" [text]="true" />
    </app-page-placeholder>
  `,
})
export class Forbidden {}
