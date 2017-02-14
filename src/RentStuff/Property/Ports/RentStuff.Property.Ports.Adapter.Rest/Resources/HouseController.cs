using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Newtonsoft.Json;
using RentStuff.Common;
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

        /// <summary>
        /// Save the House instance.
        /// After saving the house, the images for that House need to be uploaded using another POSt call to 
        /// 'HouseImageUplaod' method
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        [Route("house")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] Object house)
        {
            try
            {
                if (house != null)
                {
                    var jsonString = house.ToString();
                    CreateHouseCommand houseReborn = JsonConvert.DeserializeObject<CreateHouseCommand>(jsonString);
                    return Ok(_houseApplicationService.SaveNewHouseOffer(houseReborn));
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Separate method is used for uploading the images for a house. So two POST calls are involved; 
        /// One for saving the House, and then another(this one) to save the images related to a house
        /// </summary>
        /// <returns></returns>
        [Route("HouseImageUpload")]
        [HttpPost]
        public IHttpActionResult PostImageUpload()
        {
            bool imageUploaded = false;
            try
            {
                //var result = new HttpResponseMessage(HttpStatusCode.OK);
                //var httpRequest = HttpContext.Current.Request;
                if (Request.Content.IsMimeMultipartContent())
                {
                    Request.Content.LoadIntoBufferAsync().Wait();
                    Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(
                        new MultipartMemoryStreamProvider()).ContinueWith((task) =>
                    {
                        IList<string> imagesList = new List<string>();
                        MultipartMemoryStreamProvider provider = task.Result;
                        foreach (HttpContent content in provider.Contents)
                        {
                            Stream stream = content.ReadAsStreamAsync().Result;
                            var image = Image.FromStream(stream);
                            var testName = content.Headers.ContentDisposition.Name;
                            String filePath = HostingEnvironment.MapPath(Constants.HOUSEIMAGESDIRECTORY);

                            string imageId = Guid.NewGuid().ToString();
                            String fileName = imageId + ".jpg";
                            String fullPath = Path.Combine(filePath, fileName);
                            image.Save(fullPath);

                            imagesList.Add(fileName);
                        }
                        String[] headerValues = (String[])Request.Headers.GetValues("HouseId");
                        _houseApplicationService.AddImagesToHouse(headerValues[0], imagesList);
                        imageUploaded = true;
                    });
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                }
                return Ok(imageUploaded);
            }
            catch (Exception exception)
            {
                return InternalServerError();
            }
        }

        [Route("house")]
        [HttpGet]
        public IHttpActionResult GetHouse(string email = null, string area = null, string propertyType = null, string houseId = null)
        {
            try
            {
                if (area != null && propertyType != null)
                {
                    return Ok(_houseApplicationService.SearchHousesByAddressAndPropertyType(area, propertyType));
                }
                else if (email != null)
                {
                    return Ok(_houseApplicationService.GetHouseByEmail(email));
                }
                else if (!string.IsNullOrEmpty(houseId))
                {
                    return Ok(_houseApplicationService.GetHouseById(houseId));
                }
                else
                {
                    return Ok(_houseApplicationService.GetAllHouses());
                }
            }
            catch (Exception exception)
            {
                return InternalServerError();
            }
        }

        [Route("GetHouseImages")]
        [HttpGet]
        public HttpResponseMessage GetHouseImages(string houseId)
        {
            House house = null;//_houseApplicationService.GetHouseById(houseId);
            if (house == null)
            {
                throw new NullReferenceException("No house found for ID: " + houseId);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var multipartContent = new MultipartContent();
                        
            foreach (var imageId in house.HouseImages)
            {
                String filePath = HostingEnvironment.MapPath(Constants.HOUSEIMAGESDIRECTORY + imageId);
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                Image image = Image.FromStream(fileStream);
                MemoryStream memoryStream = new MemoryStream();
                image.Save(memoryStream, ImageFormat.Jpeg);
                
                var byteArrayContent = new ByteArrayContent(memoryStream.ToArray());
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                byteArrayContent.Headers.ContentLength = memoryStream.ToArray().Length;
                
                //var fileStream = new FileStream(HostingEnvironment.MapPath("~/Images/" + imageId + ".jpg"),
                  //  FileMode.Open);
                //var file1Content = new StreamContent(fileStream);

                //file1Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpeg");
                //multipartContent.Add(byteArrayContent);
                //fileStream.Close();
                //fileStream.Flush();
            }
            result.Content = multipartContent;
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");    
            // We can also return with the type HttpResponseMessage        
            return result;

            // We can also return HttpResponseMessage wrapped as IHttpActionaResult
            //IHttpActionResult response = ResponseMessage(result);
            //return response;
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
        
        [Route("property-types")]
        [HttpGet]
        public IHttpActionResult GetPropertyTypes()
        {
            try
            {
                return Ok(_houseApplicationService.GetPropertyTypes());
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
