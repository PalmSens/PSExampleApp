using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAllUsers();

        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Loads a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> LoadUserAsync(Guid id);

        /// <summary>
        /// Saves a specific user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task SaveUserAsync(string username);
    }
}