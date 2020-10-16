using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class WeaponItem : Item
{
    public int attack;
    public int defense;
    public int health;
    public int energy;
    public float healthRegen;
    public float energyRegen;
}