using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EncounterTracker.Shared.Base;
using EncounterTracker.Shared.ImprovedInitiativeImporter;
using Action = EncounterTracker.Shared.Base.Action;
using D20Check = EncounterTracker.Shared.Base.D20Check;

namespace EncounterTracker.Shared.FifthEdition
{
    public class Creature : CreatureBase
    {
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

        public Creature(Dnd5ECreature importedCreature)
        {
            Id = importedCreature.Id;
            Name = importedCreature.Name;
            Source = importedCreature.Source;
            Type = importedCreature.Type;
            Hp = importedCreature.Hp;
            Ac = importedCreature.Ac;
            InitiativeModifier = importedCreature.InitiativeModifier;
            InitiativeAdvantage = importedCreature.InitiativeAdvantage;
            Speed = importedCreature.Speed;
            Abilities = importedCreature.Abilities;
            DamageVulnerabilities = importedCreature.DamageVulnerabilities;
            DamageResistances = importedCreature.DamageResistances;
            DamageImmunities = importedCreature.DamageImmunities;
            ConditionImmunities = importedCreature.ConditionImmunities;
            Saves = importedCreature.Saves;
            Skills = importedCreature.Skills;
            Senses = importedCreature.Senses;
            Languages = importedCreature.Languages;
            Challenge = importedCreature.Challenge;
            Traits = importedCreature.Traits;
            Actions = importedCreature.Actions;
            BonusActions = importedCreature.BonusActions;
            Reactions = importedCreature.Reactions;
            LegendaryActions = importedCreature.LegendaryActions;
            MythicActions = importedCreature.MythicActions;
            Description = importedCreature.Description;
            ImageUrl = importedCreature.ImageUrl;
        }
    }
}
