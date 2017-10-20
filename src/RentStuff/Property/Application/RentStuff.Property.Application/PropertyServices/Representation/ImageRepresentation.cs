using System;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.PropertyServices.Representation
{
    /// <summary>
    /// Represents an image associated with a house
    /// </summary>
    [Serializable]
    [DataContract]
    public class ImageRepresentation
    {
        public ImageRepresentation(string name, string type, string base64String)
        {
            Name = name;
            Type = type;
            Base64String = base64String;
        }

        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Type { get; private set; }
        [DataMember]
        public string Base64String { get; private set; }
    }
}
