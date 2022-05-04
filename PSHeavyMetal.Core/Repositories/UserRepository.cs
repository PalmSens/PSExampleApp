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

        public Task<User> LoadUser(string id)
        {
            return _dataOperations.LoadAsync<User>(id);
        }

        public Task SaveUser(string username, string password)
        {
            return _dataOperations.SaveAsync(new User { Name = username, Password = password, Id = Guid.NewGuid().ToString() });
        }

        public Task<IEnumerable<User>> GetAllUsers()
        {
            return _dataOperations.GetAllAsync<User>();
        }
    }
}