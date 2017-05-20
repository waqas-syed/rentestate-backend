using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using RentStuff.Property.Domain.Model.Services;

namespace RentStuff.Property.Infrastructure.Services.GoogleStorageServices
{
    /// <summary>
    /// Communicates with the Google Cloud Storage bucket where our photos are stored and uploads or deletes photos
    /// </summary>
    public class PhotoStorageService : IPhotoStorageService
    {
        private readonly StorageClient _storageClient;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public PhotoStorageService()
        {
            _storageClient = StorageClient.Create();
        }

        /// <summary>
        /// Upload the photo to Google Cloud Storage
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="photoStream"></param>
        /// <returns></returns>
        public void UploadPhoto(string fileName, Stream photoStream)
        {
            // Declare this image as Public once it will be uploaded in the Cloud Bucket
            var imageAcl = PredefinedObjectAcl.PublicRead;
            // Upload this image to Google Cloud Storage bucket
            _storageClient.UploadObject(
                bucket: ConfigurationManager.AppSettings["GoogleCloudStoragePhotoBucketName"],
                objectName: fileName,
                contentType: "image/jpeg",
                source: photoStream,
                options: new UploadObjectOptions { PredefinedAcl = imageAcl }
            );
        }

        /// <summary>
        /// Delte the phto from Google Cloud Storage
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void DeletePhoto(string fileName)
        {
            // Parse the object name out of the link
            var lastIndexOfSlash = fileName.LastIndexOf("/", StringComparison.CurrentCulture);
            var objectName = fileName.Substring(lastIndexOfSlash + 1, (fileName.Length) - (lastIndexOfSlash + 1));
            _storageClient.DeleteObject(
                bucket: ConfigurationManager.AppSettings["GoogleCloudStoragePhotoBucketName"],
                objectName: objectName
            );
        }
    }
}
