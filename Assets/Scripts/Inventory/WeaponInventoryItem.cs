public class WeaponInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        inventory.Equip(this, inventory.leftWeaponSlot);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        inventory.Equip(this, inventory.rightWeaponSlot);
    }
}