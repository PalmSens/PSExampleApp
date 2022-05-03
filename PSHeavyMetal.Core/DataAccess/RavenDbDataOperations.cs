using PSHeavyMetal.Common.Models;
using Raven.Client.Documents;
using Raven.Embedded;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.DataAccess
{
    public class RavenDbDataOperations : IDataOperations
    {
        private IDocumentStore _documentStore;
        private const string DatabaseName = "RavenDb";

        public RavenDbDataOperations()
        {
            Init();
        }

        private void Init()
        {
            EmbeddedServer.Instance.StartServer();

            _documentStore = EmbeddedServer.Instance.GetDocumentStore(DatabaseName);
            _documentStore.Initialize();
        }

        public void OpenDb()
        {
            EmbeddedServer.Instance.OpenStudioInBrowser();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var entities = new List<T>();

            using (var session = _documentStore.OpenAsyncSession())
            {
                var query = session.Query<T>();

                await using (var enumerator = await session.Advanced.StreamAsync(query))
                {
                    while (await enumerator.MoveNextAsync())
                    {
                        entities.Add(enumerator.Current.Document);
                    }
                }
            }

            return entities;
        }

        public async Task SaveAsync<T>(T entity)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(entity);
                await session.SaveChangesAsync();
            }
        }

        public async Task<T> LoadAsyncAsync<T>(string id) where T : DataObject
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<T>(id);
            }
        }
    }
}