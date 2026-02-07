import { Routes } from '@angular/router';
import { AdminLayout } from './features/admin/layout/admin-layout/admin-layout';
import { UserManagement } from './features/admin/user-management/user-management';

export const routes: Routes = [
// The default route 
  { 
    path: '', 
    redirectTo: 'auth/login', 
    pathMatch: 'full' 
  },
  
  { 
    path: 'auth/login', 
    loadComponent: () => import('./features/auth/login/login').then(m => m.LoginComponent) 
  },
  { 
    path: 'auth/register', 
    loadComponent: () => import('./features/auth/register/register').then(m => m.RegisterComponent) 
  },
    { 
    path: 'candidate/profile-wizard', 
    loadComponent: () => import('./features/candidate/profile-wizard/profile-wizard').then(m => m.ProfileWizardComponent) 
  },
  {
    path: 'admin',
    component: AdminLayout, 
    // canActivate: [AdminGuard], 
    children: [
      {
        path: 'users',
        component: UserManagement 
      },
      {
        path: '',
        redirectTo: 'users',
        pathMatch: 'full'
      }
    ]
  }
];
