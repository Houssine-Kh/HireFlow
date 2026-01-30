import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/aura';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { HireFlowPreset } from './core/Theme/hireflow.preset';


export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
        provideHttpClient(
      withInterceptors([authInterceptor])
    ),
    providePrimeNG({
    theme: {
        preset: HireFlowPreset,
        options: {
            // 1. Force Light Mode (Disables OS Dark Mode detection)
            darkModeSelector: false,
            cssLayer: {
                name: 'primeng', // Matches the @layer name in styles.scss
                order: 'tailwind-base, primeng, tailwind-utilities'
            }
        }
    }
})
  ]
};
