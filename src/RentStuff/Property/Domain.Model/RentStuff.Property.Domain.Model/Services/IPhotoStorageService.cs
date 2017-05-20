﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.Services
{
    /// <summary>
    /// Communicates with the Google Cloud Storage bucket where our photos are stored and uploads or deltes photos
    /// </summary>
    public interface IPhotoStorageService
    {
        /// <summary>
        /// Upload the photo to Google Cloud Storage
        /// </summary>
        /// <param name="fileNamem"></param>
        /// <param name="photoStream"></param>
        /// <returns></returns>
        void UploadPhoto(string fileNamem, Stream photoStream);

        /// <summary>
        /// Delte the phto from Google Cloud Storage
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        void DeletePhoto(string fileName);
    }
}
