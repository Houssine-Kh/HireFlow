import { inject } from "@angular/core";
import { patchState, signalStore, withMethods, withState } from "@ngrx/signals";
import { CreateJobRequest, JobDto, updateJobRequest } from "../../domain/dtos/job.dto";
import { JobService } from "./job.service";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { pipe, switchMap, tap } from "rxjs";
import { tapResponse } from "@ngrx/operators";
import { HttpErrorResponse } from "@angular/common/http";
import { normalizeValidationErrors, parseHttpError } from "../../core/utils/http-error.utils";


interface JobState{
    jobs : JobDto[];
    isLoading : boolean;
    error: string | null;
    validationErrors: Record<string, string[]>;
    lastAction: 'create' | 'update' | 'publish' | 'close' | null;
} 

const initialState : JobState = {
    jobs : [],
    isLoading : false,
    error : null,
    validationErrors : {},
    lastAction : null
};

export const JobStore = signalStore(
    {providedIn : 'root'},

    withState(initialState),

    withMethods((store, jobService = inject(JobService)) => ({
        loadJobs: rxMethod<void>(
            pipe(
                tap(() => patchState(store, { isLoading: true, error: null, validationErrors: {}, lastAction: null })),
                switchMap(() =>
                jobService.getAllJobs().pipe(
                    tapResponse({
                    next: (response) => patchState(store, { jobs: response.data ?? [], isLoading: false }),
                    error: (err: HttpErrorResponse) => {

                        const parsedError = parseHttpError(err);
                        
                        patchState(store, {
                        isLoading: false,
                        error: parsedError.message,
                        lastAction: null
                    });
                    }
                })
                )
              )
            )    
        ),

        createJob : rxMethod<CreateJobRequest>(
            pipe(
                tap(() => patchState(store, {isLoading : true, error : null, validationErrors :{}, lastAction : null})),
                switchMap((request) => 
                    jobService.createJob(request).pipe(
                        tapResponse({
                            next : (response) => {
                                patchState(store, (state) => ({ 
                                isLoading : false,
                                jobs : [response.data!, ...state.jobs],
                                lastAction : 'create' as const
                            }));
                        }, 
                            error : (err : HttpErrorResponse) => {
                                const parsedError = parseHttpError(err);
                                patchState(store, {
                                    isLoading : false,
                                    error : parsedError.message,
                                    validationErrors : normalizeValidationErrors(parsedError.validationErrors),
                                    lastAction : null
                                })
                            }
                        })
                    )
                )
            )
        ),

        updateJob : rxMethod<{id : string; request : updateJobRequest}>(
            pipe(
                tap(() => patchState(store, {isLoading : true, error : null, validationErrors : {} , lastAction : null })),
                switchMap(({id, request}) => 
                    jobService.updateJob(id, request).pipe(
                    tapResponse({
                        next : (response) => {
                            patchState(store, (state) => ({
                                isLoading : false,
                                jobs : state.jobs.map(job => job.id == id ? response.data! : job), 
                                lastAction : "update" as const
                            }));
                        },
                        error : (err : HttpErrorResponse) => patchState(store,{
                            isLoading : false,
                            error : parseHttpError(err).message,
                            validationErrors : normalizeValidationErrors(parseHttpError(err).validationErrors),
                            lastAction : null
                        })
                    })
                  )
                )
            )
        ),

        publishJob : rxMethod<string>(
            pipe(
                tap(() => patchState(store, {isLoading : true, error : null , validationErrors: {}, lastAction: null})),
                switchMap((id) => 
                    jobService.publishJob(id).pipe(
                        tapResponse({
                            next : (response) => {
                                patchState(store, (state) => ({
                                    isLoading : false,
                                    jobs : state.jobs.map(job => job.id == id ? {... job, status : 'Published'} : job),
                                    lastAction : "publish" as const
                                }))
                            },
                            error : (err : HttpErrorResponse) => patchState(store, {
                                isLoading : false,
                                error : parseHttpError(err).message,
                                validationErrors : normalizeValidationErrors(parseHttpError(err).validationErrors),
                                lastAction : null
                            })
                        })
                    )
                )
            )
        ),

        closeJob : rxMethod<string>(
            pipe(
                tap(() => patchState(store, {isLoading : true, error: null, validationErrors: {}, lastAction: null})),
                switchMap((id) => 
                    jobService.closeJob(id).pipe(
                        tapResponse({
                            next : (response) => {
                                patchState(store, (state) => ({
                                    isLoading : false,
                                    jobs : state.jobs.map(job => job.id == id ? {... job, status : 'Closed'} : job),
                                    lastAction: "close" as const
                                }))
                            },
                            error : (err : HttpErrorResponse) => patchState(store, {
                                isLoading : false,
                                error : parseHttpError(err).message,
                                validationErrors : normalizeValidationErrors(parseHttpError(err).validationErrors),
                                lastAction: null
                            })
                        })
                    )
                )
            )
        )
    }))
);