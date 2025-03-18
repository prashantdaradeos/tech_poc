import { inject, Injectable } from "@angular/core";
import { AppTranslationService } from "./app-translation.service";
import { LocalStoreManager } from "./local-store-manager.service";
import { DBkeys } from "../utils/db-keys";

@Injectable()
export class ConfigurationService {
    public static readonly defaultLanguage = 'en';
    public static readonly defaultThemeId = false;

    private _language: string | null = null;
    private _themeId: boolean | null = null;

    private localStorage = inject(LocalStoreManager);
    private translationService = inject(AppTranslationService);

    constructor() {
        this.loadLocalChanges();
    }

    set language(value: string | null) {
        this._language = value;
        this.saveToLocalStore(value, DBkeys.LANGUAGE);
        this.translationService.changeLanguage(value);
    }

    get language(): string {
        return this._language ?? ConfigurationService.defaultLanguage;
    }

    private saveToLocalStore(data: unknown, key: string) {
        setTimeout(() => this.localStorage.savePermanentData(data, key));
    }

    public saveConfiguration(data: unknown, configKey: string) {
        this.addKeyToUserConfigKeys(configKey);
        this.localStorage.savePermanentData(data, configKey);
    }

    public getConfiguration<T>(configKey: string, isDateType = false) {
        return this.localStorage.getDataObject<T>(configKey, isDateType);
    }


    private addKeyToUserConfigKeys(configKey: string) {
        const configKeys = this.localStorage.getDataObject<string[]>(DBkeys.USER_CONFIG_KEYS) ?? [];

        if (!configKeys.includes(configKey)) {
            configKeys.push(configKey);
            this.localStorage.savePermanentData(configKeys, DBkeys.USER_CONFIG_KEYS);
        }
    }

    private loadLocalChanges() {
        if (this.localStorage.exists(DBkeys.LANGUAGE)) {
            this._language = this.localStorage.getDataObject<string>(DBkeys.LANGUAGE);
            this.translationService.changeLanguage(this._language);
        } else {
            this.resetLanguage();
        }
    }

    private resetLanguage() {
        const language = this.translationService.useBrowserLanguage();

        if (language) {
            this._language = language;
        } else {
            this._language = this.translationService.useDefaultLanguage();
        }
    }

    set themeId(value: boolean) {
        this._themeId = value;
        this.saveToLocalStore(value, DBkeys.THEME_ID);
    }

    get themeId() {
        return this._themeId ?? ConfigurationService.defaultThemeId;
    }

}