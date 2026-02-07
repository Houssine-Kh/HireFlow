import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { API_CONFIG } from "../../core/config/api.config";
import { UserDto } from "../../domain/dtos/admin.dto";
import { Observable } from "rxjs";


@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private readonly http = inject(HttpClient);

  // GET /api/admin/users
  getUsers(): Observable<UserDto[]> {
    const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.admin.getUsers}`;
    return this.http.get<UserDto[]>(url);
  }

  // POST /api/admin/recruiters/{id}/approve
  approveRecruiter(id: string): Observable<void> {
    const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.admin.approveRecruiter(id)}`;
    return this.http.post<void>(url, {});
  }

  rejectRecruiter(id: string): Observable<void> {
    const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.admin.rejectRecruiter(id)}`;
    return this.http.post<void>(url, {});
  }

  banUser(id: string): Observable<void> {
    const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.admin.banUser(id)}`;
    return this.http.post<void>(url, {});
  }

  unlockUser(id: string): Observable<void> {
    const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.admin.unlockUser(id)}`;
    return this.http.post<void>(url, {});
  }
}