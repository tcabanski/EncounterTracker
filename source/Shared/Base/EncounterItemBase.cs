namespace EncounterTracker.Shared.Base;

public class EncounterItemBase
{
    public string Id { get; set; }
    public IList<CombatantBase> Combatants { get; set; }
    public int Initiative { get; set; }
    public int InitiativeTieBreaker { get; set; }
    public string Alias { get; set; }
}