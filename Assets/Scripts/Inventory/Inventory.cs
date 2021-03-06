﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UUID))]
public abstract class Inventory : MonoBehaviour, IDataPersistance
{
    [SerializeField]
    public InventorySlot[] items = new InventorySlot[ItemSlotsAmount];
    public const int ItemSlotsAmount = 16;
    public CharacterBase character;
    public abstract Sprite TitleImage { get; }

    public abstract void EquipLeftHand(InventoryItem itemToEquip, bool swap);
    public abstract void EquipRightHand(InventoryItem itemToEquip, bool swap);
    public abstract void EquipCenter(InventoryItem itemToEquip, bool swap);

    protected UUID uuid;

    protected virtual void Awake()
    {
        character = FindObjectOfType<PlayerBase>();
        uuid = GetComponent<UUID>();
    }

    public InventoryItem AddItem(Item itemToAdd, bool autoEquip = true)
    {
        var emptySlotPosition = -1;
        for (var i = ItemSlotsAmount - 1; i >= 0; i--)
        {
            if (InventorySlotIsEmpty(i)) emptySlotPosition = i;
            if (!InventorySlotIs(i, itemToAdd)) continue;
            IncreaseItemQuantity(i);

            if(autoEquip)
                autoEquipItem(items[i].InventoryItem);
            return items[i].InventoryItem;
        }
        return AssignItemSlot(emptySlotPosition, itemToAdd, autoEquip);
    }

    private void autoEquipItem(InventoryItem item)
    {
        switch (item)
        {
            case AbilityInventoryItem abilityInventoryItem:
                EquipRightHand(item, false);
                break;
            case ArmourInventoryItem armourInventoryItem:
                EquipCenter(item, false);
                break;
            case WeaponInventoryItem weaponInventoryItem:
                EquipLeftHand(item, false);
                break;
        }
    }

    private bool InventorySlotIsEmpty(int position) { return items[position].InventoryItem == null; }
    
    private bool InventorySlotIs(int position, Item item) { return !InventorySlotIsEmpty(position) && items[position].InventoryItem.Item == item; }

    private void IncreaseItemQuantity(int position)
    {
        items[position].InventoryItem.Quantity++;
        UpdateQuantityUiAt(position);
    }

    private void UpdateQuantityUiAt(int position) { items[position].Quantity.text = items[position].InventoryItem.Quantity.ToString(); }

    private InventoryItem AssignItemSlot(int position, Item itemToAdd, bool autoEquip)
    {
        items[position].InventoryItem = InventoryItemFactory.CreateInventoryItem(itemToAdd);
        items[position].InventoryItem.Item = itemToAdd;
        items[position].Image.sprite = itemToAdd.Sprite;
        items[position].Image.enabled = true;
        UpdateQuantityUiAt(position);
        items[position].Quantity.enabled = true;

        if(autoEquip)
            autoEquipItem(items[position].InventoryItem);
        return items[position].InventoryItem;
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

    private void RemoveAll()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].InventoryItem = null;
            items[i].Image.enabled = false;
            items[i].Quantity.text = "";
        }
    }

    #region IDataPersistance
    public virtual void Save(DataContext context)
    {
        if (!uuid)
            return;

        Dictionary<string,int> itemPaths = new Dictionary<string, int>();
        foreach (var inventoryItem in items)
        {
            var item = inventoryItem.InventoryItem?.Item;
            if (item)
                itemPaths.Add(item.AssetPath.Replace("Resources/","").Replace(".asset", ""), inventoryItem.InventoryItem.Quantity);
        }

        context.SaveData(uuid.ID, "items", itemPaths);
    }

    public virtual void Load(DataContext context, bool destroyUnloaded = false)
    {
        if (!uuid)
            return;

        Dictionary<string, int> itemPaths;
        itemPaths =  context.GetValue<Dictionary<string, int>>(uuid.ID, "items");

        //Remove all items;
        RemoveAll();

        //add loaded items
        foreach (var itemPath in itemPaths)
        {
            //load asset
            var item = Resources.Load<Item>(itemPath.Key);

            for (int i = 0; i < itemPath.Value; i++)
            {
                AddItem(item as Item, false);
            }
        }
    }
    #endregion
}