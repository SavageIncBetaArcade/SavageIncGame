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
        Debug.Log($"Granting stat boost {Ability.AbilityName} to character");
        ScriptableStatAbility statAbility = (ScriptableStatAbility) Ability;

        useableAbility.CharacterBase.ApplyStatModifier(statAbility.StatType, statAbility.Modifier);
    }

    public override void Remove()
    {
        Debug.Log($"OnRemove stat boost {Ability.AbilityName} to character");
        ScriptableStatAbility statAbility = (ScriptableStatAbility)Ability;

        useableAbility.CharacterBase.ApplyStatModifier(statAbility.StatType, -statAbility.Modifier);
    }
}
