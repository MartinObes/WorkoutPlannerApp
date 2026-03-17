import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  ChartColumn,
  DoorOpen,
  Dumbbell,
  LayoutDashboard,
  LucideAngularModule,
  Settings,
} from 'lucide-angular';

@Component({
  selector: 'app-sideNav',
  standalone: true,
  imports: [LucideAngularModule, RouterLink],
  templateUrl: './sideNav.html',
})
export class SideNavComponent {
  readonly userId = input<string>('');
  readonly username = input<string>('User');
  readonly dashboardIcon = LayoutDashboard;
  readonly workoutsIcon = Dumbbell;
  readonly evaluationsIcon = ChartColumn;
  readonly settingsIcon = Settings;
  readonly logoutIcon = DoorOpen;
}
