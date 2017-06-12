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
            "Car Tow/Recovery",
            "Butcher/Qassai",
            "Makeup Artist/Beautician",
            "Car Mechanic",
            "Car Electrician",
            "Wall Painter",
            "Tv Technician",
            "Fridge Technician",
            "Electric Motor Technician",
            "Geyser Technician",
            "Ac Technician",
            "Welder",
            "Wall Grace Painter",
            "Wall Panel Installer",

            // Organization(or possibly individual)
            "Tutor Service",
            "Light Decoration",
            "Food Catering",
            "Interior Designing",
            "Cargo Service",
            "Furnishing(Furniture)",
            "Fire Safety Engineering",
            "Car Denting/Painting",
            "Vehicle Body Parts",
            "Cable Tv Service",
            "Dish Antenna Service"
        };

        internal static IReadOnlyList<string> List { get { return _serviceProfessionList.AsReadOnly(); } }
    }
}
