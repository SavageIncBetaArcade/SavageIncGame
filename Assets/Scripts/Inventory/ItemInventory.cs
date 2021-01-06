using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : Inventory
{
    public EquipSlot armourSlot;
    public EquipSlot leftWeaponSlot;
    public EquipSlot rightWeaponSlot;
    public Item[] StartingItems;
    public override Sprite TitleImage => Resources.Load<Sprite>("Inventory/inventory");

    void Start()
    {
        foreach (var item in StartingItems)
        {
            AddItem(item);
        }
    }

    public override void EquipLeftHand(InventoryItem itemToEquip, bool swap)
    {
        if (!swap && leftWeaponSlot.equippedSlot.InventoryItem  != null)
        {
            return;
        }

        RemoveItem(itemToEquip.Item);
        leftWeaponSlot.EquipItem(itemToEquip);
    }

    public override void EquipRightHand(InventoryItem itemToEquip, bool swap)
    {
        if (!swap && rightWeaponSlot.equippedSlot.InventoryItem != null)
        {
            return;
        }

        RemoveItem(itemToEquip.Item);
        rightWeaponSlot.EquipItem(itemToEquip);
    }

    public override void EquipCenter(InventoryItem itemToEquip, bool swap)
    {
        if (!swap && armourSlot.equippedSlot.InventoryItem != null)
        {
            return;
        }

        RemoveItem(itemToEquip.Item);
        armourSlot.EquipItem(itemToEquip);
    }

    #region IDataPersistance

    public override void Save(DataContext context)
    {
        base.Save(context);

        //Save armour/weapons slots
        if(armourSlot.equippedSlot.InventoryItem != null && armourSlot.equippedSlot.InventoryItem.Item)
        {
            string path = armourSlot.equippedSlot.InventoryItem.Item
                .AssetPath.Replace("Resources/", "").Replace(".asset", "");
            context.SaveData(uuid.ID, "armourSlot", path);
        }
        if (leftWeaponSlot.equippedSlot.InventoryItem != null && leftWeaponSlot.equippedSlot.InventoryItem.Item)
        {
            string path = leftWeaponSlot.equippedSlot.InventoryItem.Item
                .AssetPath.Replace("Resources/", "").Replace(".asset", "");
            context.SaveData(uuid.ID, "leftWeapon", path);
        }
        if (rightWeaponSlot.equippedSlot.InventoryItem != null && rightWeaponSlot.equippedSlot.InventoryItem.Item)
        {
            string path = rightWeaponSlot.equippedSlot.InventoryItem.Item
                .AssetPath.Replace("Resources/", "").Replace(".asset", "");
            context.SaveData(uuid.ID, "rightWeapon", path);
        }
    }

    public override void Load(DataContext context, bool destroyUnloaded = false)
    {
        base.Load(context, destroyUnloaded);

        armourSlot.equippedSlot.InventoryItem?.UnapplyModifiers(character);
        leftWeaponSlot.equippedSlot.InventoryItem?.UnapplyModifiers(character);
        rightWeaponSlot.equippedSlot.InventoryItem?.UnapplyModifiers(character);

        armourSlot.equippedSlot.Clear();
        leftWeaponSlot.equippedSlot.Clear();
        rightWeaponSlot.equippedSlot.Clear();

        var armourPath = context.GetValue<string>(uuid.ID, "armourSlot");
        var leftWeaponPath = context.GetValue<string>(uuid.ID, "leftWeapon");
        var rightWeaponPath = context.GetValue<string>(uuid.ID, "rightWeapon");

        if(!string.IsNullOrWhiteSpace(armourPath))
        {
            var item = Resources.Load<Item>(armourPath);
            if (item) AddItem(item, false).LeftClick(this, character);
        }
        if (!string.IsNullOrWhiteSpace(leftWeaponPath))
        {
            var item = Resources.Load<Item>(leftWeaponPath);
            if (item) AddItem(item, false).LeftClick(this, character);
        }
        if (!string.IsNullOrWhiteSpace(rightWeaponPath))
        {
            var item = Resources.Load<Item>(rightWeaponPath);
            if (item) AddItem(item, false).RightClick(this, character);
        }
    }

    #endregion
}