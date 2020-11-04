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
    
    public override string GetInfoDescription()
    {
        return getStatString(attack, "Attack") +
               getStatString(defense, "Defense") +
               getStatString(health, "Health") +
               getStatString(energy, "Energy)" +
               getRegenString(healthRegen, "Health") + 
               getRegenString(energyRegen, "Energy"));
    }

    private string getStatString(int stat, string statName) { return stat != 0 ? $"+ {stat} {statName}\n" : ""; }
    
    private string getRegenString(float stat, string statName) { return stat != 0 ? $"+{stat}/second {statName} Regen\n" : ""; }
}