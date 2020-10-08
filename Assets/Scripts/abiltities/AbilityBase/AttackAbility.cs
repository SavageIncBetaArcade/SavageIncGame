using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AttackAbility is a BaseAbility with an additional Hit method and OnHit event
/// MeleeAbility, ProjectileAbility and RaycaseAbility all derive from the AttackAbility
/// </summary>
public abstract class AttackAbility : BaseAbility
{
    public delegate void HitAction(CharacterBase attackingCharacter, CharacterBase targetCharacter);
    public event HitAction OnHit;

    protected AttackAbility(UseableAbility useableAbility) : base(useableAbility)
    {

    }

    protected virtual void Hit(CharacterBase targetCharacter)
    {
        //TODO make calls to damage system

        OnHit?.Invoke(useableAbility.CharacterBase,targetCharacter);

    }

}
