import { patchState, signalStore, withMethods, withState } from "@ngrx/signals";
import { UserDto } from "../../domain/dtos/admin.dto";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { AdminService } from "./admin.service";
import { inject } from "@angular/core";
import { pipe, switchMap, tap } from "rxjs";
import { tapResponse } from "@ngrx/operators";
import { HttpErrorResponse } from "@angular/common/http";
import { normalizeValidationErrors, parseHttpError } from "../../core/utils/http-error.utils";

interface AdminState {
  users: UserDto[];
  isLoading: boolean;
  error: string | null;
  validationErrors: Record<string, string[]>;
}

const initialState : AdminState = {
    users : [],
    isLoading : false,
    error : null,
    validationErrors: {}
}

export const AdminStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),

    withMethods((store, adminService = inject(AdminService)) => ({
        loadUsers : rxMethod<void>(
            pipe(
                tap(() => 
                    patchState(store,{ isLoading: true, error: null, validationErrors: {} })
                ),
            switchMap(() => 
                adminService.getUsers().pipe(
                    tapResponse({
                        next : (users) => patchState(store,{users, isLoading: false}),
                        error: (err: HttpErrorResponse) => {
                            const parsed = parseHttpError(err);
                            patchState(store, { error: parsed.message, isLoading: false });
                          }
                        })
                     ) 
                   )
                )
            ),

        // APPROVE RECRUITER
        approveRecruiter: rxMethod<string>(
        pipe(
            tap(() => patchState(store, { isLoading: true, error: null })),

            switchMap((id) =>
            adminService.approveRecruiter(id).pipe(
                tapResponse({
                next: () => {
                   // store.loadUsers(); 
                   patchState(store, (state) => ({
                    isLoading : false,
                    users : state.users.map(u =>
                        u.id == id ? {...u, status : "Active"} : u
                    )
                }));
                },
                error: (err: HttpErrorResponse) => patchState(store, { error: parseHttpError(err).message, isLoading: false })
                })
              )
            )
          )
        ),

        // REJECT RECRUITER
        rejectRecruiter: rxMethod<string>(
        pipe(
            tap(() => patchState(store, { isLoading: true, error: null })),

            switchMap((id) =>
            adminService.rejectRecruiter(id).pipe(
                tapResponse({
                next: () => {
                   // store.loadUsers();
                   patchState(store, (state) => ({
                    isLoading : false,
                    users : state.users.filter(u => u.id != id)
                   }));
                },
                error: (err: HttpErrorResponse) => patchState(store, { error: parseHttpError(err).message, isLoading: false })
                })
              )
            )
          )
        ),

        // BAN USER
        banUser: rxMethod<string>(
        pipe(
            tap(() => patchState(store, { isLoading: true, error: null })),

            switchMap((id) =>
            adminService.banUser(id).pipe(
                tapResponse({
                next: () => {
                    //store.loadUsers();
                    patchState(store, (state) => ({
                        isLoading : false,
                        users : state.users.map(u => 
                            u.id == id ? {...u,status : "Banned"} : u
                        )
                    }));
                },
                error: (err: HttpErrorResponse) => patchState(store, { error: parseHttpError(err).message, isLoading: false })
                })
              )
            )
          )
        ),

        // UNLOCK USER
        unlockUser: rxMethod<string>(
        pipe(
            tap(() => patchState(store, { isLoading: true, error: null })),

            switchMap((id) =>
            adminService.unlockUser(id).pipe(
                tapResponse({
                next: () => {
                    //store.loadUsers();
                    patchState(store, (state) => ({
                        isLoading : false,
                        users : state.users.map(u =>
                            u.id == id ? {...u, status : "Active"} : u
                        )
                    }))
                },
                error: (err: HttpErrorResponse) => patchState(store, { error: parseHttpError(err).message, isLoading: false })
              })
            )
           )
          )
        )
            
    }))
);