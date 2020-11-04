using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Item/Consumable")]
public class ConsumableItem : Item
{
    public ScriptableModifier modifier;
    public override string GetInfoDescription()
    {
        return "I'm a consumable item!";
    }
}