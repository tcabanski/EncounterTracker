// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography.X509Certificates;
using Autofac;
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

Console.WriteLine($"Importing file {args[1]} from {args[0]}");

using (StreamReader r = new StreamReader(args[1]))
{
    string json = r.ReadToEnd();
    importData = JsonConvert.DeserializeObject(json);
}

int creatureCount = 0;
int encounterCount = 0;

using (BulkInsertOperation bulkInsert = container.Resolve<IDocumentStore>().BulkInsert())
{
    foreach (dynamic entry in importData)
    {
        EncounterTracker.Shared.Open5E.Creature importedCreature =
                JsonConvert.DeserializeObject<EncounterTracker.Shared.Open5E.Creature>(entry.ToString());



        //if (entry.Name.StartsWith("Creatures."))
        //{
        //    Dnd5ECreature importedCreature = JsonConvert.DeserializeObject<Dnd5ECreature>(entry.Value.ToString());
        //    Creature creature = new EncounterTracker.Shared.FifthEdition.Creature(importedCreature);
        //    bulkInsert.Store(creature);
        //    creatureCount++;
        //    Console.WriteLine($"Processed creature {creature.Id} with name {creature.Name} ");
        //}

        //if (entry.Name.StartsWith("SavedEncounters."))
        //{
        //    Encounter importedEncounter = JsonConvert.DeserializeObject<Encounter>(entry.Value.ToString());
        //    var encounter = new EncounterTracker.Shared.FifthEdition.Encounter(importedEncounter);
        //    bulkInsert.Store(encounter);
        //    encounterCount++;
        //    Console.WriteLine($"Processed encounter {encounter.Id} with name {encounter.Path}/{encounter.Name} ");

        //}
    }
}

Console.WriteLine($"Imported {creatureCount} creatures");
Console.WriteLine($"Imported {encounterCount} encounters");

IContainer ConfigureContainer()
{
    var builder = new ContainerBuilder();
    builder.RegisterAssemblyModules(typeof(EncounterTracker.Data.Registrar).Assembly);

    return builder.Build();
}
