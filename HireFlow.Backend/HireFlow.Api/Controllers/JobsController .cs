using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Api.Contracts.Jobs;
using HireFlow.Application.Candidates.Commands.CreateProfile;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Jobs.Commands.CloseJob;
using HireFlow.Application.Jobs.Commands.CreateJob;
using HireFlow.Application.Jobs.Commands.PublishJob;
using HireFlow.Application.Jobs.Commands.UpdateJob;
using HireFlow.Application.Jobs.Common;
using HireFlow.Application.Jobs.Queries;
using HireFlow.Application.Users.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var result = await _mediator.Send(new GetAllJobsQuery());

            if (!result.IsSuccess)
            {
                return Problem(
                    title : "Failed to get all Jobs",
                    statusCode : StatusCodes.Status400BadRequest,
                    detail : result.Error
                );
            }
            return Ok(ApiResponse<List<JobDto>>.SuccessResponse(result.Value!));
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request )
        {
            var command = new CreateJobCommand(
                request.Title,
                request.Description,
                request.WorkMode
            );

            var result  = await _mediator.Send(command);

            if(!result.IsSuccess)
                return Problem(
                    statusCode : StatusCodes.Status400BadRequest,
                    title : "Failed to create Job.",
                    detail : result.Error
                );
            
            return Ok(ApiResponse<Guid>.SuccessResponse(result.Value,"Job created Succesfully."));
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> PublishJob(Guid id )
        {
            var result = await _mediator.Send(new PublishJobCommand(id));

            if(!result.IsSuccess)
            return Problem(
                title : "Failed to publish Job.",
                statusCode : StatusCodes.Status400BadRequest,
                detail : result.Error
            );

            return Ok(ApiResponse.SuccessResponse("Job published successfully."));
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult> CloseJob(Guid id)
        {
            var result = await _mediator.Send(new CloseJobCommand(id));

            if(!result.IsSuccess)
            return Problem(
                title : "Failed to close Job.",
                statusCode : StatusCodes.Status400BadRequest,
                detail : result.Error
            );
            return Ok(ApiResponse.SuccessResponse("Job closed successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob( Guid id, [FromBody] UpdateJobRequest request)
        {
            var command = new UpdateJobCommand(
                id,
                request.Title,
                request.Description,
                request.WorkMode
            );

            var result = await _mediator.Send(command);
            if(!result.IsSuccess)
                return Problem(
                    title : "Failed to update Job.",
                    statusCode : StatusCodes.Status400BadRequest,
                    detail : result.Error
                );
            return Ok(ApiResponse<JobDto>.SuccessResponse(result.Value!,"Job updated sucessfully."));
        }
    }
}