using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Users.Admin.Commands.ApproveRecruiter;
using HireFlow.Application.Users.Admin.Commands.BanUser;
using HireFlow.Application.Users.Admin.Commands.RejectRecruiter;
using HireFlow.Application.Users.Admin.Commands.UnlockUser;
using HireFlow.Application.Users.Admin.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
 //   [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());

            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Failed to retrieve users",
                    detail: result.Error
                );
            }
            return Ok(result.Value);
        }

        [HttpPost("recruiters/{id}/approve")]
        public async Task<IActionResult> ApproveRecruiter(Guid id)
        {
            var result = await _mediator.Send(new ApproveRecruiterCommand(id));

            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Failed to approve users",
                    detail: result.Error
                );
            }
            return Ok(new { message = "Recruiter approved successfully." });

        }

        [HttpPost("recruiters/{id}/reject")]
        public async Task<IActionResult> RejectRecruiter(Guid id)
        {
            var result = await _mediator.Send(new RejectRecruiterCommand(id));

            if (!result.IsSuccess)
            {
                return Problem(
                               statusCode: StatusCodes.Status400BadRequest,
                               title: "Failed to approve users",
                               detail: result.Error
                           );
            }
            return Ok(new { message = "Recruiter rejected and removed."} );
        }

        [HttpPost("users/{id}/ban")]
        public async Task<IActionResult> BanUser(Guid id)
        {
            var result = await _mediator.Send(new BanUserCommand(id));

            if(!result.IsSuccess)
                return Problem(
                    statusCode : StatusCodes.Status400BadRequest,
                    title : "Failded to ban user",
                    detail : result.Error
                );

            return Ok(new { message = "User banned successfully." });
        }

        [HttpPost("users/{id}/unlock")]
        public async Task<IActionResult> UnlockUser(Guid id)
        {
            var result = await _mediator.Send(new UnlockUserCommand(id));

            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Failed to unlock user",
                    detail: result.Error
                );
            }
            return Ok(new { message = "User unlocked successfully." });
        }
    }
}