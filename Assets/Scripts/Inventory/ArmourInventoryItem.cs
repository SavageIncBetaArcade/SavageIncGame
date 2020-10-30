public class ArmourInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        inventory.RemoveItem(Item);
        inventory.armourSlot.EquipItem(this);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        LeftClick(inventory, null);
    }
}