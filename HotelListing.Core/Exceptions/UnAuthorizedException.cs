namespace HotelListing.Exceptions
{
    public class UnAuthorizedException:ApplicationException
    {
        public UnAuthorizedException(string name):base($"Attempted unauthorized access in {name}")
        {
            
        }
    }
}
