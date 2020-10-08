using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int ItemSlotsAmount = 16;
    [SerializeField]
    public InventorySlot[] items = new InventorySlot[ItemSlotsAmount];
    public EquipSlot[] leftHand = new EquipSlot[4]; 
    public EquipSlot[] rightHand = new EquipSlot[4];
    public EquipSlot armourSlot;
    
    public void AddItem(Item itemToAdd)
    {
        for (var i = 0; i < ItemSlotsAmount; i++)
        {
            if (items[i].InventoryItem.Item == itemToAdd)
            {
                items[i].InventoryItem.Quanity++;
                return;
            }
            if (items[i].InventoryItem != null) continue;
            items[i].InventoryItem = InventoryItemFactory.CreateInventoryItem(itemToAdd);
            items[i].InventoryItem.Item = itemToAdd;
            items[i].Image.sprite = itemToAdd.sprite;
            items[i].Image.enabled = true;
            return;
        }
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (var i = ItemSlotsAmount - 1; i >= 0; i--)
        {
            if (items[i].InventoryItem.Item != itemToRemove) continue;
            RemoveItemAt(i);
            return;
        }
    }

    public void RemoveItemAt(int position)
    {
        items[position].InventoryItem = null;
        items[position].Image.sprite = null;
        items[position].Image.enabled = false;
    }

    public void ClickItem(int position)
    {
        RemoveItemAt(position);
    }
}