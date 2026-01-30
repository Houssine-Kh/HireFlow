using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Exceptions;
using HireFlow.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;


namespace HireFlow.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (ApplicationValidationException ex)
            {
                var problem = new ValidationProblemDetails(ex.Errors)
                {
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(problem);
            }
            catch (DomainException ex)
            {
                await WriteProblemAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    "Domain rule violated",
                    ex.Message);
            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                await WriteProblemAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "An unexpected error occurred.");
            }
        }
        private static async Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
        {
            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Type = $"https://httpstatuses.com/{status}",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
