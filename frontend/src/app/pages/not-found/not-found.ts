import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';

import { PagePlaceholder } from '@shared';

@Component({
  selector: 'app-not-found',
  imports: [PagePlaceholder, ButtonModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-placeholder
      title="Page not found"
      description="The page you asked for does not exist."
    >
      <p-button label="Back to dashboard" routerLink="/" [text]="true" />
    </app-page-placeholder>
  `,
})
export class NotFound {}
