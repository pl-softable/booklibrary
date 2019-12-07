namespace BookLibrary.Core.Commands.Identity
{
    using System;

    public class AddPersonCommand : ICommand
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
    }
}