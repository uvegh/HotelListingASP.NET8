using Microsoft.AspNetCore.Identity;

namespace HotelListing.Data.Entities
{
    public class ApiUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
