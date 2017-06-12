using System.Collections.Generic;

namespace RentStuff.Services.Domain.Model.ServiceAggregate
{
    /// <summary>
    /// Returns a list of Professions that we can use in our app. No need to store in the database
    /// </summary>
    internal static class ServiceProfessions
    {
        private static readonly List<string> _serviceProfessionList = new List<string>()
        {
            // Individual Services
            "Plumber",
            "Electrician",
            "Carpenter",
            "Mason/Mistry",
            "Maid",
            "Tv Technician",
            "Fridge Technician",
            "Electric Motor Technician",
            "Geyser Technician",
            "Makeup Artist/Beautician",
            "Butcher/Qassai",
            "Welder",

            // Organization(or possibly individual)
            "Food Catering",
            "Interior Designing",
            "Car Mechanic",
            "Ac Technician",
            "Cargo Service",
            "Furniture Furnishing",
            "Fire Safety Engineering",
            "Wall Painting",
            "Car Denting/Painting",
            "Vehicle Body Parts",
            "Light Decoration",
            "Cable Tv Service",
            "Dish Antenna Service",
            "Tutor Service"
        };

        internal static IReadOnlyList<string> List { get { return _serviceProfessionList.AsReadOnly(); } }
    }
}
