using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AttackAbility is a BaseAbility with an additional Hit method and Hit event
/// MeleeAbility, ProjectileAbility and RaycaseAbility all derive from the AttackAbility
/// </summary>
public abstract class AttackAbility : BaseAbility
{
    public delegate void HitAction(CharacterBase attackingCharacter, GameObject targetObject, Vector3 hitPoint, Vector3 hitNormal);
    public event HitAction OnHit;

    protected AttackAbility(UseableAbility useableAbility, CharacterBase ownerCharacter) : base(useableAbility, ownerCharacter)
    {

    }

    public virtual void Hit(GameObject hitObject, float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(hitObject == OwnerCharacter.gameObject)
            return;

        hitObject.GetComponent<CharacterBase>()?.TakeDamage(damage);

        OnHit?.Invoke(useableAbility.CharacterBase,hitObject, hitPoint, hitNormal);

        foreach (var hitEffect in Ability.HitEffects)
        {
            useableAbility.InstantiateObject(hitEffect, hitObject.transform);
        }
    }

}
