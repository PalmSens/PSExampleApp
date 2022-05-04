using PSHeavyMetal.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.DataAccess
{
    public interface IDataOperations
    {
        /// <summary>
        /// Loads all entities from the collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAllAsync<T>();

        /// <summary>
        /// Saves a enitity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToStore"></param>
        /// <returns></returns>
        public Task SaveAsync<T>(T entity);

        /// <summary>
        /// Loads a entity based on Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> LoadAsync<T>(string id) where T : DataObject;

        /// <summary>
        /// For testing purposes. Should only be used for unit testing!
        /// </summary>

        public void OpenDb();
    }
}