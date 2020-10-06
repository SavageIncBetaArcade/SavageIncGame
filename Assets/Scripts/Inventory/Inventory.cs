using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public const int ItemSlotsAmount = 4;
    public Image[] itemImages = new Image[ItemSlotsAmount];
    public Item[] items = new Item[ItemSlotsAmount];

    public void AddItem(Item itemToAdd)
    {
        for (var i = 0; i < items.Length; i++)
        {
            if (items[i] != null) continue;
            items[i] = itemToAdd;
            itemImages[i].sprite = itemToAdd.sprite;
            itemImages[i].enabled = true;
            return;
        }
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (var i = 0; i < items.Length; i++)
        {
            if (items[i] != itemToRemove) continue;
            items[i] = null;
            itemImages[i].sprite = null;
            itemImages[i].enabled = false;
            return;
        }
    }
}
