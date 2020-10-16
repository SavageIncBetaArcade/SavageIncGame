public class WeaponInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        inventory.EquipWeaponInLeftHand(this);
    }

    public override void RightClick(Inventory inventory)
    {
        inventory.EquipWeaponInRightHand(this);
    }
}