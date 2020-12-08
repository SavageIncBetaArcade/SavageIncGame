public class ConsumableInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        ApplyModifiers(character, Item);
        inventory.RemoveItem(Item);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        LeftClick(inventory, characterBase);
    }
}