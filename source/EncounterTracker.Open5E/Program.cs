// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography.X509Certificates;
using Autofac;
using EncounterTracker.Shared.FifthEdition;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Session;
using Encounter = EncounterTracker.Shared.FifthEdition.Encounter;

var container = ConfigureContainer();

dynamic importData;
if (args.Length < 2)
{
    Console.WriteLine("Usage is: EncounterTracker.Open5e <source> <file>");
    return;
}

string source = args[0];
string fileName = args[1];

Console.WriteLine($"Importing file {fileName} from {source}");

using (StreamReader r = new StreamReader(fileName))
{
    string json = r.ReadToEnd();
    importData = JsonConvert.DeserializeObject(json);
}

int creatureCount = 0;

using (BulkInsertOperation bulkInsert = container.Resolve<IDocumentStore>().BulkInsert())
{
    foreach (dynamic entry in importData)
    {
        EncounterTracker.Shared.Open5E.Creature importedCreature =
            JsonConvert.DeserializeObject<EncounterTracker.Shared.Open5E.Creature>(entry.ToString());
        importedCreature.Source = source;

        Creature creature = new Creature(importedCreature, true);
        bulkInsert.Store(creature);
        creatureCount++;
        Console.WriteLine($"Processed creature {creature.Id} with name {creature.Name} ");
    }
}

Console.WriteLine($"Imported {creatureCount} creatures");

IContainer ConfigureContainer()
{
    var builder = new ContainerBuilder();
    builder.RegisterAssemblyModules(typeof(EncounterTracker.Data.Registrar).Assembly);

    return builder.Build();
}
