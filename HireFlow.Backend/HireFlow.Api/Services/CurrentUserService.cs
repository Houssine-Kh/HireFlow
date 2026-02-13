using System.Security.Claims;
using HireFlow.Application.Common.Interfaces;
using HireFlow.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace HireFlow.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                // Check if a User Context exists
                var user = _httpContextAccessor.HttpContext?.User;
                
                if (user == null) 
                    return null;

                // Extract the NameIdentifier Claim (Standard for User ID)
                // This claim is populated automatically by JWT Middleware
                var idClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(idClaim)) 
                    return null;

                // Return as Guid
                return Guid.TryParse(idClaim, out var userId) ? userId : null;
            }
        }
    }
}