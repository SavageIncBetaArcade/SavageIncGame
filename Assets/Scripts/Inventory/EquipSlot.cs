using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    public InventorySlot equippedSlot;
    public Inventory inventorySection;
    public CharacterBase character;

    public delegate void OnItemChange(InventoryItem inventoryItem, InventoryItem oldItem, EquipSlot slot);
    public event OnItemChange ItemChangedEvent;

    void Awake()
    {
        character = FindObjectOfType<PlayerBase>();
    }

    public void EquipItem(InventoryItem inventoryItem)
    {
        ItemChangedEvent?.Invoke(inventoryItem, equippedSlot.InventoryItem, this);

        if (equippedSlot.InventoryItem != null) UnequipItem();
        equippedSlot.Image.sprite = inventoryItem.Item.Sprite;
        equippedSlot.Image.enabled = true;
        equippedSlot.InventoryItem = inventoryItem;

        inventoryItem.ApplyModifiers(character, true);
    }

    public void UnequipItem()
    {
        if (equippedSlot.InventoryItem == null || equippedSlot.InventoryItem.Item == null) return;
        inventorySection.AddItem(equippedSlot.InventoryItem.Item);
        if(character != null) equippedSlot.InventoryItem.UnapplyModifiers(character);
        RemoveItem();
    }
    
    private void RemoveItem()
    {
        ItemChangedEvent?.Invoke(null, equippedSlot.InventoryItem, this);

        equippedSlot.Image.sprite = null;
        equippedSlot.Image.enabled = false;
        equippedSlot.InventoryItem = null;
    }
}
