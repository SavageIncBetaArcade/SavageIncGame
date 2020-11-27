using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    [SerializeField]
    public InventorySlot[] items = new InventorySlot[ItemSlotsAmount];
    public const int ItemSlotsAmount = 16;
    public CharacterBase character;
    public abstract string Title { get; }

    public abstract void EquipLeftHand(InventoryItem itemToEquip);
    public abstract void EquipRightHand(InventoryItem itemToEquip);
    public abstract void EquipCenter(InventoryItem itemToEquip);

    void Awake()
    {
        character = FindObjectOfType<PlayerBase>();
    }

    public void AddItem(Item itemToAdd)
    {
        var emptySlotPosition = -1;
        for (var i = ItemSlotsAmount - 1; i >= 0; i--)
        {
            if (InventorySlotIsEmpty(i)) emptySlotPosition = i;
            if (!InventorySlotIs(i, itemToAdd)) continue;
            IncreaseItemQuantity(i);
            return;
        }
        AssignItemSlot(emptySlotPosition, itemToAdd);
    }
    
    private bool InventorySlotIsEmpty(int position) { return items[position].InventoryItem == null; }
    
    private bool InventorySlotIs(int position, Item item) { return !InventorySlotIsEmpty(position) && items[position].InventoryItem.Item == item; }

    private void IncreaseItemQuantity(int position)
    {
        items[position].InventoryItem.Quantity++;
        UpdateQuantityUiAt(position);
    }

    private void UpdateQuantityUiAt(int position) { items[position].Quantity.text = items[position].InventoryItem.Quantity.ToString(); }

    private void AssignItemSlot(int position, Item itemToAdd)
    {
        items[position].InventoryItem = InventoryItemFactory.CreateInventoryItem(itemToAdd);
        items[position].InventoryItem.Item = itemToAdd;
        items[position].Image.sprite = itemToAdd.Sprite;
        items[position].Image.enabled = true;
        UpdateQuantityUiAt(position);
        items[position].Quantity.enabled = true;
    }

    public int FindItemIndex(Item item)
    {
        for (var i = 0; i < ItemSlotsAmount; i++)
        {
            if (!InventorySlotIs(i, item)) continue;
            return i;
        }
        return -1;
    }
    
    public void RemoveItem(Item itemToRemove)
    {
        for (var i = 0; i < ItemSlotsAmount; i++)
        {
            if (!InventorySlotIs(i, itemToRemove)) continue;
            if (ReduceItemQuantity(i)) return;
            RemoveItemAt(i);
            return;
        }
    }

    private bool ReduceItemQuantity(int position)
    {
        if (!ItemQuantityCanBeReducedAt(position)) return false;
        items[position].InventoryItem.Quantity--;
        UpdateQuantityUiAt(position);
        return true;
    }

    private bool ItemQuantityCanBeReducedAt(int position) { return items[position].InventoryItem.Quantity > 1; }

    private void RemoveItemAt(int position)
    {
        items[position].InventoryItem = null;
        items[position].Image.sprite = null;
        items[position].Image.enabled = false;
        items[position].Quantity.enabled = false;
    }

    public void LeftClickItem(int position)
    {
        if (items[position].InventoryItem != null)
            items[position].InventoryItem.LeftClick(this, character);
    }

    public void RightClickItem(int position)
    {
        if (items[position].InventoryItem != null)
            items[position].InventoryItem.RightClick(this, character);
    }

    public Item getItemAt(int position) { return items[position].InventoryItem?.Item; }

    public bool hasItemAt(int position) { return items[position].InventoryItem != null; }
}