using PSExampleApp.Common.Models;
using PSExampleApp.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSExampleApp.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataOperations _dataOperations;

        public UserRepository(IDataOperations dataOperations)
        {
            _dataOperations = dataOperations;
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return _dataOperations.GetAllAsync<User>();
        }

        public Task<User> LoadUserById(Guid id)
        {
            return _dataOperations.LoadByIdAsync<User>(id);
        }

        public Task<User> LoadUserByName(string name)
        {
            return _dataOperations.LoadByNameAsync<User>(name);
        }

        public Task UpdateUser(User user)
        {
            return _dataOperations.SaveAsync(user);
        }
    }
}