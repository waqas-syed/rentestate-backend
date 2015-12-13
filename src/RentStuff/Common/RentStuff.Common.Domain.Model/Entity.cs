using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Common.Domain.Model
{
    /// <summary>
    /// Base class for all entities
    /// </summary>
    public class Entity
    {
        private string _id = Guid.NewGuid().ToString();

        /// <summary>
        /// Unique GUID for every entity.
        /// </summary>
        public string Id
        {
            get { return _id; }
            private set { _id = value; }
        }
    }
}
