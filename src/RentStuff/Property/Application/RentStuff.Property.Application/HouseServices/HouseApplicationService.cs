using System.Collections.Generic;
using System.Management.Instrumentation;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Application.HouseServices
{
    /// <summary>
    /// House Application Service
    /// </summary>
    public class HouseApplicationService : IHouseApplicationService
    {
        private IHouseRepository _houseRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HouseApplicationService(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        /// <summary>
        /// Saves a new house instance to the database
        /// </summary>
        public void SaveNewHouseOffer(House house)
        {
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
        public void DeleteHouseById(long id)
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
