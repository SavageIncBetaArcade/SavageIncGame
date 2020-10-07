using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    private InventoryItem equippedItem;

    public void EquipItem(InventoryItem itemToEquip)
    {
        equippedItem = itemToEquip;
    }

    public void RemoveItem()
    {
        equippedItem.Image = null;
        equippedItem.Item = null;
    }
}
