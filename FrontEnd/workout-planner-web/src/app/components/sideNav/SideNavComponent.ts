import { Component, input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import {
  ChartColumn,
  DoorOpen,
  Dumbbell,
  LayoutDashboard,
  LucideAngularModule,
  Settings,
} from 'lucide-angular';
import { SessionService } from '../../services/session.service';

@Component({
  selector: 'app-sideNav',
  standalone: true,
  imports: [LucideAngularModule, RouterModule],
  templateUrl: './sideNav.html',
})
export class SideNavComponent {
  readonly username = input<string>('User');
  readonly dashboardIcon = LayoutDashboard;
  readonly workoutsIcon = Dumbbell;
  readonly evaluationsIcon = ChartColumn;
  readonly settingsIcon = Settings;
  readonly logoutIcon = DoorOpen;

  constructor(
    private readonly sessionService: SessionService,
    private readonly router: Router,
  ) {}

  onLogout(): void {
    this.sessionService.clearSession();
    this.router.navigate(['/login']);
  }
}
