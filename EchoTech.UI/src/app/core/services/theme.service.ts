import { DOCUMENT } from '@angular/common';
import { inject, Inject, Injectable } from '@angular/core';
import { ConfigurationService } from './configuration.service';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  activeTheme: string = 'dark';
  private configurationService = inject(ConfigurationService);

  private readonly LINK_ID: string = 'app-theme';

  constructor(@Inject(DOCUMENT) private document: Document) { }

  setTheme(theme: boolean) {
    const themeLink = this.document.getElementById(this.LINK_ID) as HTMLLinkElement;
    if (themeLink) {
      themeLink.href = theme ? 'light.css' : 'dark.css';
      this.activeTheme = theme ? 'light' : 'dark';
    }

    this.configurationService.themeId = theme;

  }
}
