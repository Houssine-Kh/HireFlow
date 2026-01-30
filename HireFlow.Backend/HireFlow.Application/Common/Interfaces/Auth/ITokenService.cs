using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Users.Entities;

namespace HireFlow.Application.Common.Interfaces.Auth
{
    public interface ITokenService
    {
        public string CreateToken(Guid userId, string email, string role);
    }
}