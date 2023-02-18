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

        public Creature(Open5E.Creature importedCreature, string source)
        {
            Name = importedCreature.Name;
            Source = source;
            Type = importedCreature.Type;
            Hp = new Hp
            {
                Value = importedCreature.HitPoints,
                Notes = importedCreature.HitDice
            };
            Ac = new Ac
            {
                Value = importedCreature.ArmorClass
            };
            InitiativeModifier = 0;
            InitiativeAdvantage = false;
            Speed = new List<string>
            {
                $"{importedCreature.Speed}"
            };
            Abilities = new Abilities
            {
                Str = importedCreature.Strength,
                Dex = importedCreature.Dexterity,
                Con = importedCreature.Constitution,
                Int = importedCreature.Intelligence,
                Wis = importedCreature.Wisdom,
                Cha = importedCreature.Charisma
            };
            DamageVulnerabilities = new List<string>
            {
                $"{importedCreature.Damage_vulnerabilities}"
            };
            DamageResistances = new List<string>
            {
                $"{importedCreature.Damage_resistances}"
            };
            DamageImmunities = new List<string>
            {
                $"{importedCreature.Damage_immunities}"
            };
            ConditionImmunities = new List<string>
            {
                $"{importedCreature.Condition_immunities}"
            };
            SetSavesFromOpen5e(importedCreature);
            SetSkillsFromOpen5e(importedCreature);

            Senses = new List<string>
            {
                $"{importedCreature.Senses}"
            };
            Languages = new List<string>
            {
                $"{importedCreature.Languages}"
            };
            Challenge = importedCreature.Challenge_rating;
            
            Traits = new List<Trait>();
            if (importedCreature.Special_abilities != null)
            {
                foreach (var specialAbility in importedCreature.Special_abilities)
                {
                    Traits.Add(new Trait
                    {
                        Name = specialAbility.Name,
                        Content = specialAbility.Desc
                    });
                }
            }
            Actions = new List<Action>();
            if (importedCreature.Actions != null)
            {
                foreach (var action in importedCreature.Actions)
                {
                    Actions.Add(new Action
                    {
                        Name = action.Name,
                        Content = action.Desc
                    });
                }
            }
            BonusActions = new List<Action>();
            if (importedCreature.Bonus_actions != null)
            {
                foreach (var action in importedCreature.Bonus_actions)
                {
                    BonusActions.Add(new Action
                    {
                        Name = action.Name,
                        Content = action.Desc
                    });
                }
            }
            Reactions = new List<Action>();
            if (importedCreature.Reactions != null)
            {
                foreach (var action in importedCreature.Reactions)
                {
                    Reactions.Add(new Action
                    {
                        Name = action.Name,
                        Content = action.Desc
                    });
                }
            }
            LegendaryActions = new List<Action>();
            if (importedCreature.Legendary_actions != null)
            {
                foreach (var action in importedCreature.Legendary_actions)
                {
                    LegendaryActions.Add(new Action
                    {
                        Name = action.Name,
                        Content = action.Desc
                    });
                }
            }
            MythicActions = new List<Action>();
            if (importedCreature.Mythic_actions != null)
            {
                foreach (var action in importedCreature.Mythic_actions)
                {
                    MythicActions.Add(new Action
                    {
                        Name = action.Name,
                        Content = action.Desc
                    });
                }
            }
        }

        private void SetSavesFromOpen5e(Open5E.Creature importedCreature)
        {
            if (importedCreature.Strength_save != null ||
                importedCreature.Dexterity_save != null ||
                importedCreature.Constitution_save != null ||
                importedCreature.Intelligence_save != null ||
                importedCreature.Wisdom_save != null ||
                importedCreature.Charisma_save != null)
            {
                Saves = new List<D20Check>();
            }

            if (importedCreature.Strength_save != null)
            {
                Saves.Add(new D20Check
                {
                    Name = "STR",
                    Modifier = importedCreature.Strength_save.Value
                });
            }

            if (importedCreature.Constitution_save != null)
            {
                Saves.Add(new D20Check
                {
                    Name = "CON",
                    Modifier = importedCreature.Constitution_save.Value
                });
            }

            if (importedCreature.Dexterity_save != null)
            {
                Saves.Add(new D20Check
                {
                    Name = "DEX",
                    Modifier = importedCreature.Dexterity_save.Value
                });
            }

            if (importedCreature.Intelligence_save != null)
            {
                Saves.Add(new D20Check
                {
                    Name = "INT",
                    Modifier = importedCreature.Intelligence_save.Value
                });
            }

            if (importedCreature.Wisdom_save != null)
            {
                Saves.Add(new D20Check
                {
                    Name = "WIS",
                    Modifier = importedCreature.Wisdom_save.Value
                });
            }

            if (importedCreature.Charisma_save != null)
            {
                Saves.Add(new D20Check
                {
                    Name = "CHA",
                    Modifier = importedCreature.Charisma_save.Value
                });
            }
        }

        private void SetSkillsFromOpen5e(Open5E.Creature importedCreature)
        {
            if (importedCreature.Acrobatics != null ||
                importedCreature.Animal_handling != null ||
                importedCreature.Arcana != null ||
                importedCreature.Athletics != null ||
                importedCreature.Deception != null ||
                importedCreature.History != null ||
                importedCreature.Insight != null ||
                importedCreature.Intimidation != null ||
                importedCreature.Investigation != null ||
                importedCreature.Medicine != null ||
                importedCreature.Nature != null ||
                importedCreature.Perception != null ||
                importedCreature.Performance != null ||
                importedCreature.Persuasion != null ||
                importedCreature.Religion != null ||
                importedCreature.Sleight_of_hand != null ||
                importedCreature.Stealth != null ||
                importedCreature.Survival != null)
            {
                Skills = new List<D20Check>();
            }

            if (importedCreature.Acrobatics != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Acrobatics",
                    Modifier = importedCreature.Acrobatics.Value
                });
            }
            if (importedCreature.Animal_handling != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Animal Handling",
                    Modifier = importedCreature.Animal_handling.Value
                });
            }
            if (importedCreature.Arcana != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Arcana",
                    Modifier = importedCreature.Arcana.Value
                });
            }
            if (importedCreature.Athletics != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Athletics",
                    Modifier = importedCreature.Athletics.Value
                });
            }
            if (importedCreature.Deception != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Deception",
                    Modifier = importedCreature.Deception.Value
                });
            }
            if (importedCreature.History != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "History",
                    Modifier = importedCreature.History.Value
                });
            }
            if (importedCreature.Insight != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Insight",
                    Modifier = importedCreature.Insight.Value
                });
            }
            if (importedCreature.Intimidation != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Intimidation",
                    Modifier = importedCreature.Intimidation.Value
                });
            }
            if (importedCreature.Investigation != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Investigation",
                    Modifier = importedCreature.Investigation.Value
                });
            }
            if (importedCreature.Medicine != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Medicine",
                    Modifier = importedCreature.Medicine.Value
                });
            }
            if (importedCreature.Nature != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Nature",
                    Modifier = importedCreature.Nature.Value
                });
            }
            if (importedCreature.Perception != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Perception",
                    Modifier = importedCreature.Perception.Value
                });
            }
            if (importedCreature.Performance != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Performance",
                    Modifier = importedCreature.Performance.Value
                });
            }
            if (importedCreature.Persuasion != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Persuasion",
                    Modifier = importedCreature.Persuasion.Value
                });
            }
            if (importedCreature.Religion != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Religion",
                    Modifier = importedCreature.Religion.Value
                });
            }
            if (importedCreature.Sleight_of_hand != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Sleight of Hand",
                    Modifier = importedCreature.Sleight_of_hand.Value
                });
            }
            if (importedCreature.Stealth != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Stealth",
                    Modifier = importedCreature.Stealth.Value
                });
            }
            if (importedCreature.Survival != null)
            {
                Skills.Add(new D20Check
                {
                    Name = "Survival",
                    Modifier = importedCreature.Survival.Value
                });
            }
        }
    }
}

