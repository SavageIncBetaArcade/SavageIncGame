using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct InventoryItem
{
    public Image Image;
    public Item Item;
}

public class Inventory : MonoBehaviour
{
    public const int ItemSlotsAmount = 16;
    [SerializeField]
    private InventoryItem[] items = new InventoryItem[ItemSlotsAmount];

    public void AddItem(Item itemToAdd)
    {
        for (var i = 0; i < ItemSlotsAmount; i++)
        {
            if (items[i].Item != null) continue;
            items[i].Item = itemToAdd;
            items[i].Image.sprite = itemToAdd.sprite;
            items[i].Image.enabled = true;
            return;
        }
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (var i = ItemSlotsAmount - 1; i >= 0; i--)
        {
            if (items[i].Item != itemToRemove) continue;
            items[i].Item = null;
            items[i].Image.sprite = null;
            items[i].Image.enabled = false;
            return;
        }
    }
}
