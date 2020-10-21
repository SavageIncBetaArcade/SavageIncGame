﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    ATTACK,
    DEFENSE,
    HEALTH,
    ENERGY,
    SPEED,
    JUMP_HEIGHT
}

public class CharacterBase : MonoBehaviour, IDamageTaker
{
    [SerializeField] 
    private float attackModifier, defenseModifier, maxHealth, maxEnergy;
    [SerializeField]
    private float speed = 6.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float currentHealth, currentEnergy;
    
    public delegate void DeathAction();
    public event DeathAction OnDeath;
    public delegate void DamageAction();
    public event DamageAction OnDamage;
    public delegate void HealAction();
    public event HealAction OnHeal;
    public delegate void ReplenishEnergyAction();
    public event ReplenishEnergyAction OnReplenishEnergy;

    private HashSet<Modifier> appliedModifiers;

    #region Properties
    public float Gravity { get; } = -9.81f;

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


    public HashSet<Modifier> AppliedModifiers => appliedModifiers;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float JumpHeight
    {
        get => jumpHeight;
        set => jumpHeight = value;
    }

    public bool IsAlive => currentHealth >= 0.0f;

    #endregion

    protected virtual void Awake()
    {
        appliedModifiers = new HashSet<Modifier>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

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
            case StatType.SPEED:
                Speed += amount;
                break;
            case StatType.JUMP_HEIGHT:
                JumpHeight += amount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public IEnumerator ApplyStatsModifierOverPeriod(StatType type, float amount, float activePeriod)
    {
        ApplyStatModifier(type,amount);
        yield return new WaitForSeconds(activePeriod);
        ApplyStatModifier(type, -amount);
    }

    public float GetStatModifier(StatType type)
    {
        switch (type)
        {
            case StatType.ATTACK:
                return AttackModifier;
            case StatType.DEFENSE:
                return DefenseModifier;
            case StatType.HEALTH:
                return MaxHealth;
            case StatType.ENERGY:
                return MaxEnergy;
            case StatType.SPEED:
                return Speed;
            case StatType.JUMP_HEIGHT:
                return JumpHeight;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void TakeDamage(float attackDamage)
    {
        OnDamage?.Invoke();
        currentHealth = Mathf.Clamp((float)(currentHealth - attackDamage * Math.Pow(0.95, defenseModifier)), 0f, maxHealth);
        if (currentHealth == 0)
            OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        OnHeal?.Invoke();
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
    }

    public void ReplenishEnergy(float amount)
    {
        OnReplenishEnergy?.Invoke();
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
    }
}