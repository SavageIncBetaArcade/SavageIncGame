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

    public void TakeDamage(int attackDamage)
    {
        currentHealth -= (float)(attackDamage * Math.Pow(0.95, defenseModifier));
        HandleHealthBoundaries();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        HandleHealthBoundaries();
    }
    
    private void HandleHealthBoundaries()
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Die
        }
    }
}