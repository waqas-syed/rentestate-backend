using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.HotelAggregate
{
    /// <summary>
    /// The Number of occupants this room can accommodate
    /// </summary>
    public class Occupants
    {
        private int _adults;
        private int _children;

        public Occupants(int adults, int children)
        {
            Adults = adults;
            Children = children;
        }

        public void Update(int adults, int children)
        {
            Adults = adults;
            Children = children;
        }

        public int Adults { get { return _adults; } private set { _adults = value; } }

        public int Children { get { return _children; } private set { _children = value; } }

        public int TotalOccupants { get { return _adults + _children; } private set {} }
    }
}
