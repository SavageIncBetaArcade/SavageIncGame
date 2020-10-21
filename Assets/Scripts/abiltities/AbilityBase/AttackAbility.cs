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

    protected AttackAbility(UseableAbility useableAbility, CharacterBase ownerCharacter) : base(useableAbility, ownerCharacter)
    {

    }

    public virtual void Hit(CharacterBase targetCharacter, float damage)
    {
        targetCharacter.TakeDamage(damage);

        OnHit?.Invoke(useableAbility.CharacterBase,targetCharacter);

        foreach (var hitEffect in Ability.HitEffects)
        {
            useableAbility.InstantiateObject(hitEffect, targetCharacter.transform);
        }
    }

}
