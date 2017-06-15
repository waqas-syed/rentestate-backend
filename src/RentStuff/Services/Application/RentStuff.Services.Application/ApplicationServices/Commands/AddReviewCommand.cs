
namespace RentStuff.Services.Application.ApplicationServices.Commands
{
    /// <summary>
    /// Command to add a Review
    /// </summary>
    public class AddReviewCommand : IServiceCommand
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public AddReviewCommand(string authorName, string authorEmail, string reviewDescription, string serviceId)
        {
            AuthorName = authorName;
            AuthorEmail = authorEmail;
            ReviewDescription = reviewDescription;
            ServiceId = serviceId;
        }

        public string AuthorName { get; private set; }
        public string AuthorEmail { get; private set; }
        public string ReviewDescription { get; private set; }
        public string ServiceId { get; private set; }
    }
}
