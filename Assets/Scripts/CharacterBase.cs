using System;
using UnityEngine;

public enum StatType
{
    ATTACK,
    DEFENSE,
    HEALTH,
    ENERGY
}

public class CharacterBase : MonoBehaviour, IDamageTaker
{
    [SerializeField]
    private float attackModifier, defenseModifier, maxHealth, maxEnergy;
    private float currentHealth, currentEnergy;
    
    public delegate void DeathAction();
    public event DeathAction OnDeath;
    public delegate void DamageAction();
    public event DamageAction OnDamage;
    public delegate void HealAction();
    public event HealAction OnHeal;
    public delegate void HealEnergyAction();
    public event HealEnergyAction OnHealEnergy;

    #region Properties
    public float AttackModifier
    {
        get => attackModifier;
        set => attackModifier = value;
    }

    public float DefenseModifier
    {
        get => defenseModifier;
        set => defenseModifier = value;
    }

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float MaxEnergy
    {
        get => maxEnergy;
        set => maxEnergy = value;
    }
    #endregion

    public void ApplyStatModifier(StatType type, float amount)
    {
        switch (type)
        {
            case StatType.ATTACK:
                AttackModifier += amount;
                break;
            case StatType.DEFENSE:
                DefenseModifier += amount;
                break;
            case StatType.HEALTH:
                MaxHealth += amount;
                break;
            case StatType.ENERGY:
                MaxEnergy += amount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void TakeDamage(float attackDamage)
    {
        OnDamage?.Invoke();
        currentHealth = Mathf.Clamp((float)(currentHealth - attackDamage * Math.Pow(0.95, defenseModifier)), 0f, maxHealth);
        if (currentHealth == 0) OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        OnHeal?.Invoke();
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
    }

    public void HealEnergy(float amount)
    {
        OnHealEnergy?.Invoke();
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
    }
}