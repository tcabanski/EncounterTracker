using System.Text.Json.Serialization;
using EncounterTracker.Shared.Base;
using Action = EncounterTracker.Shared.Base.Action;

namespace EncounterTracker.Shared.ImprovedInitiativeImporter;

public class Dnd5ECreature : CreatureBase
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Source { get; set; }
    public string Type { get; set; }

    [JsonPropertyName("HP")]
    public Hp Hp { get; set; }
    [JsonPropertyName("AC")]
    public Ac Ac { get; set; }
    public int InitiativeModifier { get; set; }
    public bool InitiativeAdvantage { get; set; }
    public List<string> Speed { get; set; }
    public Abilities Abilities { get; set; }
    public List<string> DamageVulnerabilities { get; set; }
    public List<string> DamageResistances { get; set; }
    public List<string> DamageImmunities { get; set; }
    public List<string> ConditionImmunities { get; set; }
    public List<D20Check> Saves { get; set; }
    public List<D20Check> Skills { get; set; }
    public List<string> Senses { get; set; }
    public List<string> Languages { get; set; }
    public string Challenge { get; set; }
    public List<Trait> Traits { get; set; }
    public List<Action> Actions { get; set; }
    public List<Action> BonusActions { get; set; }
    public List<Action> Reactions { get; set; }
    public List<Action> LegendaryActions { get; set; }
    public List<Action> MythicActions { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}