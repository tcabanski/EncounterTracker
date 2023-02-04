using EncounterTracker.Shared.Base;

namespace EncounterTracker.Shared.FifthEdition;

public class EncounterItem : EncounterItemBase
{
    public EncounterItem(ImprovedInitiativeImporter.Combatant importedCombatant)
    {
        if (importedCombatant.InitiativeGroup != null)
        {
            Id = importedCombatant.InitiativeGroup;
        }
        else
        {
            Id = importedCombatant.Id;
        }
            
        Combatants = new List<CombatantBase>
        {
            new Combatant(importedCombatant)
        };

    }

}