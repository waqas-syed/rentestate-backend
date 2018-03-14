using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentStuff.Common.Domain.Model;

namespace RentStuff.Property.Domain.Model.HotelAggregate
{
    /// <summary>
    /// Specifies the amount of beds for the specified bed type
    /// </summary>
    public class Bed : Entity
    {
        public Bed()
        {
            
        }

        public Bed(int bedCount, BedType bedType)
        {
            BedCount = bedCount;
            BedType = bedType;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bedCount"></param>
        /// <param name="bedType"></param>
        public Bed(int bedCount, BedType bedType, Hotel hotel)
        {
            BedCount = bedCount;
            BedType = bedType;
            Hotel = hotel;
        }

        /// <summary>
        /// Bed COunt
        /// </summary>
        public int BedCount { get; set; }

        /// <summary>
        /// BedType
        /// </summary>
        public BedType BedType { get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
