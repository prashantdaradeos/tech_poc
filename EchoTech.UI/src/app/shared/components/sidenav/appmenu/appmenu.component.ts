import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { AppMenuitemComponent } from './app.menuitem.component';

interface MenuItem {
  label: string;
  icon?: string;
  routerLink?: string[];
  url?: string | string[];
  target?: string;
  badge?: string;
  routerLinkActiveOptions?: {
    paths?: string;
    queryParams?: string;
    matrixParams?: string;
    fragment?: string;
  };
  items?: MenuItem[];  // Nested items
  separator ? : boolean
}



@Component({
  selector: 'app-appmenu',
  standalone: true,
  imports: [NgFor,NgIf,AppMenuitemComponent],
  templateUrl: './appmenu.component.html'
})
export class AppmenuComponent {

  menuItems: MenuItem[] = [
    {
      label: 'Home',
      items: [
        { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] },
        { label: 'Manage Users', icon: 'pi pi-fw pi-id-card', routerLink: ['/manage-user'] },
      ]
    },
    {
      label: 'Settings',
      items: [
        { label: 'Profile', icon: 'pi pi-fw pi-cog', routerLink: ['/profile'] },
      ]
    },
  ]

}
