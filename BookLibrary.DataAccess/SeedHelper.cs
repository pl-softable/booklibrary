namespace BookLibrary.DataAccess
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using Models;

    public class SeedHelper
    {
        public static IList<Librarian> SeedLibrarians()
        {
            var passwordHasher = new PasswordHasher<Librarian>();

            var users = new List<Librarian>
            {
                new Librarian
                {
                    Id = 1,
                    UserName = "librarian@library.co.uk",
                    NormalizedUserName = "librarian@library.co.uk",
                    Email = "librarian@library.co.uk",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new Librarian
                {
                    Id = 2,
                    UserName = "seniorlibrarian@library.co.uk",
                    NormalizedUserName = "seniorlibrarian@library.co.uk",
                    Email = "seniorlibrarian@library.co.uk",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            };

            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "Librarian123###");
            users[1].PasswordHash = passwordHasher.HashPassword(users[1], "SeniorLibrarian123###");

            return users;
        }

        public static IList<IdentityRole<int>> SeedRoles()
        {
            var roles = new List<IdentityRole<int>>
            {
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = "SeniorLibrarian",
                    NormalizedName = "SeniorLibrarian"
                },
                new IdentityRole<int>
                {
                    Id = 2,
                    Name = "Librarian",
                    NormalizedName = "Librarian"
                }
            };

            return roles;
        }
    }
}