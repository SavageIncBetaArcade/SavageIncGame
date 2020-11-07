﻿public class WeaponInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        inventory.EquipLeftHand(this);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        inventory.EquipRightHand(this);
    }
}