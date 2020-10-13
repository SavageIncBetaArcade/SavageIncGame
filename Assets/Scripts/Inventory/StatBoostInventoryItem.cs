public class StatBoostInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        inventory.RemoveItem(Item);
        var item = (StatBoostItem)Item;
        character.ApplyStatModifier(item.statType, item.effectAmount);
    }

    public override void RightClick(Inventory inventory)
    {
    }
}