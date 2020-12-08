using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem
{
    public Item Item;
    public int Quantity = 1;

    public abstract void LeftClick(Inventory inventory, CharacterBase character);
    public abstract void RightClick(Inventory inventory, CharacterBase characterBase);

    private readonly HashSet<Modifier> appliedModifiers = new HashSet<Modifier>();

    public void ApplyModifiers(CharacterBase character, Item item, bool passive = false)
    {
        //add defence modifier
        //override the modifiers to add the attack bonus 
        var defenseModifier = ScriptableObject.CreateInstance<ScriptableStatModifier>();
        defenseModifier.Type = StatType.DEFENSE;

        switch (item)
        {
            case WeaponItem weapon:
                defenseModifier.Amount = weapon.defense;
                break;
            case ArmourItem armour:
                defenseModifier.Amount = armour.defense;

                //add attack modifier from the armour
                var attackModifier = ScriptableObject.CreateInstance<ScriptableStatModifier>();
                attackModifier.Type = StatType.ATTACK;
                attackModifier.Amount = armour.attack;
                var attackMod = new Modifier(attackModifier, character);
                attackMod.IsPassive = passive;
                attackMod.Apply(character);
                appliedModifiers.Add(attackMod);
                break;
            default:
                break;
        }
        var deffenceMod = new Modifier(defenseModifier, character);
        deffenceMod.IsPassive = passive;
        deffenceMod.Apply(character);
        appliedModifiers.Add(deffenceMod);

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