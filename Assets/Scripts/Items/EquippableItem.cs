public class EquippableItem : Item
{
    public int attack;
    public int defense;
    public int health;
    public int energy;
    public float healthRegen;
    public float energyRegen;

    protected EquippableItem()
    {
        AssignModifiers();
    }
    
    public override string GetInfoDescription()
    {
        return getStatString(attack, "Attack") +
               getStatString(defense, "Defense") +
               getStatString(health, "Health") +
               getStatString(energy, "Energy") +
               getRegenString(healthRegen, "Health") + 
               getRegenString(energyRegen, "Energy");
    }

    private string getStatString(int stat, string statName) { return stat != 0 ? $"+ {stat} {statName}\n" : ""; }
    
    private string getRegenString(float stat, string statName) { return stat != 0 ? $"+{stat}/second {statName} Regen\n" : ""; }

    private void AssignModifiers()
    {
        if (attack != 0) CreateModifier(StatType.ATTACK, attack);
        if (defense != 0) CreateModifier(StatType.DEFENSE, defense);
        if (health != 0) CreateModifier(StatType.HEALTH, health);
        if (energy != 0) CreateModifier(StatType.ENERGY, energy);
        // if (healthRegen != 0) result.Add(AddModifier(StatType.HEALTH_REGEN, attack));
        // if (energyRegen != 0) result.Add(AddModifier(StatType.ENERGY_REGEN, attack));
    }

    private void CreateModifier(StatType type, int amount)
    {
        var mod = CreateInstance<ScriptableStatModifier>();
        mod.Type = type;
        mod.Amount = amount;
        modifiers.Add(mod);
    }
}