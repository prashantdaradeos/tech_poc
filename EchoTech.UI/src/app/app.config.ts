import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import { provideHttpClient, withFetch } from '@angular/common/http';
import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { AppTranslationService, TranslateLanguageLoader } from './core/services/app-translation.service';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { ConfigurationService } from './core/services/configuration.service';
import { LocalStoreManager } from './core/services/local-store-manager.service';

export const appConfig: ApplicationConfig = {
  providers:
    [
      provideZoneChangeDetection({ eventCoalescing: true }),
      provideRouter(routes, withComponentInputBinding()),
      provideClientHydration(),
      provideAnimationsAsync(),
      provideHttpClient(withFetch()),
      importProvidersFrom(
        TranslateModule.forRoot({
          loader: { provide: TranslateLoader, useClass: TranslateLanguageLoader }
        })
      ),
      { provide: LocationStrategy, useClass: PathLocationStrategy },
      AppTranslationService,
      ConfigurationService,
      LocalStoreManager
    ]
};
