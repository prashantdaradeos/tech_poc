import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateService, TranslateLoader } from '@ngx-translate/core';
import { Subject, of } from 'rxjs';

import fallbackLangData from '../../../assets/locale/en.json';

@Injectable()
export class AppTranslationService {
  private translate = inject(TranslateService);

  private onLanguageChanged = new Subject<string>();
  languageChanged$ = this.onLanguageChanged.asObservable();

  constructor() {
    this.addLanguages(['en', 'fr', 'de', 'pt', 'ar', 'ko', 'hi']);
    this.setDefaultLanguage('en');
  }

  addLanguages(lang: string[]) {
    this.translate.addLangs(lang);
  }

  setDefaultLanguage(lang: string) {
    this.translate.setDefaultLang(lang);
  }

  getDefaultLanguage() {
    return this.translate.defaultLang;
  }

  getBrowserLanguage() {
    return this.translate.getBrowserLang();
  }

  getCurrentLanguage() {
    return this.translate.currentLang;
  }

  getLoadedLanguages() {
    return this.translate.langs;
  }

  useBrowserLanguage(): string | void {
    const browserLang = this.getBrowserLanguage();

    if (browserLang?.match(/en|fr|de|pt|ar|ko|hi/)) {
      this.changeLanguage(browserLang);
      return browserLang;
    }
  }

  useDefaultLanguage() {
    return this.changeLanguage(null);
  }

  changeLanguage(language: string | null) {
    if (!language) {
      language = this.getDefaultLanguage();
    }

    if (language !== this.translate.currentLang) {
      const lang = language;

      setTimeout(() => {
        this.translate.use(lang);
        this.onLanguageChanged.next(lang);
      });
    }

    return language;
  }

  getTranslation(key: string | string[], interpolateParams?: object) {
    return this.translate.instant(key, interpolateParams);
  }

  getTranslationAsync(key: string | string[], interpolateParams?: object) {
    return this.translate.get(key, interpolateParams);
  }
}


export class TranslateLanguageLoader implements TranslateLoader {
  http = inject(HttpClient);

  public getTranslation(lang: string) {
    if (lang === 'en')
      return of(fallbackLangData);

    return this.http.get(`assets/locale/${lang}.json`);
  }
}
