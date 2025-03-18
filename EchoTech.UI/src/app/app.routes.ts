import { Routes } from '@angular/router';
import { LayoutComponent } from './shared/components/layout/layout.component';

export const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children:[
            {
                path:'',
                loadComponent : () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
            },
            {
                path:'manage-user',
                loadComponent : () => import('./features/manage-user/manage-user.component').then(m =>m.ManageUserComponent)
            },
            {
                path:'profile',
                loadComponent : () => import('./features/profile/profile.component').then(m =>m.ProfileComponent)
            }
        ]
    }
];
