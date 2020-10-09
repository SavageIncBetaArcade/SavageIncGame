﻿using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    public InventorySlot equippedSlot;

    public void EquipItem(Sprite sprite, InventoryItem inventoryItem)
    {
        if (equippedSlot.InventoryItem != null) UnequipItem();
        equippedSlot.Image.sprite = sprite;
        equippedSlot.Image.enabled = true;
        equippedSlot.InventoryItem = inventoryItem;
    }

    private void RemoveItem()
    {
        equippedSlot.Image.sprite = null;
        equippedSlot.Image.enabled = false;
        equippedSlot.InventoryItem = null;
    }

    public void UnequipItem()
    {
        var inventory = FindObjectOfType<Inventory>();
        inventory.AddItem(equippedSlot.InventoryItem.Item);
        RemoveItem();
    }
}
