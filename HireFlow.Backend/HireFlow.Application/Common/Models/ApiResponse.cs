using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireFlow.Application.Common.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        // Add this property
        public IDictionary<string, string[]>? ValidationErrors { get; set; }    

        public static ApiResponse SuccessResponse(string? message = null)
            => new() { Success = true, Message = message };

        public static ApiResponse FailResponse(string? message, IDictionary<string, string[]>? validationErrors = null)
            => new() { Success = false, Message = message, ValidationErrors = validationErrors };

        
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
            => new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> FailResponse(string? message)
            => new() { Success = false, Message = message, Data = default };

        public static ApiResponse<T> Created(T data, string message = "Created successfully") 
        => new() { Success = true, Data = data, Message = message };
    }

}