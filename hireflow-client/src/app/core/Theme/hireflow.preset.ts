// src/app/core/theme/hireflow.preset.ts
import { definePreset } from '@primeng/themes';
import Aura from '@primeng/themes/aura';

export const HireFlowPreset = definePreset(Aura, {
    semantic: {
        primary: {
            50: '#eff6ff',
            100: '#dbeafe',
            200: '#bfdbfe',
            300: '#93c5fd',
            400: '#60a5fa',
            500: '#3b82f6', // Main Brand Color (Blue-500)
            600: '#2563eb',
            700: '#1d4ed8',
            800: '#1e40af',
            900: '#1e3a8a',
            950: '#172554'
        },
        colorScheme: {
            light: {
                surface: {
                    0: '#ffffff',
                    50: '#f8fafc',  // Slate-50 (Very cool light gray)
                    100: '#f1f5f9', // Slate-100
                    200: '#e2e8f0', // Slate-200 (Borders)
                    300: '#cbd5e1',
                    400: '#94a3b8',
                    500: '#64748b', // Slate-500 (Muted Text)
                    600: '#475569',
                    700: '#334155',
                    800: '#1e293b',
                    900: '#0f172a', // Slate-900 (Headings)
                    950: '#020617'
                }
            }
        }
    }
});