using TMPro;
using UnityEngine;

public class InventorySectionHandler : MonoBehaviour
{
    public ItemInventory itemInventory;
    public AbilityInventory abilityInventory;
    public Inventory currentInventory;
    public TextMeshProUGUI inventoryTitle;

    private void Start()
    {
        currentInventory = itemInventory;
    }

    public void SwitchInventories()
    {
        currentInventory.gameObject.SetActive(false);
        SwitchTo(OtherInventory());
        currentInventory.gameObject.SetActive(true);
        inventoryTitle.text = currentInventory.Title;
    }

    private void SwitchTo(Inventory newInventory)
    {
        currentInventory = newInventory;
    }
    
    private Inventory OtherInventory()
    {
        return currentInventory == itemInventory ? (Inventory) abilityInventory : itemInventory;
    }
}