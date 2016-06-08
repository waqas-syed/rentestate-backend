using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Newtonsoft.Json;
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
        public IHttpActionResult Post([FromBody]Object house)
        {
            try
            {
                if (house != null)
                {
                    var jsonString = house.ToString();
                    CreateHouseCommand houseReborn = JsonConvert.DeserializeObject<CreateHouseCommand>(jsonString);
                    _houseApplicationService.SaveNewHouseOffer(houseReborn);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        /*[Route("house")]
        [HttpPost]
        public IHttpActionResult Post()
        {
            try
            {
                //var jsonString = house.ToString();
                //CreateHouseCommand houseResult = JsonConvert.DeserializeObject<CreateHouseCommand>(jsonString);
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                var httpRequest = HttpContext.Current.Request;
                if (Request.Content.IsMimeMultipartContent())
                {
                    Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider()).ContinueWith((task) =>
                    {
                        MultipartMemoryStreamProvider provider = task.Result;
                        foreach (HttpContent content in provider.Contents)
                        {
                            Stream stream = content.ReadAsStreamAsync().Result;
                            //var image = MediaTypeNames.Image.FromStream(stream);
                            var testName = content.Headers.ContentDisposition.Name;
                            String filePath = HostingEnvironment.MapPath("~/Images/");
                            String[] headerValues = (String[])Request.Headers.GetValues("UniqueId");
                            String fileName = headerValues[0] + ".jpg";
                            String fullPath = Path.Combine(filePath, fileName);
                            //image.Save(fullPath);
                        }
                    });
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }*/

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
