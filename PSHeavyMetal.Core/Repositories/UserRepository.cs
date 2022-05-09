using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataOperations _dataOperations;

        public UserRepository(IDataOperations dataOperations)
        {
            _dataOperations = dataOperations;
        }

        public Task<User> LoadUserById(Guid id)
        {
            return _dataOperations.LoadByIdAsync<User>(id);
        }

        public Task<User> LoadUserByName(string name)
        {
            return _dataOperations.LoadByNameAsync<User>(name);
        }

        public Task SaveUser(string username)
        {
            return _dataOperations.SaveAsync(new User { Name = username, Password = "123", Id = Guid.NewGuid() });
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _dataOperations.GetAllAsync<User>();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _dataOperations.GetAll<User>();
        }
    }
}