using System.Collections.Generic;

public abstract class InventoryItem
{
    public Item Item;
    public int Quantity = 1;

    public abstract void LeftClick(Inventory inventory, CharacterBase character);
    public abstract void RightClick(Inventory inventory, CharacterBase characterBase);

    private readonly HashSet<Modifier> appliedModifiers = new HashSet<Modifier>();

    public void ApplyModifiers(CharacterBase character, bool passive = false)
    {
        foreach (var modifier in Item.modifiers)
        {
            var mod = new Modifier(modifier, character);
            mod.IsPassive = passive;
            mod.Apply(character);
            appliedModifiers.Add(mod);
        }
    }

    public void UnapplyModifiers(CharacterBase character)
    {
        foreach (var modifier in appliedModifiers)
        {
            modifier.Remove(character, true);
        }
        appliedModifiers.Clear();
    }
}