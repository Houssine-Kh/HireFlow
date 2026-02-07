import { patchState, signalStore, withMethods, withState } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { pipe, switchMap, tap } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../infrastructure/auth/auth.service';
import { LoginCommand, RegisterCommand, AuthResponseDto } from '../../domain/dtos/auth.dto';
import { normalizeValidationErrors, parseHttpError } from '../../core/utils/http-error.utils';
import { Router } from '@angular/router';

// State includes user data AND validation errors for the "Red Border" effect
interface AuthState {
  user: AuthResponseDto | null;
  isLoading: boolean;
  error: string | null;
  validationErrors: Record<string, string[]>;
}

const initialState: AuthState = {
  user: null,
  isLoading: false,
  error: null,
  validationErrors: {}
};

export const AuthStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  
  withMethods((store, authService = inject(AuthService) , router = inject(Router)) => ({
    
    // --- LOGIN ACTION ---
    login: rxMethod<LoginCommand>(
      pipe(
        tap(() => patchState(store, { isLoading: true, error: null, validationErrors: {} })),
        switchMap((command) =>
          authService.login(command).pipe(
            tapResponse({
              next: (response) => {
                // 1. Check the 'success' boolean from your ApiResponse
                  patchState(store, { 
                    user: response, 
                    isLoading: false 
                  });

                  if(response.token){
                  localStorage.setItem('token', response.token);
                  }

              switch (response.role) {
                  
                  case 'Admin':
                    router.navigate(['/admin/users']); 
                    break;

                  case 'Candidate':
                    if (!response.IsProfileComplete) {
                      router.navigate(['/candidate/profile-wizard']);
                    } else {
                     // router.navigate(['/candidate/dashboard']); 
                    }
                    break;

                  case 'Recruiter':
                    //router.navigate(['/recruiter/dashboard']); 
                    break;

                  default:
                    router.navigate(['/']);
                    break;
                }

              },
              error: (err: HttpErrorResponse) => {
                const parsed = parseHttpError(err);

                patchState(store, { 
                  error: parsed.message, 
                  validationErrors: normalizeValidationErrors(parsed.validationErrors),
                  isLoading: false 
                });
              },
            })
          )
        )
      )
    ),

    // --- REGISTER ACTION ---
    register: rxMethod<RegisterCommand>(
      pipe(
        tap(() => patchState(store, { isLoading: true, error: null, validationErrors: {} })),
        switchMap((command) =>
          authService.register(command).pipe(
            tapResponse({
              next: (response) => {
                  patchState(store, { user: response, isLoading: false });
                  if(response.token){
                    localStorage.setItem('token', response.token);
                  }
              },
              error: (err: HttpErrorResponse) => {
                const parsed = parseHttpError(err);

                patchState(store, { 
                    error: parsed.message, 
                    validationErrors: normalizeValidationErrors(parsed.validationErrors),
                    isLoading: false 
                });
              },
            })
          )
        )
      )
    ),
    
    resetErrors: () => {
      patchState(store, { error: null, validationErrors: {} });
    },
    
    logout: () => {
      patchState(store, initialState);
      localStorage.removeItem('token');
    }
  }))
);