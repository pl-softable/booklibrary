namespace BookLibrary.DataAccess.Models
{
    using Microsoft.AspNetCore.Identity;

    public class Librarian : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
    }
}