using LiteDB.Async;
using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.DataAccess
{
    public class LiteDbDataOperations : IDataOperations
    {
        private readonly string _connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamarinLiteDB.db");

        private readonly ILiteDatabaseAsync _liteDatabaseAsync;

        public LiteDbDataOperations()
        {
            _liteDatabaseAsync = new LiteDatabaseAsync(_connectionString);
        }

        public Task DeleteAll<T>() where T : DataObject
        {
            var collection = _liteDatabaseAsync.GetCollection<T>();
            return collection.DeleteAllAsync();
        }

        public Task DeleteByIdAsync<T>(Guid id) where T : DataObject
        {
            try
            {
                var collection = _liteDatabaseAsync.GetCollection<T>();
                return collection.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public Task<List<T>> GetAllAsync<T>() where T : DataObject
        {
            try
            {
                var collection = _liteDatabaseAsync.GetCollection<T>();
                return collection.Query().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public Task<T> LoadByIdAsync<T>(Guid id) where T : DataObject
        {
            try
            {
                var collection = _liteDatabaseAsync.GetCollection<T>();
                return collection.FindByIdAsync(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public Task<T> LoadByNameAsync<T>(string name) where T : DataObject
        {
            try
            {
                var collection = _liteDatabaseAsync.GetCollection<T>();
                return collection.FindOneAsync(x => x.Name == name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public Task SaveAsync<T>(T entity) where T : DataObject
        {
            try
            {
                var collection = _liteDatabaseAsync.GetCollection<T>();
                return collection.UpsertAsync(entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}