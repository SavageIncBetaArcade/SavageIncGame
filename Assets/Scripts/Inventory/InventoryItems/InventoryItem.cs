public abstract class InventoryItem
{
    public Item Item;
    public int Quantity = 1;

    public abstract void LeftClick(Inventory inventory, CharacterBase character);
    public abstract void RightClick(Inventory inventory, CharacterBase characterBase);

    protected void ApplyModifiers(CharacterBase character)
    {
        foreach (var modifier in Item.modifiers)
        {
            new Modifier(modifier, character).Apply(character);
        }
    }

    public void UnapplyModifiers(CharacterBase character)
    {
        foreach (var modifier in Item.modifiers)
        {
            new Modifier(modifier, character).Remove(character);
        }
    }
}