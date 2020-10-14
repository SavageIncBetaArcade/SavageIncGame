using UnityEngine;

[CreateAssetMenu(fileName = "StatBoostItem", menuName = "Item/StatBoost")]
public class StatBoostItem : Item
{
    public StatType statType;
    public float effectAmount;
    public float effectTime;
}