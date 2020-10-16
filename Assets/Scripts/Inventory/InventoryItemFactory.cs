using System;

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
            case HealthPotionItem _:
                return new HealthPotionInventoryItem();
            case EnergyPotionItem _:
                return new EnergyPotionInventoryItem();
            case StatBoostItem _:
                return new StatBoostInventoryItem();
            case SpeedAttackBoostItem _:
                return new SpeedAttackBoostInventoryItem();
        }
        throw new Exception("Item not of any type");
    }
}