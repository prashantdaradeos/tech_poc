import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { SelectItem } from 'primeng/api';
import { DropdownChangeEvent, DropdownModule } from "primeng/dropdown";
import { ConfigurationService } from '../../core/services/configuration.service';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { ThemeService } from '../../core/services/theme.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [TranslateModule, DropdownModule, FormsModule,ToggleButtonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {

  private themeService = inject(ThemeService)
  configurations = inject(ConfigurationService);
  isLightTheme = true
  
  languages: SelectItem[] = [
    { label: 'English', value: 'en' },
    { label: 'Hindi', value: 'hi' }
  ];
  selectedLangauge = this.configurations.language // Default to the first language

  checked : boolean = false;

  onLanguageClick(language: DropdownChangeEvent) {
    this.configurations.language = language.value
  }

  onThemetoggle(){
    this.isLightTheme = !this.isLightTheme
    this.themeService.setTheme(this.isLightTheme)
  }

}
