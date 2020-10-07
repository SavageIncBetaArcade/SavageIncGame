using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPassiveAbility : PassiveAbility
{
    public StatType ModifyStatType;
    public int Amount;

    public override void Grant()
    {
        switch (ModifyStatType)
        {
            case StatType.ATTACK:
                CharacterBase.AttackModifier += Amount;
                break;
            case StatType.DEFENSE:
                CharacterBase.DefenseModifier += Amount;
                break;
            case StatType.HEALTH:
                CharacterBase.HealthModifier += Amount;
                break;
            case StatType.ENERGY:
                CharacterBase.EnergyModifier += Amount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Diminish()
    {
        switch (ModifyStatType)
        {
            case StatType.ATTACK:
                CharacterBase.AttackModifier -= Amount;
                break;
            case StatType.DEFENSE:
                CharacterBase.DefenseModifier -= Amount;
                break;
            case StatType.HEALTH:
                CharacterBase.HealthModifier -= Amount;
                break;
            case StatType.ENERGY:
                CharacterBase.EnergyModifier -= Amount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
