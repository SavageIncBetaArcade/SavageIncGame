public class WeaponInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory)
    {
        inventory.EquipWeaponInLeftHand(this);
    }

    public override void RightClick(Inventory inventory)
    {
        inventory.EquipWeaponInRightHand(this);
    }
}