using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public string DisplayName { get; set; }
    }

    public class AppRole : IdentityRole<int>
    {

    }
}
