public class ArmourInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory)
    {
        inventory.RemoveItem(Item);
        inventory.armourSlot.EquipItem(this);
    }

    public override void RightClick(Inventory inventory)
    {
        LeftClick(inventory);
    }
}