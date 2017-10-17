using System;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using NLog;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Application.HouseServices.Commands.CreateCommands;
using RentStuff.Property.Application.HouseServices.Commands.DeleteCommands;
using RentStuff.Property.Application.HouseServices.Commands.UpdateCommands;

namespace RentStuff.Property.Ports.Adapter.Rest.Resources
{
    /// <summary>
    /// Gateway for the house resource
    /// </summary>
    [RoutePrefix("v1")]
    public class HouseController : ApiController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IPropertyApplicationService _houseApplicationService;
        
        /// <summary>
        /// Initialize with House's application service
        /// </summary>
        /// <param name="houseApplicationService"></param>
        public HouseController(IPropertyApplicationService houseApplicationService)
        {
            _houseApplicationService = houseApplicationService;
        }

        /// <summary>
        /// Save the House instance.
        /// After saving the house, the images for that House need to be uploaded using another PoSt call to 
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
                // Nul reference check
                if (house != null)
                {
                    var jsonString = house.ToString();

                    CreateHouseCommand houseReborn = null;
                    // First try to convert it to CreateHouseCommand
                    houseReborn = JsonConvert.DeserializeObject<CreateHouseCommand>(jsonString);
                    // Check if the house's owner email is the same as the one the current user is logged in with
                    CheckOwnerEmailIntegrity(houseReborn.OwnerEmail);
                    // Save the new house
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
                    // Try to convert the JSON into a domain model object
                    refurbishedHouse = JsonConvert.DeserializeObject<UpdateHouseCommand>(jsonString);
                    // First check that the request initiator has the same email as the uploader of the house
                    CheckOwnerEmailIntegrity(refurbishedHouse.OwnerEmail);
                    // Then check that this house was actually upoaded by the current requestor
                    _houseApplicationService.HouseOwnershipCheck(refurbishedHouse.Id,
                                                                        refurbishedHouse.OwnerEmail);
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

        /// <summary>
        /// Checks that the current user's email and the request house's uploader emails are the same
        /// </summary>
        /// <param name="houseOwnerEmail"></param>
        /// <returns></returns>
        private void CheckOwnerEmailIntegrity(string houseOwnerEmail)
        {
            // Check if the current caller is using his own email in the CreateHouseCommand to upload a new house
            var currentUserEmail = GetEmailFromClaims(User.Identity);
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
            // Get the HouseId from the header
            String[] headerValues = (String[]) Request.Headers.GetValues("HouseId");
            string houseId = headerValues[0];
            // Get the email from the claims identity
            var userEmail = GetEmailFromClaims(User.Identity);
            // Make sure that this house belongs to this logged in user
            bool allowedToEditHouse = _houseApplicationService.HouseOwnershipCheck(houseId, userEmail);
            try
            {
                if (allowedToEditHouse)
                {
                    // We need a multipart message in order for it to contains pictures
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
                            // Fetch as stream
                            using (Stream stream = content.ReadAsStreamAsync().Result)
                            {
                                // Add the image to the house
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
        
        /// <summary>
        ///  Delete the given image from the house
        /// </summary>
        /// <param name="deleteImageCommand"></param>
        /// <returns></returns>
        [Route("houseimageupload")]
        [HttpPut]
        [Authorize]
        public IHttpActionResult ImageDelete([FromBody] DeleteImageCommand deleteImageCommand)
        {
            // Get the email from the claims identity
            var userEmail = GetEmailFromClaims(User.Identity);
            // Make sure the logged in user is the owner of the house
            bool allowedToEditHouse = _houseApplicationService.HouseOwnershipCheck(deleteImageCommand.HouseId,
                userEmail);

            try
            {
                if (allowedToEditHouse)
                {
                    // Delete the images from the house using the application service
                    _houseApplicationService.DeleteImagesFromHouse(deleteImageCommand.HouseId,
                        deleteImageCommand.ImagesList);
                    return Ok();
                }
                else
                {
                    // Otherwise someone tried to misuse our service. Send back error
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

        /// <summary>
        /// Get the total number of houses in the database for the given criteria
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="area"></param>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the house(s) by the given criteria. Can be one or multiple
        /// </summary>
        /// <param name="email"></param>
        /// <param name="area"></param>
        /// <param name="propertyType"></param>
        /// <param name="houseId"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
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
                // If both area and property type are given
                if (!string.IsNullOrWhiteSpace(area) && !string.IsNullOrWhiteSpace(propertyType))
                {
                    _logger.Info("Get House by Area {0} and Property Type {1}", area, propertyType);
                    return Ok(_houseApplicationService.SearchPropertiesByAreaAndPropertyType(area, propertyType, pageNo));
                }
                // If only email is given
                else if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(propertyType))
                {
                    // Get the email from the identity
                    var emailFromClaims = GetEmailFromClaims(User.Identity);
                    if (!string.IsNullOrWhiteSpace(emailFromClaims))
                    {
                        // If the requested house's owner email is the same as the logged in user's email, move 
                        // forward, otherwise return an error.
                        if (email.Equals(emailFromClaims))
                        {
                            _logger.Info("Get House by Email {0}", email);
                            return Ok(_houseApplicationService.GetPropertiesByEmail(propertyType, email, pageNo));
                        }
                        else
                        {
                            _logger.Error("Only user's own houses can be retreived by email. CurrentUserEmail: {0}", email);
                            return BadRequest("Only user's own houses can be retreived by email");
                        }
                    }
                    else
                    {
                        // The user is not authorized to access this information
                        return Unauthorized();
                    }
                }
                // If only property type is given
                else if (!string.IsNullOrWhiteSpace(propertyType))
                {
                    _logger.Info("Get House by Property Type {0}", propertyType);
                    return Ok(_houseApplicationService.SearchPropertiesByPropertyType(propertyType, pageNo));
                }
                else if (!string.IsNullOrEmpty(houseId) && !string.IsNullOrWhiteSpace(propertyType))
                {
                    _logger.Info("Get House by HouseId {0}", houseId);
                    return Ok(_houseApplicationService.GetPropertyById(houseId, propertyType));
                }
                else
                {
                    return BadRequest("An appropriate parameter must be supplied to return data");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception.ToString());
            }
        }
        
        /// <summary>
        /// Delete the house with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("house/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    // Get email
                    var userEmail = GetEmailFromClaims(User.Identity);
                    // Check if its the same user who owns this house
                    bool allowedToEditHouse = _houseApplicationService.HouseOwnershipCheck(id, userEmail);
                    // If so, proceed
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
        
        /// <summary>
        /// Get the property types that we support, like house, apartment etc
        /// </summary>
        /// <returns></returns>
        [Route("property-type")]
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

        /// <summary>
        /// Get all the rent units we support, like per hour, per day etc.
        /// </summary>
        /// <returns></returns>
        [Route("rent-unit")]
        [HttpGet]
        public IHttpActionResult GetRentUnits()
        {
            try
            {
                return Ok(_houseApplicationService.GetAllRentUnits());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /// <summary>
        /// Get the email address, either from Identity.Name or ClaimsIdentity's email address
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        private string GetEmailFromClaims(IIdentity identity)
        {
            if (!string.IsNullOrWhiteSpace(User.Identity?.Name))
            {
                return User.Identity.Name;
            }
            else
            {
                var claimsIdentity = identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    var email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        return email;
                    }
                }
                return null;
            }
        }
    }
}
