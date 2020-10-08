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

    public override void Apply()
    {
        Debug.Log($"Granting stat boost {ability.AbilityName} to character");
        ScriptableStatAbility statAbility = (ScriptableStatAbility) ability;

        useableAbility.CharacterBase.ApplyStatModifier(statAbility.StatType, statAbility.Modifier);
    }

    public override void Remove()
    {
        Debug.Log($"Remove stat boost {ability.AbilityName} to character");
        ScriptableStatAbility statAbility = (ScriptableStatAbility)ability;

        useableAbility.CharacterBase.ApplyStatModifier(statAbility.StatType, -statAbility.Modifier);
    }
}
