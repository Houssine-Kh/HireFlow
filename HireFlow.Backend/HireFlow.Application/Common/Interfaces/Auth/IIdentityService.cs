using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Users.Auth.Dtos;
using HireFlow.Domain.Users.Entities;

namespace HireFlow.Application.Common.Interfaces.Auth
{
    public interface IIdentityService
    {
        Task<bool> EmailExists(string email);
        Task<Guid> GetIdentityUserIdByEmail(string email);
        Task<Result<Guid>> CreateIdentityUser(string firstName, string lastName, string email, string Password, string role);
        Task<bool> CheckPassword( Guid UserId,string Password);
        Task<string> GetUserRole(Guid userId);
        Task<Result> DeleteIdentityUser(Guid userId);

    }
}