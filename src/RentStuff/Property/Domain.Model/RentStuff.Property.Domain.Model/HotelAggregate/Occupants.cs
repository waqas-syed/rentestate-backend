using RentStuff.Common.Domain.Model;
using System;

namespace RentStuff.Property.Domain.Model.HotelAggregate
{
    /// <summary>
    /// The Number of occupants this room can accommodate
    /// </summary>
    public class Occupants : Entity
    {
        public Occupants()
        {
            
        }

        public Occupants(int adults, int children)
        {
            Adults = adults;
            Children = children;
        }

        public Occupants(int adults, int children, Hotel hotel)
        {
            Adults = adults;
            Children = children;
            Hotel = hotel;
        }

        public void Update(int adults, int children)
        {
            Adults = adults;
            Children = children;
        }

        public int Adults { get; set; }

        public int Children { get; set; }

        public int TotalOccupants { get { return Adults + Children; } private set {} }

        public Hotel Hotel { get; set; }
    }
}
