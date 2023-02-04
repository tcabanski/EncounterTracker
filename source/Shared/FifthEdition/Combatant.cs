using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterTracker.Shared.Base;

namespace EncounterTracker.Shared.FifthEdition
{
    public class Combatant : CombatantBase
    {
        public Combatant(Shared.ImprovedInitiativeImporter.Combatant importedCombatant) : base(importedCombatant)
        {
            Creature = new Creature(importedCombatant.StatBlock);
        } 

    }
}
