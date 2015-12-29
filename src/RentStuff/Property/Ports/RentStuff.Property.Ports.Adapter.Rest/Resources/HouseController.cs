using System;
using System.Web.Http;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Ports.Adapter.Rest.Resources
{
    /// <summary>
    /// Gateway for the house resource
    /// </summary>
    [RoutePrefix("v1")]
    public class HouseController : ApiController
    {
        private IHouseApplicationService _houseApplicationService;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="houseApplicationService"></param>
        public HouseController(IHouseApplicationService houseApplicationService)
        {
            _houseApplicationService = houseApplicationService;
        }

        [Route("house")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]CreateHouseCommand house)
        {
            try
            {
                if (house != null)
                {
                    _houseApplicationService.SaveNewHouseOffer(house);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("house")]
        [HttpDelete]
        public IHttpActionResult Delete([FromBody]string houseId)
        {
            try
            {
                if (!string.IsNullOrEmpty(houseId))
                {
                    _houseApplicationService.DeleteHouseById(houseId);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("house")]
        [HttpDelete]
        public IHttpActionResult Delete([FromBody]House house)
        {
            try
            {
                if (house != null)
                {
                    _houseApplicationService.DeleteHouse(house);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("house")]
        [HttpGet]
        public IHttpActionResult Get(string email = null, string address = null)
        {
            try
            {
                if (address != null)
                {
                    return Ok(_houseApplicationService.SearchHousesByAddress(address));
                }
                else if (email != null)
                {
                    return Ok(_houseApplicationService.GetHouseByEmail(email));
                }
                else
                {
                    return Ok(_houseApplicationService.GetAllHouses());
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
