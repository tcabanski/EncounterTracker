using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterTracker.Shared.Open5E
{
    public class Creature
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string Speed { get; set; }
        public string Type { get; set; }
        public string Alignment { get; set; }
        public int ArmorClass { get; set; }
        public int HitPoints { get; set; }
        public string HitDice { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int? Constitution_save { get; set; }
        public int? Intelligence_save { get; set; }
        public int? Wisdom_save { get; set; }
        public int? Dexterity_save { get; set; }
        public int? Charisma_save { get; set; }
        public int? Strength_save { get; set; }
        public int? Acrobatics { get; set; }
        public int? Animal_handling { get; set; }
        public int? Arcana { get; set; }
        public int? Athletics { get; set; }
        public int? Deception { get; set; }
        public int? History { get; set; }
        public int? Insight { get; set; }
        public int? Intimidation { get; set; }
        public int? Investigation { get; set; }
        public int? Medicine { get; set; }
        public int? Nature { get; set; }
        public int? Perception { get; set; }
        public int? Performance { get; set; }
        public int? Persuasion { get; set; }
        public int? Religion { get; set; }
        public int? Sleight_of_hand { get; set; }
        public int? Stealth { get; set; }
        public int? Survival { get; set; }
        public string Damage_vulnerabilities { get; set; }
        public string Damage_resistances { get; set; }
        public string Damage_immunities { get; set; }
        public string Condition_immunities { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Challenge_rating { get; set; }
        public List<Ability> Special_abilities { get; set; }
        public List<Ability> Actions { get; set; }

        public List<Ability> Reactions { get; set; }
        public List<Ability> Bonus_actions { get; set; }
        public List<Ability> Legendary_actions { get; set; }
        public List<Ability> Mythic_actions { get; set; }
    }

    public struct Ability
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        
    }
}
