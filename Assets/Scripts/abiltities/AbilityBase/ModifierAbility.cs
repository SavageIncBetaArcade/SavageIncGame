using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ModifierAbility is a BaseAbility with that just uses the modifiers attached to it
/// because it has no hit functionality, the modifiers are only for the caster (E.G buff)
/// </summary>
public class ModifierAbility : BaseAbility
{
    public ModifierAbility(UseableAbility useableAbility) : base(useableAbility)
    {

    }

    public override void Initilise()
    {
        
    }

    public override void Use()
    {
        
    }
}
