using EncounterTracker.Shared.Base;

namespace EncounterTracker.Shared.FifthEdition;

public class Encounter : EncounterBase
{
    public Encounter(ImprovedInitiativeImporter.Encounter importedEncounter) : base(importedEncounter) 
    {
        foreach (var combatant in importedEncounter.Combatants)
        {
            var groupItem = EncounterItems.FirstOrDefault(x => x.Id == combatant.InitiativeGroup);
            if (groupItem != null)
            {
                groupItem.Combatants.Add(new Combatant(combatant));
            }
            else
            {
                EncounterItems.Add(new EncounterItem(combatant));
            }
                
        }
    }
}