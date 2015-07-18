
namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Specifies hte size of the house
    /// </summary>
    public class Size
    {
        private int length;
        private int width;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Size(int length, int width)
        {
            this.length = length;
            this.width = width;
        }

        public int Length
        {
            get { return length; }
        }

        public int Width
        {
            get { return width; }
        }
    }
}
