import { environment } from "../../../environments/environment";

export const API_CONFIG = {
  // This pulls 'https://localhost:7266/api' from environment file
  baseUrl: environment.apiUrl, 
  
  endpoints: {
    auth: {
      // Result: https://localhost:7266/api/auth/login
      login: '/auth/login',

      // Result: https://localhost:7266/api/auth/register
      register: '/auth/register'
    },
 admin: {
      getUsers: '/admin/users',                                      // GET /api/admin/users
      approveRecruiter: (id: string) => `/admin/recruiters/${id}/approve`, // POST /api/admin/recruiters/{id}/approve
      rejectRecruiter: (id: string) => `/admin/recruiters/${id}/reject`,   // POST /api/admin/recruiters/{id}/reject
      banUser: (id: string) => `/admin/users/${id}/ban`,             // POST /api/admin/users/{id}/ban
      unlockUser: (id: string) => `/admin/users/${id}/unlock`        // POST /api/admin/users/{id}/unlock
    },
    // Future placeholders based on your HireFlow domain
    users: {
      getAll: '/users',
      getById: (id: string) => `/users/${id}`,
      update: (id: string) => `/users/${id}`,
      delete: (id: string) => `/users/${id}`
    },
    candidates: {
      create: '/candidates' 
    },
    jobs: {
        getAll: '/jobs',
        getById: (id: string) => `/jobs/${id}`,
        apply: (id: string) => `/jobs/${id}/apply`
    }
  }
};