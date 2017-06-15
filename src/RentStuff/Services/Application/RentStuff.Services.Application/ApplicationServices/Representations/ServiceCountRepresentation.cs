using System;
using System.Runtime.Serialization;

namespace RentStuff.Services.Application.ApplicationServices.Representations
{
    /// <summary>
    /// Represents the count of houses and the page size; used for pagination by the frontend
    /// </summary>
    [Serializable]
    [DataContract]
    public class ServiceCountRepresentation
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ServiceCountRepresentation(int recordCount, int pageSize)
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
