namespace BookLibrary.DataAccess.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RegisteredPerson
    {
        [Key] 
        public Guid PersonId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
    }
}