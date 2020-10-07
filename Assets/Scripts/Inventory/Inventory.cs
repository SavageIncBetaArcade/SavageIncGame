using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int ItemSlotsAmount = 16;
    [SerializeField]
    public InventoryItem[] items = new InventoryItem[ItemSlotsAmount];

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
            RemoveItemAt(i);
            return;
        }
    }

    public void RemoveItemAt(int position)
    {
        items[position].Item = null;
        items[position].Image.sprite = null;
        items[position].Image.enabled = false;
    }
}
