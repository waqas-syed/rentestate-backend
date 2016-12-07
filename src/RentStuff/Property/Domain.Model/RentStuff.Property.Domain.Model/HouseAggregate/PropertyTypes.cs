using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Contains a list of Property Types
    /// </summary>
    public class PropertyTypes
    {
        private IList<PropertyType> _propertyTypes;

        public IList<PropertyType> PropertyTypesList
        {
            get { return _propertyTypes; }
            set { _propertyTypes = value; }
        }
    }
}
