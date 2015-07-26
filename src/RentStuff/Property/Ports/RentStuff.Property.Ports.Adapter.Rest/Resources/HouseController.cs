using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using RentStuff.Property.Application.HouseServices;
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
        public IHttpActionResult Save([FromBody]House house)
        {
            try
            {
                if (house != null)
                {
                    _houseApplicationService.SaveNewHouseOffer(house);
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
        public IHttpActionResult Delete([FromBody]long houseId)
        {
            try
            {
                if (houseId != 0)
                {
                    _houseApplicationService.DeleteHouseById(houseId);
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
        public IHttpActionResult Get(string email = null)
        {
            try
            {
                if (email != null)
                {
                    return Ok(_houseApplicationService.GetHouseByEmail(email));
                }
                else
                {
                    return Ok(_houseApplicationService.GetAllHouses());
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
