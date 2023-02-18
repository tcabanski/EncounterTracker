using EncounterTracker.Shared.Base;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;

namespace EncounterTracker.Data
{
    public class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateDocumentStore);

        private static IDocumentStore CreateDocumentStore()
        {
            string serverUrl = "http://localhost:8080";
            string databaseName = "EncounterBuilder";

            IDocumentStore documentStore = new DocumentStore
            {
                Urls = new[] { serverUrl },
                Database = databaseName
            };

            documentStore.Conventions.FindCollectionName = type =>
            {
                if (typeof(CreatureBase).IsAssignableFrom(type))
                    return "Creatures";

                return DocumentConventions.DefaultGetCollectionName(type);
            };

            documentStore.Initialize();
            
            return documentStore;
        }

        public static IDocumentStore Store
        {
            get { return store.Value; }
        }
    }
}