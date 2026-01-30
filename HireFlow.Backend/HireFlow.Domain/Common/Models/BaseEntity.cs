using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireFlow.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id{get; protected set;}
    }
}