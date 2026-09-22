import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DrawerModule } from 'primeng/drawer';
import { ProgressBarModule } from 'primeng/progressbar';

import { LoadingService } from '@core';

/**
 * Application shell: top bar, module navigation and the routed outlet. One entry
 * per business module, mirroring the backend's module boundaries.
 */
@Component({
  selector: 'app-main-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    ButtonModule,
    DrawerModule,
    ProgressBarModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout {
  private readonly loading = inject(LoadingService);

  readonly isLoading = this.loading.isLoading;
  readonly menuOpen = signal(false);

  readonly modules: MenuItem[] = [
    { label: 'Masters', icon: 'pi pi-book', routerLink: '/masters' },
    { label: 'Inventory', icon: 'pi pi-box', routerLink: '/inventory' },
    { label: 'Accounts', icon: 'pi pi-wallet', routerLink: '/accounts' },
    { label: 'Admin', icon: 'pi pi-cog', routerLink: '/admin' },
  ];

  toggleMenu(): void {
    this.menuOpen.update((open) => !open);
  }

  closeMenu(): void {
    this.menuOpen.set(false);
  }
}
