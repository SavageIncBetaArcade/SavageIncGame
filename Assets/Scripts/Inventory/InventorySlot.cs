using System;
using TMPro;
using UnityEngine.UI;

[Serializable]
public struct InventorySlot
{
    public Image Image;
    public InventoryItem InventoryItem;
    public TextMeshProUGUI Quantity;
}