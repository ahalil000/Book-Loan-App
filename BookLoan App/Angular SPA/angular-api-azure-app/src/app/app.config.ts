import { InjectionToken } from '@angular/core';
import { environment } from '../environments/environment';

export const APP_CONFIG = new InjectionToken<AppConfig>('app.config');

export class AppConfig {
    baseApiUrl: string;
    baseLoanApiUrl: string;
    baseIdentityApiUrl: string;
    maxRowsPerPage: string;
    title: string;
}

export const BOOKS_DI_CONFIG: AppConfig = {
    baseApiUrl: environment.baseApiUrl,
    baseLoanApiUrl: environment.baseLoanApiUrl,
    baseIdentityApiUrl: environment.baseIdentityApiUrl,
    maxRowsPerPage: "20",
    title: "Angular Demo App"
};
