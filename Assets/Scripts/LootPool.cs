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

    #region
    public Dictionary<string, object> Save()
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        if (!uuid)
            return dataDictionary;

        //Load currently saved values
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "looted", looted);

        //save json to file
        DataPersitanceHelpers.SaveDictionary(ref dataDictionary, uuid.ID);

        return dataDictionary;
    }

    public Dictionary<string, object> Load(bool destroyUnloaded = false)
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        if (!uuid)
            return dataDictionary;

        //load dictionary
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        looted = DataPersitanceHelpers.GetValueFromDictionary<bool>(ref dataDictionary, "looted");

        return dataDictionary;
    }
    #endregion
}
