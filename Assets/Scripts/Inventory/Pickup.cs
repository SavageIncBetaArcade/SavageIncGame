using UnityEngine;

public class Pickup : MonoBehaviour
{
    public ItemInventory itemInventory;
    public AbilityInventory abilityInventory;
    public Item armourItem;
    public Item armourItem2;
    public Item weaponItem;
    public Item weaponItem2;
    public ConsumableItem healthPotion;
    public ScriptableUseableAbility ability;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            itemInventory.AddItem(armourItem);
        else if (Input.GetKeyDown(KeyCode.R))
            itemInventory.AddItem(armourItem2);
        else if(Input.GetKeyDown(KeyCode.T))
            itemInventory.AddItem(weaponItem);
        else if(Input.GetKeyDown(KeyCode.Y))
            itemInventory.AddItem(weaponItem2);
        else if(Input.GetKeyDown(KeyCode.U))
            itemInventory.AddItem(healthPotion);
        else if(Input.GetKeyDown(KeyCode.I))
            abilityInventory.AddItem(ability);
    }
}