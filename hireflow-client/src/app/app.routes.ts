import { Routes } from '@angular/router';
import { AdminLayout } from './features/admin/layout/admin-layout/admin-layout';
import { UserManagement } from './features/admin/user-management/user-management';

export const routes: Routes = [
// 1. The default route (Fixes the blank page)
  { 
    path: '', 
    redirectTo: 'auth/login', 
    pathMatch: 'full' 
  },
  
  // 2. Your existing Login route
  // Make sure the path string matches your actual file name!
  // If your file is "login.component.ts", the import should usually be:
  // import('./features/auth/login/login.component')
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
    component: AdminLayout, // 1. Loads the Sidebar Shell first
    // canActivate: [AdminGuard], // (Add this later to protect the route)
    children: [
      {
        path: 'users',
        component: UserManagement // 2. Loads the table inside the shell
      },
      {
        path: '',
        redirectTo: 'users',
        pathMatch: 'full'
      }
    ]
  }

];
