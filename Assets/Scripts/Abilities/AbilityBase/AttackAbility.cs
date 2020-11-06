using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AttackAbility is a BaseAbility with an additional Hit method and Hit event
/// MeleeAbility, ProjectileAbility and RaycaseAbility all derive from the AttackAbility
/// </summary>
public abstract class AttackAbility : BaseAbility
{
    public delegate void HitAction(CharacterBase attackingCharacter, GameObject targetObject, Vector3 hitPoint, Vector3 hitDirection, Vector3 hitSurfaceNormal);
    public event HitAction OnHit;

    public delegate void EndAttackAction(CharacterBase targetCharacter);
    public event EndAttackAction OnEndAttack;

    protected AttackAbility(UseableAbility useableAbility, CharacterBase ownerCharacter) : base(useableAbility, ownerCharacter)
    {

    }

    public virtual void Hit(GameObject hitObject, float damage, Vector3 hitPoint, Vector3 hitDirection,
        Vector3 surfaceNormal)
    {
        if(hitObject == OwnerCharacter.gameObject)
            return;

        CharacterBase hitCharacter = hitObject.GetComponent<CharacterBase>();

        OnHit?.Invoke(useableAbility.CharacterBase,hitObject, hitPoint, hitDirection, surfaceNormal);

        float damageDealt = Ability.AddOwnerBaseAttack ? damage + (OwnerCharacter.AttackModifier * Ability.OwnerBaseAttackScalar) : damage;
        if (hitCharacter != null)
            Debug.Log($"Character:{OwnerCharacter.name} hit character:{hitCharacter.name} with:{Ability.Name} dealing:{damageDealt}, target def:{hitCharacter.DefenseModifier}");

        hitCharacter?.TakeDamage(damageDealt);
        


        foreach (var hitEffect in Ability.HitEffects)
        {
            useableAbility.InstantiateObject(hitEffect, hitObject.transform);
        }

        if(hitCharacter != null)
            OnEndAttack?.Invoke(hitCharacter);
    }

}
