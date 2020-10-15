using System;
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

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    private float attackModifier, defenseModifier, maxHealth, maxEnergy, speed, jumpHeight;

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
            case StatType.SPEED:
                Speed += speed;
                break;
            case StatType.JUMP_HEIGHT:
                JumpHeight += jumpHeight;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}