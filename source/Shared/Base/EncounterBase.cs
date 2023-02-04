namespace EncounterTracker.Shared.Base;

using System.Collections.Generic;

public class EncounterBase
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Id { get; set; }
    public List<EncounterItemBase> EncounterItems { get; set; }
    private string? currentEncounterItemId;
    private int round;
    public EncounterItemBase? CurrentEncounterItem
    {
        get
        {
            if (currentEncounterItemId == null)
            {
                return null;
            }

            return EncounterItems.Find(x => x.Id == currentEncounterItemId);
        }
    }

    public int Round
    {
        get => round;
    }

    public EncounterBase()
    {
        EncounterItems = new List<EncounterItemBase>();
    }

    public EncounterBase(ImprovedInitiativeImporter.Encounter importedEncounter)
    {
        Name = importedEncounter.Name;
        Path = importedEncounter.Path;
        Id = importedEncounter.Id;
        EncounterItems = new List<EncounterItemBase>();
    }

    public void Sort()
    {
        EncounterItems = EncounterItems.OrderByDescending(x => x.Initiative).ThenByDescending(x => x.InitiativeTieBreaker)
            .ToList();
    }

    public void Start()
    {
        round = 0;
        currentEncounterItemId = EncounterItems.Count > 0 ? EncounterItems[0].Id : null;
    }

    public void End()
    {
        currentEncounterItemId = null;
        round = 0;
    }

    public void Next()
    {
        int index = EncounterItems.FindIndex(x => x.Id == currentEncounterItemId);
        if (index == EncounterItems.Count - 1)
        {
            round++;
            currentEncounterItemId = EncounterItems[0].Id;
        }
        else
        {
            currentEncounterItemId = EncounterItems[index + 1].Id;
        }
    }


    public void Previous()
    {
        int index = EncounterItems.FindIndex(x => x.Id == currentEncounterItemId);
        if (index == 0)
        {
            //Can't go back before round 0
            if (round == 0)
            {
                return;
            }
            round--;
            currentEncounterItemId = EncounterItems[EncounterItems.Count - 1].Id;
        }
        else
        {
            currentEncounterItemId = EncounterItems[index - 1].Id;
        }
    }
}