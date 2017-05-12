using System;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.HouseServices.Representation
{
    /// <summary>
    /// Represents the count of houses and the page size; used for pagination by the frontend
    /// </summary>
    [Serializable]
    [DataContract]
    public class HouseCountRepresentation
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public HouseCountRepresentation(int recordCount, int pageSize)
        {
            RecordCount = recordCount;
            PageSize = pageSize;
        }

        /// <summary>
        /// Number of records
        /// </summary>
        [DataMember]
        public int RecordCount { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
    }
}
