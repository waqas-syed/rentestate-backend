using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.Services;

namespace RentStuff.Property.Application.HouseServices
{
    /// <summary>
    /// House Application Service
    /// </summary>
    public class HouseApplicationService : IHouseApplicationService
    {
        private IHouseRepository _houseRepository;
        private IGeocodingService _geocodingService;
        private ILocationRepository _locationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HouseApplicationService(IHouseRepository houseRepository, IGeocodingService geocodingService,
            ILocationRepository locationRepository)
        {
            _houseRepository = houseRepository;
            _geocodingService = geocodingService;
            _locationRepository = locationRepository;
        }

        /// <summary>
        /// Saves a new house instance to the database
        /// </summary>
        public void SaveNewHouseOffer(CreateHouseCommand createHouseCommand)
        {
            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal,decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(createHouseCommand.Area);

            // Create the location value object instance for the current house
            Location location = new Location(coordinates.Item1, coordinates.Item2, createHouseCommand.HouseNo,
                createHouseCommand.StreetNo, createHouseCommand.Area);
            _locationRepository.SaveOrUpdate(location);

            PropertyType propertyType = default(PropertyType);
            if (!string.IsNullOrEmpty(createHouseCommand.PropertyType))
            {
                Enum.TryParse(createHouseCommand.PropertyType, out propertyType);
            }

            // Create the new house instance
            House house = new House.HouseBuilder().BoysOnly(createHouseCommand.BoysOnly)
                .CableTvAvailable(createHouseCommand.CableTvAvailable).FamiliesOnly(createHouseCommand.FamiliesOnly)
                .GarageAvailable(createHouseCommand.GarageAvailable).GirlsOnly(createHouseCommand.GirlsOnly)
                .LandlinePhoneAvailable(createHouseCommand.LandlinePhoneAvailable).MonthlyRent(createHouseCommand.MonthlyRent)
                .NumberOfBathrooms(createHouseCommand.NumberOfBathrooms).NumberOfBedrooms(createHouseCommand.NumberOfBedrooms)
                .NumberOfKitchens(createHouseCommand.NumberOfKitchens).OwnerEmail(createHouseCommand.OwnerEmail)
                .OwnerPhoneNumber(createHouseCommand.OwnerPhoneNumber).SmokingAllowed(createHouseCommand.SmokingAllowed)
                .WithInternetAvailable(createHouseCommand.InternetAvailable).Location(location).PropertyType(propertyType).Build();
            
            // Save new the house instance
            _houseRepository.SaveorUpdate(house);
        }

        /// <summary>
        /// Delete the given house instance
        /// </summary>
        /// <param name="house"></param>
        public void DeleteHouse(House house)
        {
            _houseRepository.Delete(house);
        }

        /// <summary>
        /// Delete the house by providing the id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteHouseById(string id)
        {
            House house = _houseRepository.GetHouseById(id);
            if (house != null)
            {
                _houseRepository.Delete(house);
            }else
            {
                throw new InstanceNotFoundException("No house could be found for the given house id = " + id);
            }
        }

        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        public IList<House> GetHouseByEmail(string email)
        {
            return _houseRepository.GetHouseByOwnerEmail(email);
        }

        /// <summary>
        /// Gets all the houses
        /// </summary>
        /// <returns></returns>
        public IList<House> GetAllHouses()
        {
            return _houseRepository.GetAllHouses();
        }
    }
}
