using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace RentStuff.Common
{
    /// <summary>
    ///  Provides the type of image format or type supported by our application
    /// </summary>
    public class ImageFormatProvider
    {
        /// <summary>
        /// Returns the corresponding string depending on the raw format of the image
        /// </summary>
        /// <returns></returns>
        public static string GetImageExtension()
        {
            // We use only jpg extension
            return ".jpg";
        }

        public static ImageFormat GetImageFormat()
        {
            // We use Only JPEG Format
            return System.Drawing.Imaging.ImageFormat.Jpeg;
        }
    }
}
