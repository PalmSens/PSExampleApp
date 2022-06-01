using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Saves a user based on username. Generates a id while saving
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<User> CreateUser(string name);

        /// <summary>
        /// Get all saved users
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAllUsers();

        /// <summary>
        /// Get all saved users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAllUsersAsync();

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