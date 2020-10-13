public class HealthPotionInventoryItem : InventoryItem
{
    public override void LeftClick(Inventory inventory, CharacterBase character)
    {
        inventory.RemoveItem(Item);
        character.Heal(((HealthPotionItem)Item).effectAmount);
    }

    public override void RightClick(Inventory inventory)
    {
    }
}