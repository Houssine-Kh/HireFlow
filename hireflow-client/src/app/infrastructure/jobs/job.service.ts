import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { API_CONFIG } from "../../core/config/api.config";
import { Observable } from "rxjs";
import { ApiResponse } from "../../core/models/api-response.model";
import { CreateJobRequest, JobDto, updateJobRequest } from "../../domain/dtos/job.dto";

@Injectable({
    providedIn: 'root'
})

export class JobService{
    private http = inject(HttpClient);
    private readonly baseUrl = API_CONFIG.baseUrl;  
    
    getAllJobs(): Observable<ApiResponse<JobDto[]>> {
    return this.http.get<ApiResponse<JobDto[]>>(`${this.baseUrl}${API_CONFIG.endpoints.jobs.getAll}`);
  }
  createJob(request: CreateJobRequest): Observable<ApiResponse<JobDto>> { 
    return this.http.post<ApiResponse<JobDto>>(`${this.baseUrl}${API_CONFIG.endpoints.jobs.create}`, request);
  }

  updateJob(id : string, request : updateJobRequest) : Observable<ApiResponse<JobDto>>{
    return this.http.put<ApiResponse<JobDto>>(`${this.baseUrl}${API_CONFIG.endpoints.jobs.update(id)}`, request);
  }

  publishJob(id: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.baseUrl}${API_CONFIG.endpoints.jobs.publish(id)}`, {});
  }

  closeJob(id: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.baseUrl}${API_CONFIG.endpoints.jobs.close(id)}`, {});
  }

}