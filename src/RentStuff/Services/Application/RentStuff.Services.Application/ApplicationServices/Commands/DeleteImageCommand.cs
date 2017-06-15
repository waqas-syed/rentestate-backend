using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Services.Application.ApplicationServices.Commands
{
    /// <summary>
    /// To delete an image related to a house
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteImageCommand
    {
        public DeleteImageCommand(string serviceId, IList<string> imagesList)
        {
            ServiceId = serviceId;
            ImagesList = imagesList;
        }

        [DataMember]
        public string ServiceId { get; set; }
        [DataMember]
        public IList<string> ImagesList { get; set; }
    }
}
