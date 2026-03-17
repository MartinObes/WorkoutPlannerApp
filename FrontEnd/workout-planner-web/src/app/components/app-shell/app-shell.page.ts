import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SideNavComponent } from '../sideNav/SideNavComponent';

@Component({
  selector: 'app-app-shell-page',
  standalone: true,
  imports: [SideNavComponent],
  templateUrl: './app-shell.page.html',
})
export class AppShellPage {
  private readonly route = inject(ActivatedRoute);
  readonly userId = this.route.snapshot.paramMap.get('userId') ?? '';
  readonly username = localStorage.getItem('currentUserName') ?? 'User';
}
