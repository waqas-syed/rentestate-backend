using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.HotelAggregate
{
    /// <summary>
    /// Specifies the amount of beds for the specified bed type
    /// </summary>
    public class Bed
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bedCount"></param>
        /// <param name="bedType"></param>
        public Bed(int bedCount, BedType bedType)
        {
            BedCount = bedCount;
            BedType = bedType;
        }

        /// <summary>
        /// Bed COunt
        /// </summary>
        public int BedCount { get; set; }

        /// <summary>
        /// BedType
        /// </summary>
        public BedType BedType { get; set; }
    }
}
