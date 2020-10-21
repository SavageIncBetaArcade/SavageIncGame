public class ConsumableInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        if (!(Item is ConsumableItem consumableItem)) return;
        new Modifier(consumableItem.modifier, character).Apply(character, character);
        inventory.RemoveItem(Item);
    }

    public override void RightClick(Inventory inventory, CharacterBase characterBase)
    {
        LeftClick(inventory, characterBase);
    }
}