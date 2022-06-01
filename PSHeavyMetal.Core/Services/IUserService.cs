using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Triggers event when the active users changes. View models can listen to the event
        /// </summary>
        event EventHandler<User> ActiveUserChanged;

        /// <summary>
        /// The active user that is logged in
        /// </summary>
        User ActiveUser { get; }

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
        /// Saves the info of a measurement to the active user.
        /// </summary>
        /// <returns></returns>
        Task SaveMeasurementInfo(HeavyMetalMeasurement measurement);

        /// <summary>
        /// Saves a specific user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task SaveUserAsync(string username);

        /// <summary>
        /// Sets the user as active user
        /// </summary>
        /// <param name="user"></param>
        void SetActiveUser(User user);
    }
}