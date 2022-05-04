using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsers();
        }

        public Task<User> LoadUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> SaveUserAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}