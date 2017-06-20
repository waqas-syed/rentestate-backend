using System;

namespace RentStuff.Services.Domain.Model.ServiceAggregate
{
    /// <summary>
    /// User review about a particular service
    /// </summary>
    public class Review
    {
        private string _id = Guid.NewGuid().ToString();

        public Review()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Review(string authorname, string authorEmail, string reviewDescription, Service service)
        {
            AuthorName = authorname;
            AuthorEmail = authorEmail;
            ReviewDescription = reviewDescription;
            Service = service;
        }

        public virtual string Id { get { return _id; } private set { _id = value; } }

        public string AuthorName { get; private set; }
        public virtual string AuthorEmail { get; protected internal set; }
        public virtual string ReviewDescription { get; protected internal set; }
        public virtual Service Service { get; protected internal set; }
    }
}
