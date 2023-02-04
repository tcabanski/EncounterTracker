namespace EncounterTracker.Shared.Base
{
    public class CombatantBase
    {
        public string Id { get; set; }

        public int CurrentHp { get; set; }

        public int TemporaryHp { get; set; }

        public CreatureBase Creature { get; set; }

        public CombatantBase(Shared.ImprovedInitiativeImporter.Combatant importedCombatant)
        {
            Id = importedCombatant.Id;
            CurrentHp = importedCombatant.CurrentHp;
            TemporaryHp = importedCombatant.TemporaryHp;
        }
    }
}