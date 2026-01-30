using System.Security.Claims;
using HireFlow.Api.Contracts.Candidates;
using HireFlow.Application.Candidates.Commands.CreateProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // This works now because you fixed DependencyInjection.cs
    public class CandidatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandidatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest request)
        {
            // 1. Extract User ID from the JWT Token (ClaimTypes.NameIdentifier)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Problem(
                    detail: "User is not authenticated.",
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized"
                );
            }

            var userId = Guid.Parse(userIdClaim);

            // 2. Map Request DTO to CQRS Command
            var command = new CreateProfileCommand(
                userId,
                request.ResumeUrl,
                request.PhoneNumber,
                request.LinkedInUrl,
                request.EducationLevel
            );

            // 3. Send to Handler
            var result = await _mediator.Send(command);

            // 4. Handle Result
            if (!result.IsSuccess)
            {
                return Problem(
                    detail: result.Error,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Profile Creation Failed"
                );
            }

            return Ok(result.Value);
        }
    }
}