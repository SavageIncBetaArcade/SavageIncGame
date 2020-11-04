using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Item/Consumable")]
public class ConsumableItem : Item
{
    public ScriptableModifier modifier;
    public override string GetInfoDescription()
    {
        throw new System.NotImplementedException();
    }
}