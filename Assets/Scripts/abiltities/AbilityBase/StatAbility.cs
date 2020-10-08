using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAbility : DurationAbility
{

    public StatAbility(UseableAbility useableAbility) : base(useableAbility)
    {
    }

    public override void Initilise()
    {

    }

    public override void Grant()
    {
        Debug.Log($"Granting stat boost {ability.AbilityName} to character");
        ScriptableStatAbility statAbility = (ScriptableStatAbility) ability;

        switch (statAbility.StatType)
        {
            case StatType.ATTACK:
                useableAbility.CharacterBase.AttackModifier += statAbility.Modifier;
                break;
            case StatType.DEFENSE:
                useableAbility.CharacterBase.DefenseModifier += statAbility.Modifier;
                break;
            case StatType.HEALTH:
                useableAbility.CharacterBase.HealthModifier += statAbility.Modifier;
                break;
            case StatType.ENERGY:
                useableAbility.CharacterBase.EnergyModifier += statAbility.Modifier;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Diminish()
    {
        Debug.Log($"Diminish stat boost {ability.AbilityName} to character");
        ScriptableStatAbility statAbility = (ScriptableStatAbility)ability;

        switch (statAbility.StatType)
        {
            case StatType.ATTACK:
                useableAbility.CharacterBase.AttackModifier -= statAbility.Modifier;
                break;
            case StatType.DEFENSE:
                useableAbility.CharacterBase.DefenseModifier -= statAbility.Modifier;
                break;
            case StatType.HEALTH:
                useableAbility.CharacterBase.HealthModifier -= statAbility.Modifier;
                break;
            case StatType.ENERGY:
                useableAbility.CharacterBase.EnergyModifier -= statAbility.Modifier;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
