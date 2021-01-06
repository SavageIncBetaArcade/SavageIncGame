using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UUID))]
public class LootPool : MonoBehaviour, IDataPersistance
{
    public InteractionTrigger Trigger;
    public InventorySectionHandler InventorySection;
    public bool OnlyLootOnce = true;
    public bool DisableObjectOnLoot = false;

    public Item[] Items;

    //TODO save looted var
    private bool looted = false;
    private UUID uuid;

    public void Awake()
    {
        Trigger.OnTrigger += trigger;
        uuid = GetComponent<UUID>();
        if(!InventorySection)
            InventorySection = DataPersitanceHelpers.FindAllGameObjects<InventorySectionHandler>().FirstOrDefault();
    }

    private void OnDestroy()
    {
        Trigger.OnTrigger -= trigger;
    }

    public Item GetRandomItem()
    {
        if(Items.Length > 0)
        {
            return Items[Random.Range(0, Items.Length)];
        }

        return null;
    }

    void trigger(bool triggered, InteractionTrigger trigger)
    {
        if (!InventorySection || (OnlyLootOnce && looted))
            return;

        Item item = GetRandomItem();

        if (item)
        {
            switch (item)
            {
                case EquippableItem _:
                    InventorySection.itemInventory.AddItem(item);
                    break;
                case KeyItem _:
                    InventorySection.itemInventory.AddItem(item);
                    break;
                case ConsumableItem _:
                    InventorySection.itemInventory.AddItem(item);
                    break;
                case ScriptableUseableAbility _:
                    InventorySection.abilityInventory.AddItem(item);
                    break;
            }
            looted = true;

            if (DisableObjectOnLoot)
                gameObject.SetActive(false);
        }
    }

    #region IPersitantData
    public void Save(DataContext context)
    {
        if (!uuid)
            return;

        context.SaveData(uuid.ID, "looted", looted);
    }

    public void Load(DataContext context, bool destroyUnloaded = false)
    {
        if (!uuid)
            return;

        looted = context.GetValue<bool>(uuid.ID, "looted");
    }
    #endregion
}
