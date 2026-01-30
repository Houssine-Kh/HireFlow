
import { inject } from "@angular/core";
import { patchState, signalStore, withMethods, withState } from "@ngrx/signals";
import { CandidateService } from "./candidate.service";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { CreateProfileCommand } from "../../domain/dtos/candidate.dto";
import { pipe, switchMap, tap } from "rxjs";
import { tapResponse } from "@ngrx/operators";
import { HttpErrorResponse } from "@angular/common/http";
import { parseHttpError } from "../../core/utils/http-error.utils";


interface CandidateState {
    isLoading : boolean;
    error : string | null;
}

const initialState : CandidateState = {
    isLoading : false,
    error : null
}

export const CandidateStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),

    withMethods((store, candidateService =  inject(CandidateService)) => ({
        createProfile : rxMethod<CreateProfileCommand>(
            pipe(
                tap(() => patchState(store, {isLoading :true, error : null })),
                switchMap((command) => 
                    candidateService.createProfile(command).pipe(
                        tapResponse({
                            next : (response) => {
                                patchState(store, {isLoading : false})
                            },
                            error : (err : HttpErrorResponse) => {
                                const parsed = parseHttpError(err)
                                patchState(store, {
                                    isLoading : false,
                                    error : parsed.message
                                })
                            }
                        })
                    )

                )
            )
        ),
    }))
);