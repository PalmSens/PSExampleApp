using PSHeavyMetal.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public interface IAppConfigurationRepository
    {
        /// <summary>
        /// Loads the method saved in the database. This can be only one method
        /// </summary>
        /// <returns></returns>
        Task<List<MethodConfiguration>> LoadMethodAsync();

        /// <summary>
        /// Saves a method to the database. It first deletes the
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        Task SaveMethodAsync(MethodConfiguration method);
    }
}