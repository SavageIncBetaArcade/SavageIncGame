public class AbilityInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        inventory.EquipAbilityInLeftHand(this);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        inventory.EquipAbilityInRightHand(this);
    }
}