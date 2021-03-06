﻿using System;

public static class InventoryItemFactory
{
    public static InventoryItem CreateInventoryItem(Item itemToAdd)
    {
        switch (itemToAdd)
        {
            case ArmourItem _:
                return new ArmourInventoryItem();
            case WeaponItem _:
                return new WeaponInventoryItem();
            case ConsumableItem _:
                return new ConsumableInventoryItem();
            case KeyItem _:
                return new KeyInventoryItem();
            case ScriptableUseableAbility _:
                return new AbilityInventoryItem();
        }
        throw new Exception("Item not of any type");
    }
}