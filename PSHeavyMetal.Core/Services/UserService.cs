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
        private User _activeUser;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public event EventHandler<User> ActiveUserChanged;

        public User ActiveUser
        {
            get => _activeUser;
            private set
            {
                _activeUser = value;
                ActiveUserChanged?.Invoke(this, value);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> LoadUserAsync(Guid id)
        {
            var loadedUser = await _userRepository.LoadUserById(id);
            ActiveUser = loadedUser;
            return loadedUser;
        }

        public async Task SaveUserAsync(string username)
        {
            var user = await _userRepository.SaveUser(username);
            ActiveUser = user;
        }

        public void SetActiveUser(User user)
        {
            ActiveUser = user;
        }
    }
}