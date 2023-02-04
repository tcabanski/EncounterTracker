using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterTracker.Shared.Base;
using FluentAssertions;

namespace EncounterTracker.Tests
{
    public class EncounterBaseTests
    {
        private EncounterBase CreateEncounter()
        {
            var encounter = new EncounterBase();
            encounter.EncounterItems = new List<EncounterItemBase>();
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "1", Initiative = 10, InitiativeTieBreaker = 1 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "2", Initiative = 10, InitiativeTieBreaker = 2 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "3", Initiative = 9, InitiativeTieBreaker = 1 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "4", Initiative = 9, InitiativeTieBreaker = 2 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "5", Initiative = 9, InitiativeTieBreaker = 3 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "6", Initiative = 9, InitiativeTieBreaker = 4 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "7", Initiative = 9, InitiativeTieBreaker = 5 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "8", Initiative = 9, InitiativeTieBreaker = 6 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "9", Initiative = 9, InitiativeTieBreaker = 7 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "10", Initiative = 9, InitiativeTieBreaker = 8 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "11", Initiative = 9, InitiativeTieBreaker = 9 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "12", Initiative = 9, InitiativeTieBreaker = 10 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "13", Initiative = 9, InitiativeTieBreaker = 11 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "14", Initiative = 9, InitiativeTieBreaker = 12 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "15", Initiative = 9, InitiativeTieBreaker = 13 });
            encounter.EncounterItems.Add(new EncounterItemBase() { Id = "16", Initiative = 9, InitiativeTieBreaker = 14 });

            encounter.Sort();

            return encounter;
        }
        
        [Test]
        public void SortsIntoCorrectOrder()
        {
            var encounter = CreateEncounter();

            encounter.EncounterItems[0].Initiative.Should().Be(10);
            encounter.EncounterItems[0].InitiativeTieBreaker.Should().Be(2);
            encounter.EncounterItems[1].Initiative.Should().Be(10);
            encounter.EncounterItems[1].InitiativeTieBreaker.Should().Be(1);
            encounter.EncounterItems[2].Initiative.Should().Be(9);
            encounter.EncounterItems[2].InitiativeTieBreaker.Should().Be(14);
        }

        [Test]
        public void NextGoesToNextItemInRound()
        {
            var encounter = CreateEncounter();
            encounter.Start();
            encounter.Next();
            encounter.EncounterItems[1].Id.Should().Be(encounter.CurrentEncounterItem.Id);
            encounter.Round.Should().Be(0);
        }
        
        [Test]
        public void NextFromLastItemGoesToFirstItemNextRound()
        {
            var encounter = CreateEncounter();
            encounter.Start();
            
            // call next on every item
            for (int i = 0; i < encounter.EncounterItems.Count; i++)
            {
                encounter.Next();
            }
            encounter.EncounterItems[0].Id.Should().Be(encounter.CurrentEncounterItem.Id);
            encounter.Round.Should().Be(1);
        }

        [Test]
        public void PrevFromFirstItemFirstRoundDoesNothing()
        {
            var encounter = CreateEncounter();
            encounter.Start();
            encounter.Previous();
            encounter.EncounterItems[0].Id.Should().Be(encounter.CurrentEncounterItem.Id);
            encounter.Round.Should().Be(0);
        }

        [Test]
        public void PrevFromNthItemInRoundGoesToPrev()
        {
            var encounter = CreateEncounter();
            encounter.Start();
            //go to the last item in the list
            for (int i = 0; i < encounter.EncounterItems.Count - 1; i++)
            {
                encounter.Next();
            }
            encounter.Previous();
            encounter.EncounterItems[encounter.EncounterItems.Count - 2].Id.Should().Be(encounter.CurrentEncounterItem.Id);
            encounter.Round.Should().Be(0);
        }
        [Test]
        public void PrevFromFirstItemInRoundPast0GoesToLastItemPrevRound()
        {
            var encounter = CreateEncounter();
            encounter.Start();
            // call next on every item to jump to the next round
            for (int i = 0; i < encounter.EncounterItems.Count; i++)
            {
                encounter.Next();
            }
            encounter.Previous();
            encounter.EncounterItems[encounter.EncounterItems.Count - 1].Id.Should().Be(encounter.CurrentEncounterItem.Id);
            encounter.Round.Should().Be(0);
        }

        [Test]
        public void EndResets()
        {
            var encounter = CreateEncounter();
            encounter.Start();
            encounter.End();
            encounter.CurrentEncounterItem.Should().BeNull();
            encounter.Round.Should().Be(0);
        }
    }

}
