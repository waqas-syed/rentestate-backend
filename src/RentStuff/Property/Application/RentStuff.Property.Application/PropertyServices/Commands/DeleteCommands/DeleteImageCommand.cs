﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.PropertyServices.Commands.DeleteCommands
{
    /// <summary>
    /// To delete an image related to a house
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteImageCommand
    {
        public DeleteImageCommand(string houseId, IList<string> imagesList)
        {
            HouseId = houseId;
            ImagesList = imagesList;
        }

        [DataMember]
        public string HouseId { get; set; }
        [DataMember]
        public IList<string> ImagesList { get; set; }
    }
}
