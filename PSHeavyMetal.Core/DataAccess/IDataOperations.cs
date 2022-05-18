using PSHeavyMetal.Common.Models;
using System;
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
        public IEnumerable<T> GetAll<T>() where T : DataObject;

        /// <summary>
        /// Loads all entities from the collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAllAsync<T>() where T : DataObject;

        /// <summary>
        /// Loads a entity based on Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> LoadByIdAsync<T>(Guid id) where T : DataObject;

        /// <summary>
        /// Loads a entity based on Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> LoadByNameAsync<T>(string name) where T : DataObject;

        /// <summary>
        /// Saves a enitity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToStore"></param>
        /// <returns></returns>
        public Task SaveAsync<T>(T entity) where T : DataObject;
    }
}