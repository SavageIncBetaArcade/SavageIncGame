public class ConsumableInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        new Modifier((Item as ConsumableItem)?.modifier, character).Apply(character);
        inventory.RemoveItem(Item);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        LeftClick(inventory, characterBase);
    }
}