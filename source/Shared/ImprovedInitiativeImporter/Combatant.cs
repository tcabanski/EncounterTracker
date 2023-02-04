namespace EncounterTracker.Shared.ImprovedInitiativeImporter
{
    public class Combatant
    {
        public string Id { get; set; }
        public Dnd5ECreature StatBlock { get; set; }

        public int CurrentHp { get; set; }

        public int TemporaryHp { get; set; }

        public int Initiative { get; set; }
        public string InitiativeGroup { get; set; }

        public string Alias { get; set; }

        public string CurrentNotes { get; set; }

        public Combatant()
        {
        }
    }
}