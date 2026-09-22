import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { CardModule } from 'primeng/card';

/**
 * Stand-in for a screen that has not been built yet. Feature areas use it so
 * the routing skeleton is navigable before any ERP feature exists; delete the
 * usage as each real screen lands.
 */
@Component({
  selector: 'app-page-placeholder',
  imports: [CardModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <p-card [header]="title()">
      <p class="placeholder__text">{{ description() }}</p>
      <ng-content />
    </p-card>
  `,
  styles: `
    .placeholder__text {
      margin: 0;
      color: var(--p-text-muted-color);
    }
  `,
})
export class PagePlaceholder {
  readonly title = input.required<string>();
  readonly description = input('This area is ready for features to be added.');
}
