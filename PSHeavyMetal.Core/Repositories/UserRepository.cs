using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> LoadUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> SaveUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<User>> IUserRepository.GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}