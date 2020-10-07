using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    ATTACK,
    DEFENSE,
    HEALTH,
    ENERGY
}

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    private int attackModifier, defenseModifier, healthModifier, energyModifier, mana;

    public int AttackModifier
    {
        get => attackModifier;
        set => attackModifier = value;
    }

    public int DefenseModifier
    {
        get => defenseModifier;
        set => defenseModifier = value;
    }

    public int HealthModifier
    {
        get => healthModifier;
        set => healthModifier = value;
    }

    public int EnergyModifier
    {
        get => energyModifier;
        set => energyModifier = value;
    }

    public int Mana
    {
        get => mana;
        set => mana = value;
    }
}
