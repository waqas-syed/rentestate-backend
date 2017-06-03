
namespace RentStuff.Services.Domain.Model.ServiceAggregate
{
    /// <summary>
    /// User review about a particular service
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Review(string authorname, string authorEmail, string reviewDescription)
        {
            Authorname = authorname;
            AuthorEmail = authorEmail;
            ReviewDescription = reviewDescription;
        }

        public string Authorname { get; set; }
        public string AuthorEmail { get; set; }
        public string ReviewDescription { get; set; }
    }
}
