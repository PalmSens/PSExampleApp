using PSExampleApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSExampleApp.Core.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get all saved users
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetAllUsersAsync();

        /// <summary>
        /// Loads a user based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> LoadUserById(Guid id);

        /// <summary>
        /// Loads a user based in name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<User> LoadUserByName(string name);

        /// <summary>
        /// Updates the user saved in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUser(User user);
    }
}