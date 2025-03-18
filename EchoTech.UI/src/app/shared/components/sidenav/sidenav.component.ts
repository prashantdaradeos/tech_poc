import { Component, ElementRef } from '@angular/core';
import { AppmenuComponent } from './appmenu/appmenu.component';
import { LayoutService } from '../../../core/services/layout.service';

@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [AppmenuComponent],
  templateUrl: './sidenav.component.html'
})
export class SidenavComponent {
  constructor(public layoutService: LayoutService, public el: ElementRef) { }
}
