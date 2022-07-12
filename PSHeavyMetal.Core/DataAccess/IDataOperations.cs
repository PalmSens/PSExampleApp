namespace PSHeavyMetal.Core.DataAccess
{
    using PSHeavyMetal.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataOperations
    {
        /// <summary>
        /// Clears a whole collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task DeleteAll<T>() where T : DataObject;

        /// <summary>
        /// Delete a element in the collection, based on ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public Task DeleteByIdAsync<T>(Guid id) where T : DataObject;

        /// <summary>
        /// Loads all entities from the collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <returns></returns>
        public Task<List<T>> GetAllAsync<T>() where T : DataObject;

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