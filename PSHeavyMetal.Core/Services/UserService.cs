using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public class UserService : IUserService
    {
        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> LoadUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> SaveUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}