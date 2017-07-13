using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Ports.Adapter.Rest.DTOs
{
    public class ExternalLoginModels
    {
        public class ExternalLoginViewModel
        {
            public string Name { get; set; }

            public string Url { get; set; }

            public string State { get; set; }
        }
    }
}
