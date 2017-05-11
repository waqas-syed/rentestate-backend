﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.UI.WebControls;
using Google.Cloud.Storage.V1;
using Newtonsoft.Json;
using NLog;
using RentStuff.Common;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;
using Image = System.Drawing.Image;

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
        private readonly StorageClient _storageClient;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="houseApplicationService"></param>
        public HouseController(IHouseApplicationService houseApplicationService)
        {
            _storageClient = StorageClient.Create();
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
                        IList<string> imagesList = new List<string>();
                        // Get the result of the task that was reading the multipart content
                        MultipartMemoryStreamProvider provider = imageSavetask.Result;
                        // For each file, do the processing
                        foreach (HttpContent content in provider.Contents)
                        {
                            using (Stream stream = content.ReadAsStreamAsync().Result)
                            {
                                using (var image = Image.FromStream(stream))
                                {
                                    // Create a name for this image
                                    string imageId = "IMG_" + Guid.NewGuid().ToString();
                                    // Add extension to the file name
                                    String fileName = imageId + ImageFormatProvider.GetImageExtension();
                                    // Resize the image to the size that we will be using as default
                                    var finalImage = ResizeImage(image, 830, 500);
                                    // Get the stream of the image
                                    var httpPostedStream = ToStream(finalImage);
                                    // Declare this image as Public once it will be uploaded in the Cloud Bucket
                                    var imageAcl = PredefinedObjectAcl.PublicRead;
                                    // Upload this image to Google Cloud Storage bucket
                                    var imageObject = _storageClient.UploadObjectAsync(
                                        bucket: ConfigurationManager.AppSettings["GoogleCloudStoragePhotoBucketName"],
                                        objectName: fileName,
                                        contentType: "image/jpeg",
                                        source: httpPostedStream,
                                        options: new UploadObjectOptions {PredefinedAcl = imageAcl}
                                    );
                                    imageObject.Wait(10000);
                                    // Get the url of the bucket and append with it the name of the file. This will be the public 
                                    // url for this image and ready to view
                                    fileName = ConfigurationManager.AppSettings["GoogleCloudStoragePhotoBucketUrl"] +
                                               fileName;
                                    // Add this image to the list of images received in this HTTP request
                                    imagesList.Add(fileName);
                                }
                            }
                        }
                        // Add images to the house and save it
                        _houseApplicationService.AddImagesToHouse(houseId, imagesList);
                        // Let the client know that the upload was successful
                        imageUploaded = true;
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

        public Stream ToStream(Image image)
        {
            // We support inly JPEG file format
            ImageCodecInfo encoder = GetEncoder(ImageFormatProvider.GetImageFormat());

            // Create an Encoder object based on the GUID  
            // for the Quality parameter category.  
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.  
            // An EncoderParameters object has an array of EncoderParameter  
            // objects. In this case, there is only one  
            // EncoderParameter object in the array.  
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            var stream = new System.IO.MemoryStream();
            //image.Save(stream, format);
            image.Save(stream, encoder, myEncoderParameters);
            stream.Position = 0;
            return stream;
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
                    _houseApplicationService.DeleteImageFromHouse(deleteImageCommand.HouseId,
                        deleteImageCommand.ImagesList);
                    return Ok();
                }
                else
                {
                    _logger.Error("Some tried to upload an image for another user. HouseId:{0} | UserEmail:{1}",
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
        public IHttpActionResult GetHouseCount(string propertyType = null, string location = null)
        {
            try
            {
                _logger.Info("Getting the house record count in the database");
                return Ok(_houseApplicationService.GetRecordsCount(propertyType, location));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return InternalServerError();
            }
        }

        [Route("house")]
        [HttpGet]
        public IHttpActionResult GetHouse(string email = null, string area = null, string propertyType = null, string houseId = null, int pageNo = 0)
        {
            try
            {
                _logger.Info("Get House request received");
                if (area != null && propertyType != null)
                {
                    _logger.Info("Get House by Area {0} and Property Type {1}", area, propertyType);
                    return Ok(_houseApplicationService.SearchHousesByAreaAndPropertyType(area, propertyType));
                }
                else if (area != null)
                {
                    _logger.Info("Get House by Area {0}", area);
                    return Ok(_houseApplicationService.SearchHousesByArea(area));
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
                            return Ok(_houseApplicationService.GetHouseByEmail(email));
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
                    return Ok(_houseApplicationService.GetAllHouses());
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception.ToString());
            }
        }
        
        [Route("GetHouseImages")]
        [HttpGet]
        [Obsolete]
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

        #region Private Methods

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        

        #endregion Private methods
    }
}
