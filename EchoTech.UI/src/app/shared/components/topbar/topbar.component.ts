import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { LayoutService } from '../../../core/services/layout.service';
import { NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ThemeService } from '../../../core/services/theme.service';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [NgClass,RouterLink],
  templateUrl: './topbar.component.html'
})
export class TopbarComponent {
  layoutService  = inject(LayoutService)
  private themeService = inject(ThemeService)
  isLightTheme = true

  @ViewChild('menubutton') menuButton!: ElementRef;

  @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

  
  @ViewChild('topbarmenu') menu!: ElementRef;


  onThemetoggle(){
    this.isLightTheme = !this.isLightTheme
    this.themeService.setTheme(this.isLightTheme)
  }

}
