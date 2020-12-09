public class ItemInventory : Inventory
{
    public EquipSlot armourSlot;
    public EquipSlot leftWeaponSlot;
    public EquipSlot rightWeaponSlot;
    public Item[] StartingItems;
    public override string Title => "Inventory";

    void Start()
    {
        foreach (var item in StartingItems)
        {
            AddItem(item);
        }
    }

    public override void EquipLeftHand(InventoryItem itemToEquip)
    {
        RemoveItem(itemToEquip.Item);
        leftWeaponSlot.EquipItem(itemToEquip);
    }

    public override void EquipRightHand(InventoryItem itemToEquip)
    {
        RemoveItem(itemToEquip.Item);
        rightWeaponSlot.EquipItem(itemToEquip);
    }

    public override void EquipCenter(InventoryItem itemToEquip)
    {
        RemoveItem(itemToEquip.Item);
        armourSlot.EquipItem(itemToEquip);
    }
}