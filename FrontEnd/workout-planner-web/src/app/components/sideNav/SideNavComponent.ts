import { Component } from '@angular/core';
import { Router } from '@angular/router';
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
  imports: [LucideAngularModule],
  templateUrl: './sideNav.html',
})
export class SideNavComponent {
  readonly dashboardIcon = LayoutDashboard;
  readonly workoutsIcon = Dumbbell;
  readonly evaluationsIcon = ChartColumn;
  readonly settingsIcon = Settings;
  readonly logoutIcon = DoorOpen;

  constructor(private router: Router) {}
}
