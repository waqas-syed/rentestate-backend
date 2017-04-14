using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        [Authorize]
        public IHttpActionResult Post([FromBody] Object house)
        {
            try
            {
                if (house != null)
                {
                    var jsonString = house.ToString();

                    CreateHouseCommand houseReborn = null;
                    // First try to convert it to CreateHouseCommand
                    houseReborn = JsonConvert.DeserializeObject<CreateHouseCommand>(jsonString);
                    ConfirmHouseOwner(houseReborn.OwnerEmail);
                    return Ok(_houseApplicationService.SaveNewHouseOffer(houseReborn));
                }
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
            return BadRequest();
        }

        [Route("house")]
        [HttpPut]
        [Authorize]
        public IHttpActionResult Put([FromBody] Object house)
        {
            try
            {
                if (house != null)
                {
                    var jsonString = house.ToString();

                    UpdateHouseCommand refurbishedHouse = null;
                    // If above fails, then try to convert it to UpdateHouseCommand
                    refurbishedHouse = JsonConvert.DeserializeObject<UpdateHouseCommand>(jsonString);
                    ConfirmHouseOwner(refurbishedHouse.OwnerEmail);
                    return Ok(_houseApplicationService.UpdateHouse(refurbishedHouse));
                }
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
            return BadRequest();
        }

        private void ConfirmHouseOwner(string houseOwnerEmail)
        {
            // Check if the current caller is using his own email in the CreateHouseCommand to upload a new house
            var currentUserEmail = User.Identity.Name;
            if (!currentUserEmail.Equals(houseOwnerEmail))
            {
                throw new InvalidOperationException("Current user cannot upload house using another user's email.");
            }
        }

        /// <summary>
        /// Separate method is used for uploading the images for a house. So two POST calls are involved; 
        /// One for saving the House, and then another(this one) to save the images related to a house
        /// </summary>
        /// <returns></returns>
        [Route("HouseImageUpload")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostImageUpload()
        {
            bool imageUploaded = false;
            String[] headerValues = (String[])Request.Headers.GetValues("HouseId");
            string houseId = headerValues[0];
            var userEmail = User.Identity.Name;
            bool allowedToEditHouse = _houseApplicationService.HouseOwnershipEmailCheck(houseId, userEmail);
            
            try
            {
                if (allowedToEditHouse)
                {
                    //var result = new HttpResponseMessage(HttpStatusCode.OK);
                    //var httpRequest = HttpContext.Current.Request;
                    if (Request.Content.IsMimeMultipartContent())
                    {
                        Request.Content.LoadIntoBufferAsync().Wait();

                        Task<MultipartMemoryStreamProvider> imageSavetask = Request.Content
                            .ReadAsMultipartAsync<MultipartMemoryStreamProvider>(
                                new MultipartMemoryStreamProvider());
                        imageSavetask.Wait(10000);
                        imageSavetask.ContinueWith((task) =>
                        {
                            
                        }).Wait(10000);
                        IList<string> imagesList = new List<string>();
                        MultipartMemoryStreamProvider provider = imageSavetask.Result;
                        foreach (HttpContent content in provider.Contents)
                        {
                            Stream stream = content.ReadAsStreamAsync().Result;
                            var image = Image.FromStream(stream);
                            var testName = content.Headers.ContentDisposition.Name;
                            String filePath = HostingEnvironment.MapPath(Constants.HOUSEIMAGESDIRECTORY);

                            string imageId = "IMG_" + Guid.NewGuid().ToString();
                            String fileName = imageId + ".jpg";
                            String fullPath = Path.Combine(filePath, fileName);
                            image.Save(fullPath);
                            imagesList.Add(fileName);
                        }

                        _houseApplicationService.AddImagesToHouse(houseId, imagesList);
                        imageUploaded = true;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                            "This request is not properly formatted"));
                    }
                    return Ok(imageUploaded);
                }
                else
                {
                    throw new InvalidOperationException("The logged in user is not the actual poster for the requested house. Action will be taken now against this user");
                }
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [Route("houseimageupload")]
        [HttpPut]
        [Authorize]
        public IHttpActionResult ImageDelete([FromBody] DeleteImageCommand deleteImageCommand)
        {
            var userEmail = User.Identity.Name;
            bool allowedToEditHouse = _houseApplicationService.HouseOwnershipEmailCheck(deleteImageCommand.HouseId, userEmail);

            try
            {
                if (allowedToEditHouse)
                {
                    Task.Run(() => _houseApplicationService.DeleteImageFromHouse(deleteImageCommand.HouseId, deleteImageCommand.ImagesList)).Wait(2000);
                    return Ok();
                }
                else
                {
                    throw new InvalidOperationException("The logged in user is not the actual poster for the requested house. Action will be taken now against this user");
                }
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
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
                    return Ok(_houseApplicationService.SearchHousesByAreaAndPropertyType(area, propertyType));
                }
                else if (area != null)
                {
                    return Ok(_houseApplicationService.SearchHousesByArea(area));
                }
                else if (propertyType != null)
                {
                    return Ok(_houseApplicationService.SearchHousesByPropertyType(propertyType));
                }
                else if (email != null)
                {
                    if (!string.IsNullOrWhiteSpace(User.Identity?.Name))
                    {
                        if (email.Equals(User.Identity.Name))
                        {
                            return Ok(_houseApplicationService.GetHouseByEmail(email));
                        }
                        else
                        {
                            throw new InvalidOperationException("Only user's own houses can be retreived by email");
                        }
                    }
                    else
                    {
                        throw new NullReferenceException("User must be logged in for this request");
                    }
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
                return BadRequest(exception.ToString());
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

        [Route("house/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var userEmail = User.Identity.Name;
                    bool allowedToEditHouse = _houseApplicationService.HouseOwnershipEmailCheck(id, userEmail);
                    if (allowedToEditHouse)
                    {
                        _houseApplicationService.DeleteHouse(id);
                        return Ok();
                    }
                    else
                    {
                        throw new InvalidOperationException("Current user is not allowed to delete this house. Security breach, taking necessary action");
                    }
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
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
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
