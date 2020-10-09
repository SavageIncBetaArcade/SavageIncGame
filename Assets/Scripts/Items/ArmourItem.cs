using UnityEngine;

[CreateAssetMenu(fileName = "Armour", menuName = "Item/Armour")]
public class ArmourItem : Item
{
    public int attack;
    public int defense;
    public int health;
    public int energy;
    public float healthRegen;
    public float energyRegen;
}