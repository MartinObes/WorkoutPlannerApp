import { Component, signal } from '@angular/core';
import { SideNavComponent } from './components/sideNav/SideNavComponent';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [SideNavComponent],
  template: `<app-sideNav></app-sideNav>`,
  styles: [],
})
export class App {
  protected readonly title = signal('workout-planner-web');
}
