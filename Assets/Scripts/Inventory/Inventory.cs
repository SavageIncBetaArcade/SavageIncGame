using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public InventorySlot[] items = new InventorySlot[ItemSlotsAmount];
    public EquipSlot[] leftHand = new EquipSlot[4]; 
    public EquipSlot[] rightHand = new EquipSlot[4];
    public EquipSlot armourSlot;
    public const int ItemSlotsAmount = 16;
    public Text warningText;
    public CharacterBase character;
    
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
    
    public void EquipWeaponInLeftHand(WeaponInventoryItem weaponToEquip)
    {
        if (!leftHand.Any(weaponSlot => EquipWeapon(weaponToEquip, weaponSlot)))
            StartCoroutine(ShowWarningText());
    }
    
    public void EquipWeaponInRightHand(WeaponInventoryItem weaponToEquip)
    {
        if (!rightHand.Any(weaponSlot => EquipWeapon(weaponToEquip, weaponSlot)))
            StartCoroutine(ShowWarningText());
    }
    
    private bool EquipWeapon(WeaponInventoryItem weaponToEquip, EquipSlot weaponSlot)
    {
        if (weaponSlot.equippedSlot.InventoryItem != null) return false;
        RemoveItem(weaponToEquip.Item);
        weaponSlot.EquipItem(weaponToEquip);
        return true;
    }

    private IEnumerator ShowWarningText()
    {
        warningText.enabled = true;
        yield return new WaitForSeconds(3);
        warningText.enabled = false;
    }
}