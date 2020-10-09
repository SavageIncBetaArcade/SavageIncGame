public class WeaponInventoryItem : InventoryItem
{
    public override void Click(Inventory inventory)
    {
        inventory.EquipWeapon(this);
    }
}