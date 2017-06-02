using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Services.Domain.Model.ServicesAggregate
{
    /// <summary>
    /// Is the service being provided by an individual or a business
    /// </summary>
    public enum ServiceEntityType
    {
        Individual,
        Organization
    }
}
