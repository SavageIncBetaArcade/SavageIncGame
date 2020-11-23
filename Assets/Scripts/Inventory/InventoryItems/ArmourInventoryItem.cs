public class ArmourInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        ApplyModifiers(character);
        inventory.EquipCenter(this);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        LeftClick(inventory, null);
    }
}