using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Users.Auth.Commands.Login;
using HireFlow.Application.Users.Auth.Commands.Register;
using HireFlow.Application.Users.Auth.Dtos;
using HireFlow.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HireFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return Problem(
                    detail : result.Error,
                    statusCode : StatusCodes.Status400BadRequest,
                    title : "Registration Failed"
                );
            }
            return Ok(result.Value);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            if(!result.IsSuccess)
                return Problem(
                    detail : result.Error,
                    statusCode : StatusCodes.Status400BadRequest,
                    title : "Login Failed"
                );

            return Ok(result.Value);
        }

    }
}