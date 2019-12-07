namespace BookLibrary.Core.Models
{
    using System;

    public class Person
    {
        public Person(Guid personId, string firstName, string lastName, string ssn)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            SSN = ssn;
        }

        public Guid PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
    }
}