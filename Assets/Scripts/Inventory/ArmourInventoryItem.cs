public class ArmourInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        inventory.Equip(this, inventory.armourSlot);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        LeftClick(inventory, null);
    }
}