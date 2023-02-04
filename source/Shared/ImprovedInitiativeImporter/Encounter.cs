namespace EncounterTracker.Shared.ImprovedInitiativeImporter;

public class Encounter
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Id { get; set; }
    public IList<Combatant> Combatants { get; set; }
    public Encounter()
    {
    }

}