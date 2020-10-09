using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public InventorySlot[] items = new InventorySlot[ItemSlotsAmount];
    public EquipSlot[] leftHand = new EquipSlot[4]; 
    public EquipSlot[] rightHand = new EquipSlot[4];
    public EquipSlot armourSlot;
    public const int ItemSlotsAmount = 16;
    
    public void AddItem(Item itemToAdd)
    {
        for (var i = 0; i < ItemSlotsAmount; i++)
        {
            if (ItemCanBeAssignedToEmptySlot(i, itemToAdd)) return;
            if (!InventorySlotIs(i, itemToAdd)) continue;
            IncreaseItemQuantity(i);
            return;
        }
        throw new Exception("Couldn't add item");
    }

    private bool ItemCanBeAssignedToEmptySlot(int position, Item itemToAdd)
    {
        if (!InventorySlotIsEmpty(position)) return false;
        AssignItemSlot(position, itemToAdd);
        return true;

    }

    private bool InventorySlotIsEmpty(int position) { return items[position].InventoryItem == null; }
    
    private bool InventorySlotIs(int position, Item item) { return items[position].InventoryItem.Item == item; }
    
    private void IncreaseItemQuantity(int position) { items[position].InventoryItem.Quanity++; }
    
    private void AssignItemSlot(int position, Item itemToAdd)
    {
        items[position].InventoryItem = InventoryItemFactory.CreateInventoryItem(itemToAdd);
        items[position].InventoryItem.Item = itemToAdd;
        items[position].Image.sprite = itemToAdd.sprite;
        items[position].Image.enabled = true;
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (var i = 0; i < ItemSlotsAmount; i++)
        {
            if (items[i].InventoryItem.Item != itemToRemove) continue;
            if (ReduceItemQuantity(i)) return;
            RemoveItemAt(i);
            return;
        }
    }

    private bool ReduceItemQuantity(int i)
    {
        if (items[i].InventoryItem.Quanity <= 1) return false;
        items[i].InventoryItem.Quanity--;
        return true;

    }

    private void RemoveItemAt(int position)
    {
        items[position].InventoryItem = null;
        items[position].Image.sprite = null;
        items[position].Image.enabled = false;
    }

    public void ClickItem(int position)
    {
        items[position].InventoryItem.Click(this);
        RemoveItemAt(position);
    }
}