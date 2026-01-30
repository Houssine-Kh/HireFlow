import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../models/api-response.model'; // Import your interface
import { ProblemDetails } from '../models/problem-details';

export interface ParsedError {
  message: string;
  validationErrors: Record<string, string[]>;
}

export function parseHttpError(err: HttpErrorResponse): ParsedError {
  // Default Result
  const result: ParsedError = {
    message: 'An unexpected error occurred.',
    validationErrors: {}
  };

  // 1. Network / Connection Errors (Server down, No Internet, CORS block)
  if (err.status === 0) {
     result.message = 'Network error. Please check your internet connection or server status.';
     return result;
  }

  // 2. Try to parse the body as ProblemDetails
    const problem = err.error as ProblemDetails;

  // 3. Scenario A: Validation Errors (400)
  // .NET returns an 'errors' dictionary in ValidationProblemDetails
  if (problem && problem.errors) {
    result.validationErrors = problem.errors;
    // Optional: Return the first actual error message for a Toast/Snackbar
    const firstField = Object.keys(problem.errors)[0];
    result.message = firstField 
      ? `${firstField}: ${problem.errors[firstField][0]}`       // Example: "Email: The email format is invalid."
      : (problem.title || 'Validation failed.');
      
    return result;   
  }

  // 4. Scenario B: Domain Exceptions & Standard Errors
  // The middleware puts the specific error message in 'detail'
  if (problem && problem.detail) {
    result.message = problem.detail;
    return result
  }
  
  // 5. Scenario C: Standard HTTP Fallbacks (e.g. IIS / Kestrel errors outside your app)
  switch (err.status) {
      case 400: result.message = 'Bad Request.'; break;
      case 401: result.message = 'Unauthorized. Please login again.'; break;
      case 403: result.message = 'Forbidden.'; break;
      case 404: result.message = 'Resource not found.'; break;
      case 500: result.message = 'Internal Server Error.'; break;
      default:  result.message = err.message || result.message;
    }
    return result;
  }

  export function normalizeValidationErrors(errors: Record<string, string[]>): Record<string, string[]> {
  if (!errors) return {};
  
  return Object.fromEntries(
    Object.entries(errors).map(([key, value]) => [key.toLowerCase(), value])
  );
}
