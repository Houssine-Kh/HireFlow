using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HireFlow.Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public required string FirstName {get;set;}
        public required string LastName {get;set;}

    }
}