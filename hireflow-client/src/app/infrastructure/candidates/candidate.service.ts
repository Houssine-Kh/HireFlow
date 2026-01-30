import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../../core/config/api.config';
import { CreateProfileCommand } from '../../domain/dtos/candidate.dto';


@Injectable({
    providedIn: 'root'
})
export class CandidateService {
    private readonly http = inject(HttpClient);

    createProfile(profile: CreateProfileCommand): Observable<any> {
        const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.candidates.create}`;
        return this.http.post<any>(url, profile);
    }
}
