using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility
{
    //Abilities that use mono behavior are all useable I.E (Weapons, Buffs) - Passive abilities work in a different way
    protected readonly ScriptableUseableAbility ability;
    protected readonly UseableAbility useableAbility;

    protected BaseAbility(UseableAbility useableAbility)
    {
        this.ability = useableAbility.ScriptableAbility;
        this.useableAbility = useableAbility;
    }

    public ScriptableUseableAbility Ability => ability;

    public abstract void Initilise();
    public abstract void Use();

}
