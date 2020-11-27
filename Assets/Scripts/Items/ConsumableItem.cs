using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Item/Consumable")]
public class ConsumableItem : Item
{
    public override string GetInfoDescription()
    {
        return Description;
    }
}