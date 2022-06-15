using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudCustomers.API.Models;

namespace CloudCustomers.UnitTests.Fixtures
{
    public static class UsersFixture
    {
        public static List<User> GetTestUsers() => 
            new ()
            {
                new User
                {
                    Id = 1,
                    Name = "Test User 1",
                    Email = "test.user.1@test.com",
                    Address = new Address() {
                        Street = "123 Main St",
                        City = "Austin",
                        ZipCode = "78731"
                    }
                },
                new User
                {
                    Id = 1,
                    Name = "Test User 2",
                    Email = "test.user.2@test.com",
                    Address = new Address() {
                        Street = "459 Silver St",
                        City = "Austin",
                        ZipCode = "78732"
                    }
                },
                new User
                {
                    Id = 1,
                    Name = "Test User 3",
                    Email = "test.user.3@test.com",
                    Address = new Address() {
                        Street = "789 Capital St",
                        City = "Austin",
                        ZipCode = "78733"
                    }
                },
            };
    }
}