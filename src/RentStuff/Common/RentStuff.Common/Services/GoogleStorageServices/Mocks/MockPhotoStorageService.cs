using System.IO;

namespace RentStuff.Common.Services.GoogleStorageServices.Mocks
{
    /// <summary>
    /// Mock for the Photo Storage Service
    /// </summary>
    public class MockPhotoStorageService : IPhotoStorageService
    {
        /// <summary>
        /// Upload the photo to Google Cloud Storage
        /// </summary>
        /// <param name="fileNamem"></param>
        /// <param name="photoStream"></param>
        /// <returns></returns>
        public void UploadPhoto(string fileNamem, Stream photoStream)
        {
            // Just return, do nothing
        }

        /// <summary>
        /// Delte the phto from Google Cloud Storage
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void DeletePhoto(string fileName)
        {
            // Just return, do nothing
        }
    }
}
