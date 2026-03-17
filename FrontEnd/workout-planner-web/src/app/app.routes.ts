import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'login',
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login.page').then((m) => m.LoginPage),
  },
  {
    path: 'create-user',
    loadComponent: () =>
      import('./components/create-user/create-user.page').then((m) => m.CreateUserPage),
  },
  {
    path: 'app/:userId',
    loadComponent: () =>
      import('./components/app-shell/app-shell.page').then((m) => m.AppShellPage),
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
