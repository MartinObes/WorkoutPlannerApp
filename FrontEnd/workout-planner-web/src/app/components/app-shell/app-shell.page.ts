import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SideNavComponent } from '../sideNav/SideNavComponent';
import { SessionService } from '../../services/session.service';

@Component({
  selector: 'app-app-shell-page',
  standalone: true,
  imports: [SideNavComponent, RouterOutlet],
  templateUrl: './app-shell.page.html',
})
export class AppShellPage implements OnInit, OnDestroy {
  constructor(private readonly sessionService: SessionService) {}
  username = 'User';
  private readonly destroy$ = new Subject<void>();

  ngOnInit(): void {
    this.sessionService.sessionChanges$.pipe(takeUntil(this.destroy$)).subscribe((session) => {
      this.username = session?.userName ?? 'User';
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
