using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPool : MonoBehaviour
{
    public InteractionTrigger Trigger;
    public InventorySectionHandler InventorySection;
    public bool OnlyLootOnce = true;
    public bool DisableObjectOnLoot = false;

    public ScriptableUseableAbility[] Items;

    //TODO save looted var
    private bool looted = false;

    public void Awake()
    {
        Trigger.OnTrigger += trigger;
    }

    private void OnDestroy()
    {
        Trigger.OnTrigger -= trigger;
    }

    public ScriptableUseableAbility GetRandomItem()
    {
        if(Items.Length > 0)
        {
            return Items[Random.Range(0, Items.Length)];
        }

        return null;
    }

    void trigger(bool triggered, InteractionTrigger trigger)
    {
        if (OnlyLootOnce && looted)
            return;

        ScriptableUseableAbility item = gameObject.GetComponent<LootPool>().GetRandomItem();

        if (item)
        {
            InventorySection.abilityInventory.AddItem(item);
            looted = true;

            if (DisableObjectOnLoot)
                gameObject.SetActive(false);
        }
    }
}
