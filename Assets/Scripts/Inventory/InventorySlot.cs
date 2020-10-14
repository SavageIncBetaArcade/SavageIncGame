using System;
using UnityEngine.UI;

[Serializable]
public struct InventorySlot
{
    public Image Image;
    public InventoryItem InventoryItem;
    public Text Quantity;
}