using System;
using System.Collections.Generic;
using System.IO;
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
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HouseApplicationService(IHouseRepository houseRepository, IGeocodingService geocodingService)
        {
            _houseRepository = houseRepository;
            _geocodingService = geocodingService;
        }

        /// <summary>
        /// Saves a new house instance to the database
        /// </summary>
        public bool SaveNewHouseOffer(CreateHouseCommand createHouseCommand)
        {
            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal,decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(createHouseCommand.Area);

            if (coordinates == null || coordinates.Item1 == decimal.Zero || coordinates.Item2 == decimal.Zero)
            {
                throw new InvalidDataException("Could not find coordinates from the given address");
            }
            PropertyType propertyType = default(PropertyType);
            if (!string.IsNullOrEmpty(createHouseCommand.PropertyType))
            {
                Enum.TryParse(createHouseCommand.PropertyType, out propertyType);
            }

            var dimension = new Dimension((DimensionType)Enum.Parse(typeof(DimensionType), createHouseCommand.DimensionType), createHouseCommand.DimensionStringValue, createHouseCommand.DimensionIntValue);
            _houseRepository.SaveorUpdateDimension(dimension);
            // Create the new house instance
            House house = new House.HouseBuilder().BoysOnly(createHouseCommand.BoysOnly)
                .CableTvAvailable(createHouseCommand.CableTvAvailable)
                .FamiliesOnly(createHouseCommand.FamiliesOnly)
                .GarageAvailable(createHouseCommand.GarageAvailable)
                .GirlsOnly(createHouseCommand.GirlsOnly)
                .LandlinePhoneAvailable(createHouseCommand.LandlinePhoneAvailable)
                .MonthlyRent(createHouseCommand.MonthlyRent)
                .NumberOfBathrooms(createHouseCommand.NumberOfBathrooms)
                .NumberOfBedrooms(createHouseCommand.NumberOfBedrooms)
                .NumberOfKitchens(createHouseCommand.NumberOfKitchens)
                .OwnerEmail(createHouseCommand.OwnerEmail)
                .OwnerPhoneNumber(createHouseCommand.OwnerPhoneNumber)
                .SmokingAllowed(createHouseCommand.SmokingAllowed)
                .WithInternetAvailable(createHouseCommand.InternetAvailable)
                .PropertyType(propertyType)
                .Latitude(coordinates.Item1)
                .Longitude(coordinates.Item2)
                .Dimension(dimension)
                .HouseNo(createHouseCommand.HouseNo)
                .StreetNo(createHouseCommand.StreetNo)
                .Area(createHouseCommand.Area).Build();

            // Save the new house instance
            _houseRepository.SaveorUpdate(house);

            return true;
        }

        /// <summary>
        /// Updates an exisitng house
        /// </summary>
        /// <param name="updateHouseCommand"></param>
        /// <returns></returns>
        public bool UpdateHouse(UpdateHouseCommand updateHouseCommand)
        {
            House house = _houseRepository.GetHouseById(updateHouseCommand.Id);
            if (house == null)
            {
                throw new InstanceNotFoundException(string.Format("House not found for id: {0}", updateHouseCommand.Id));
            }
            house.UpdateHouse(updateHouseCommand.MonthlyRent, updateHouseCommand.NumberOfBedrooms, updateHouseCommand.NumberOfKitchens,
                updateHouseCommand.NumberOfBathrooms, updateHouseCommand.FamiliesOnly, updateHouseCommand.BoysOnly, 
                updateHouseCommand.GirlsOnly, updateHouseCommand.InternetAvailable, updateHouseCommand.LandlinePhoneAvailable,
                updateHouseCommand.CableTvAvailable, new Dimension(DimensionType.Acre, updateHouseCommand.DimensionStringValue, updateHouseCommand.DimensionIntValue), 
                    updateHouseCommand.GarageAvailable, updateHouseCommand.SmokingAllowed,updateHouseCommand.PropertyType,
                    updateHouseCommand.OwnerEmail, updateHouseCommand.OwnerPhoneNumber, updateHouseCommand.Latitude,
                    updateHouseCommand.Longitude, updateHouseCommand.HouseNo, updateHouseCommand.StreetNo, updateHouseCommand.Area);

            _houseRepository.SaveorUpdate(house);
            return true;
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
        /// Search nearby houses by providing the address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public IList<House> SearchHousesByAddress(string address)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(address);
            // Get 20 coordinates within the range of around 30 kilometers radius
            return (IList<House>) _houseRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2);
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
