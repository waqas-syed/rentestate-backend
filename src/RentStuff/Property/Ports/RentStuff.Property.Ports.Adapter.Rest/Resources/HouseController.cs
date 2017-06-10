using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using NLog;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;

namespace RentStuff.Property.Ports.Adapter.Rest.Resources
{
    /// <summary>
    /// Gateway for the house resource
    /// </summary>
    [RoutePrefix("v1")]
    public class HouseController : ApiController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
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
                _logger.Error("Error occured while uploading house. Exception: {0} | House: {1}", exception, house);
                return InternalServerError();
            }
            return BadRequest();
        }

        [Route("house")]
        [HttpPut]
        [Authorize]
        [AcceptVerbs(new string[] {"OPTIONS", "PUT"})]
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
                _logger.Error("Error occured while updating house. Exception: {0} | House: {1}", exception, house);
                return InternalServerError();
            }
            return BadRequest();
        }

        private void ConfirmHouseOwner(string houseOwnerEmail)
        {
            // Check if the current caller is using his own email in the CreateHouseCommand to upload a new house
            var currentUserEmail = User.Identity.Name;
            if (!currentUserEmail.Equals(houseOwnerEmail))
            {
                _logger.Error(
                    "Current user cannot upload house using another user's email. CurrentUser:{0} | HouseOwner:{1}",
                    currentUserEmail, houseOwnerEmail);
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
            String[] headerValues = (String[]) Request.Headers.GetValues("HouseId");
            string houseId = headerValues[0];
            var userEmail = User.Identity.Name;
            bool allowedToEditHouse = _houseApplicationService.HouseOwnershipEmailCheck(houseId, userEmail);
            try
            {
                if (allowedToEditHouse)
                {
                    if (Request.Content.IsMimeMultipartContent())
                    {
                        // Load the HTTP request content into buffer asynchronously
                        Request.Content.LoadIntoBufferAsync().Wait();
                        // Read the multipart content from the HTTP request asynchronously
                        Task<MultipartMemoryStreamProvider> imageSavetask = Request.Content
                            .ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider());
                        imageSavetask.Wait(10000);
                        // Get the result of the task that was reading the multipart content
                        MultipartMemoryStreamProvider provider = imageSavetask.Result;
                        // For each file, do the processing
                        foreach (HttpContent content in provider.Contents)
                        {
                            using (Stream stream = content.ReadAsStreamAsync().Result)
                            {
                                _houseApplicationService.AddSingleImageToHouse(houseId, stream);
                                // Let the client know that the upload was successful
                                imageUploaded = true;
                            }
                        }
                    }
                    else
                    {
                        _logger.Error("Request for uploading image is not Mime Multipart content");
                        return BadRequest("This request is not properly formatted");
                    }
                    return Ok(imageUploaded);
                }
                else
                {
                    _logger.Error("Someone tried to upload an image for another user. HouseId:{0} | UserEmail:{1}",
                        houseId, userEmail);
                    return
                        BadRequest(
                            "The logged in user is not the actual poster for the requested house. Action will be taken now against this user");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(
                    "Error Occured while uploading image for house. HouseId:{0} | UserEmail:{1} | Exception:{2}",
                    houseId, userEmail, exception.ToString());
                return InternalServerError();
            }
        }
        
        [Route("houseimageupload")]
        [HttpPut]
        [Authorize]
        public IHttpActionResult ImageDelete([FromBody] DeleteImageCommand deleteImageCommand)
        {
            var userEmail = User.Identity.Name;
            bool allowedToEditHouse = _houseApplicationService.HouseOwnershipEmailCheck(deleteImageCommand.HouseId,
                userEmail);

            try
            {
                if (allowedToEditHouse)
                {
                    //Task.Run(() => _houseApplicationService.DeleteImageFromHouse(deleteImageCommand.HouseId, deleteImageCommand.ImagesList)).Wait(2000);
                    _houseApplicationService.DeleteImagesFromHouse(deleteImageCommand.HouseId,
                        deleteImageCommand.ImagesList);
                    return Ok();
                }
                else
                {
                    _logger.Error("Someone tried to upload an image for another user. HouseId:{0} | UserEmail:{1}",
                        deleteImageCommand.HouseId, userEmail);
                    return
                        BadRequest(
                            "The logged in user is not the actual poster for the requested house. Action will be taken now against this user");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return InternalServerError(exception);
            }
        }

        [Route("house-count")]
        [HttpGet]
        public IHttpActionResult GetHouseCount(string propertyType = null, string area = null, string email = null)
        {
            try
            {
                _logger.Info("Getting the house record count in the database");
                return Ok(_houseApplicationService.GetRecordsCount(propertyType, area, email));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return InternalServerError();
            }
        }

        [Route("house")]
        [HttpGet]
        public IHttpActionResult GetHouse(string email = null, string area = null, string propertyType = null, 
            string houseId = null, int pageNo = 1)
        {
            try
            {
                // The frontend starts with 1 as the first page, backend starts with 0. So we subtract 1 from whatever 
                // the page number is given.
                if (pageNo > 0)
                {
                    pageNo = pageNo - 1;
                }
                _logger.Info("Get House request received");
                if (area != null && propertyType != null)
                {
                    _logger.Info("Get House by Area {0} and Property Type {1}", area, propertyType);
                    return Ok(_houseApplicationService.SearchHousesByAreaAndPropertyType(area, propertyType, pageNo));
                }
                else if (area != null)
                {
                    _logger.Info("Get House by Area {0}", area);
                    return Ok(_houseApplicationService.SearchHousesByArea(area, pageNo));
                }
                else if (propertyType != null)
                {
                    _logger.Info("Get House by Property Type {0}", propertyType);
                    return Ok(_houseApplicationService.SearchHousesByPropertyType(propertyType, pageNo));
                }
                else if (email != null)
                {
                    if (!string.IsNullOrWhiteSpace(User.Identity?.Name))
                    {
                        if (email.Equals(User.Identity.Name))
                        {
                            _logger.Info("Get House by Email {0}", email);
                            return Ok(_houseApplicationService.GetHouseByEmail(email, pageNo));
                        }
                        else
                        {
                            _logger.Error("Only user's own houses can be retreived by email. CurrentUserEmail: {0}", email);
                            return BadRequest("Only user's own houses can be retreived by email");
                        }
                    }
                    else
                    {
                        //throw new NullReferenceException("User must be logged in for this request");
                        return Unauthorized();
                    }
                }
                else if (!string.IsNullOrEmpty(houseId))
                {
                    _logger.Info("Get House by HouseId {0}", houseId);
                    return Ok(_houseApplicationService.GetHouseById(houseId));
                }
                else
                {
                    return Ok(_houseApplicationService.GetAllHouses(pageNo));
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception.ToString());
            }
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
                        _logger.Error("Current user is not allowed to delete this house. CurrentUser: {0} | HouseId: {1}", userEmail, id);
                        return
                            BadRequest(
                                "Current user is not allowed to delete this house. Security breach, taking necessary action");
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
        [Obsolete]
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
