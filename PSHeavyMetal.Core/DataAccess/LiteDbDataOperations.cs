using LiteDB.Async;
using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.DataAccess
{
    public class LiteDbDataOperations : IDataOperations
    {
        private readonly string _connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamarinLiteDB.db");

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : DataObject
        {
            using var db = new LiteDatabaseAsync(_connectionString);

            var collection = db.GetCollection<T>();
            return await collection.Query().ToListAsync();
        }

        public async Task SaveAsync<T>(T entity) where T : DataObject
        {
            using var db = new LiteDatabaseAsync(_connectionString);

            var collection = db.GetCollection<T>();
            await collection.UpsertAsync(entity);
        }

        public async Task<T> LoadByIdAsync<T>(Guid id) where T : DataObject
        {
            using var db = new LiteDatabaseAsync(_connectionString);

            var collection = db.GetCollection<T>();
            return await collection.FindByIdAsync(id);
        }

        public async Task<T> LoadByNameAsync<T>(string name) where T : DataObject
        {
            using var db = new LiteDatabaseAsync(_connectionString);

            var collection = db.GetCollection<T>();
            return await collection.FindOneAsync(x => x.Name == name);
        }
    }
}