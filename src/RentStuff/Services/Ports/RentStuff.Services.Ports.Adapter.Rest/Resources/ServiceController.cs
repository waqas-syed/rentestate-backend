using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using NLog;
using RentStuff.Services.Application.ApplicationServices;
using RentStuff.Services.Application.ApplicationServices.Commands;

namespace RentStuff.Services.Ports.Adapter.Rest.Resources
{
    [RoutePrefix("v1")]
    public class ServiceController : ApiController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IServiceApplicationService _serviceApplicationService;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="serviceApplicationService"></param>
        public ServiceController(IServiceApplicationService serviceApplicationService)
        {
            _serviceApplicationService = serviceApplicationService;
        }

        [Route("service")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult Post([FromBody] Object service)
        {
            try
            {
                if (service != null)
                {
                    var jsonString = service.ToString();

                    CreateServiceCommand serviceBorn = null;
                    // First try to convert it to CreateServiceCommand
                    serviceBorn = JsonConvert.DeserializeObject<CreateServiceCommand>(jsonString);
                    // Check that the current user and the UploaderEmail are the same 
                    _serviceApplicationService.NewServiceEmailOwnershipCheck(User.Identity.Name, serviceBorn.UploaderEmail);
                    return Ok(_serviceApplicationService.SaveNewService(serviceBorn));
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Error occured while uploading Service. Exception: {0} | Service: {1}",
                    exception, service);
                return InternalServerError();
            }
            return BadRequest();
        }

        [Route("review")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReview([FromBody] Object reviewObject)
        {
            try
            {
                if (reviewObject != null)
                {
                    var jsonString = reviewObject.ToString();

                    AddReviewCommand serviceBorn = null;
                    // First try to convert it to CreateServiceCommand
                    serviceBorn = JsonConvert.DeserializeObject<AddReviewCommand>(jsonString);
                    // Check that the current user is the actual uploader of the Service
                    _serviceApplicationService.ServiceOwnershipCheck(serviceBorn.ServiceId, User.Identity.Name);
                    // Add the Review
                    _serviceApplicationService.AddReview(serviceBorn);
                    return Ok();
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Error occured while uploading Service. Exception: {0} | Service: {1}",
                    exception, reviewObject);
                return InternalServerError();
            }
            return BadRequest();
        }

        [Route("service")]
        [HttpPut]
        [Authorize]
        [AcceptVerbs(new string[] { "OPTIONS", "PUT" })]
        public IHttpActionResult Put([FromBody] Object service)
        {
            try
            {
                if (service != null)
                {
                    var jsonString = service.ToString();

                    UpdateServiceCommand modifiedService = null;
                    // Convert the Json string to UpdateServiceCommand
                    modifiedService = JsonConvert.DeserializeObject<UpdateServiceCommand>(jsonString);
                    // Check that the current user is the actual uploader of the Service
                    _serviceApplicationService.ServiceOwnershipCheck(modifiedService.Id, User.Identity.Name);
                    // Send the obejct to ApplicationService to update the given service
                    _serviceApplicationService.UpdateService(modifiedService);
                    return Ok();
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Error occured while updating Service. Exception: {0} | Service: {1}", 
                    exception, service);
                return InternalServerError();
            }
            return BadRequest();
        }
        
        /// <summary>
        /// Separate method is used for uploading the images for a Service. So two POST calls are involved; 
        /// One for saving the Service(in the above code), and then another(this one) to save the images 
        /// related to a Service
        /// </summary>
        /// <returns></returns>
        [Route("service-image-upload")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostImageUpload()
        {
            bool imageUploaded = false;
            String[] headerValues = (String[])Request.Headers.GetValues("HouseId");
            string serviceId = headerValues[0];
            var userEmail = User.Identity.Name;
            bool allowedToEditHouse = _serviceApplicationService.ServiceOwnershipCheck(serviceId, userEmail);
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
                                _serviceApplicationService.AddSingleImageToService(serviceId, stream);
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
                        serviceId, userEmail);
                    return
                        BadRequest(
                            "The logged in user is not the actual poster for the requested house. Action will be taken now against this user");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(
                    "Error Occured while uploading image for house. HouseId:{0} | UserEmail:{1} | Exception:{2}",
                    serviceId, userEmail, exception.ToString());
                return InternalServerError();
            }
        }

        [Route("service-image-upload")]
        [HttpPut]
        [Authorize]
        public IHttpActionResult ImageDelete([FromBody] DeleteImageCommand deleteImageCommand)
        {
            var userEmail = User.Identity.Name;
            bool allowedToEditHouse = _serviceApplicationService.ServiceOwnershipCheck(
                deleteImageCommand.ServiceId, userEmail);

            try
            {
                if (allowedToEditHouse)
                {
                    //Task.Run(() => _houseApplicationService.DeleteImageFromHouse(deleteImageCommand.HouseId, deleteImageCommand.ImagesList)).Wait(2000);
                    _serviceApplicationService.DeleteImagesFromService(deleteImageCommand.ServiceId,
                        deleteImageCommand.ImagesList);
                    return Ok();
                }
                else
                {
                    _logger.Error("Someone tried to upload an image for another user." +
                    " ServiceId:{0} | UserEmail:{1}", deleteImageCommand.ServiceId, userEmail);
                    return BadRequest("The logged in user is not the actual poster for the requested service." +
                                      " Action will be taken now against this user");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return InternalServerError(exception);
            }
        }
        
        [Route("service")]
        [HttpGet]
        public IHttpActionResult Get(string email = null, string location = null, 
            string serviceProfessionType = null, string serviceId = null, bool getCount = false, 
            bool getAllProfessionTypes = false, int pageNo = 1)
        {
            try
            {
                // The frontend starts with 1 as the first page, backend starts with 0. So we subtract 1 from whatever 
                // the page number is given.
                if (pageNo > 0)
                {
                    pageNo = pageNo - 1;
                }
                // If a record count for the services is requested, just provide it and return
                if (getCount)
                {
                    _logger.Info("Getting the Services record count in the database");
                    return Ok(_serviceApplicationService.GetServicesCount(serviceProfessionType, location, email));
                }

                _logger.Info("Get Service request received");
                // If both Location and ServiceProfessionType are provided
                if (location != null && serviceProfessionType != null)
                {
                    _logger.Info("Get Service by Area {0} and Property Type {1}", location, serviceProfessionType);
                    return Ok(_serviceApplicationService.SearchServicesByLocationAndProfession(
                            location, serviceProfessionType, pageNo));
                }
                // If only Location is provided
                else if (location != null)
                {
                    _logger.Info("Get Service by Location {0}", location);
                    return Ok(_serviceApplicationService.SearchServicesByLocation(location, pageNo));
                }
                // If only ServiceProfessionType is provided
                else if (serviceProfessionType != null)
                {
                    _logger.Info("Get Service by Property Type {0}", serviceProfessionType);
                    return Ok(_serviceApplicationService.SearchServicesByProfession(serviceProfessionType, pageNo));
                }
                // If only Email is provided
                else if (email != null)
                {
                    // Check is the request initiator is the actual poster of the Service
                    if (email.Equals(User.Identity.Name))
                    {
                        _logger.Info("Get Service by Email {0}", email);
                        return Ok(_serviceApplicationService.GetServicesByUploaderEmail(email, pageNo));
                    }
                    else
                    {
                        _logger.Error("Only user's own uploaded services can be retreived by email. CurrentUserEmail: {0}", email);
                        return BadRequest("Only user's own uploaded services can be retreived by email");
                    }
                }
                // If only ServiceId is provided
                else if (!string.IsNullOrEmpty(serviceId))
                {
                    _logger.Info("Get Service by ServiceId {0}", serviceId);
                    return Ok(_serviceApplicationService.GetServiceById(serviceId));
                }
                else if (getAllProfessionTypes)
                {
                    return Ok(_serviceApplicationService.GetAllServiceProfessionTypes());
                }
                // Otherwise if no criteria is provided, then return all the Services
                else
                {
                    return Ok(_serviceApplicationService.GetAllServices(pageNo));
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception.ToString());
            }
        }

        [Route("service/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var userEmail = User.Identity.Name;
                    if (_serviceApplicationService.ServiceOwnershipCheck(id, userEmail))
                    {
                        _serviceApplicationService.DeleteService(id);
                        return Ok();
                    }
                    else
                    {
                        _logger.Error("Current user is not allowed to delete this service. CurrentUser: {0} | ServiceId: {1}", userEmail, id);
                        return
                            BadRequest(
                                "Current user is not allowed to delete this service. Security breach, taking necessary action");
                    }
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
