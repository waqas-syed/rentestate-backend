using System.Collections.Generic;

namespace RentStuff.Services.Domain.Model.ServiceAggregate
{
    /// <summary>
    /// Returns a list of Professions that we can use in our app. No need to store in the database
    /// </summary>
    internal static class ServiceProfessions
    {
        private static readonly List<string> HouseMaintenanceServices = new List<string>()
        {
            // Individual Services
            "Plumber",
            "Electrician",
            "Carpenter",
            "Mason/Mistry",
            "Maid",
            "Welder",
			"Wall Painter",
            "Fire Safety Engineering"
        };

        private static readonly List<string> HouseDecorationServices = new List<string>()
        {
            "Wall Grace Painter",
            "Wall Panel Installer",
            "Interior Designing",
            "Furnishing(Furniture)"
        };

        private static readonly List<string> ApplianceTechnicians = new List<string>()
        {
            "Tv Technician",
            "Fridge Technician",
            "Electric Motor Technician",
            "Geyser Technician",
            "AC Technician"
        };

        private static readonly List<string> FestivityServices = new List<string>()
        {
            "Butcher/Qassai",
            "Light Decoration",
			"Food Catering",
            "Events Management",
			"Flower Decoration"
        };

        private static readonly List<string> AcademicServices = new List<string>()
        {
            "Tution"
        };

        private static readonly List<string> VehicleServices = new List<string>()
        {
            "Rent A Car",
            "Car Mechanic",
            "Car Electrician",
            "Car Denting/Painting",
            "Vehicle Body Parts",
            "Car Tow/Recovery"
        };

        private static readonly List<string> GoodsAndTransportServices = new List<string>()
        {
            "Cargo Service"
        };

        private static readonly List<string> MediaServices = new List<string>()
        {
            "Cable Tv Service",
            "Dish Antenna Service"
        };
        private static readonly List<string> BeauticianServices = new List<string>()
        {
            "Makeup Artist/Beautician"
        };

        private static IReadOnlyDictionary<string, IReadOnlyList<string>> _allServices;

        /// <summary>
        /// Populate the dictionary
        /// </summary>
        private static IReadOnlyDictionary<string, IReadOnlyList<string>> PopulateDictionary()
        {
            return new Dictionary
            <string, IReadOnlyList<string>>()
            {
                { "House Maintenance Services", HouseMaintenanceServices.AsReadOnly() },
                { "House Decoration Services", HouseDecorationServices.AsReadOnly() },
                { "Appliance Technicians", ApplianceTechnicians.AsReadOnly() },
                { "Festivity Services", FestivityServices.AsReadOnly() },
                { "Academic Services", AcademicServices.AsReadOnly() },
                { "Vehicle Services", VehicleServices.AsReadOnly() },
                { "Goods and Transport Services", GoodsAndTransportServices.AsReadOnly() },
                { "Media Services", MediaServices.AsReadOnly() },
                { "Beautician Services", BeauticianServices.AsReadOnly() }
            };
        }

        internal static IReadOnlyDictionary<string, IReadOnlyList<string>> AllProfessions
        {
            get { return _allServices ?? (_allServices = PopulateDictionary()); }
        }
    }
}
