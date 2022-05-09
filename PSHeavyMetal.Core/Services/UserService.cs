using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public Task<User> LoadUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveUserAsync(string username)
        {
            await _userRepository.SaveUser(username);
        }
    }
}