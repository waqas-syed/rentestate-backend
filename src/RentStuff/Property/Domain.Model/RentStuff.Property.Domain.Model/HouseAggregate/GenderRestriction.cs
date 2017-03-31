using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Enum to specify restrictions as to whether the house is for families or boys or girls only, or there is no restriction at all
    /// </summary>
    public enum GenderRestriction
    {
        NoRestriction,
        BoysOnly,
        GirlsOnly,
        FamiliesOnly
    }
}
