using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        //TODO: check if the hit object was a AbilityInteractionTrigger
        AbilityInteractionTrigger interactionTrigger = hitObject.GetComponent<AbilityInteractionTrigger>();
        if (interactionTrigger && interactionTrigger.Abilities.Contains(Ability))
        {
            interactionTrigger.Interact();
        }

        CharacterBase hitCharacter = GetParentCharacterBase(hitObject.transform);

        OnHit?.Invoke(useableAbility.CharacterBase,hitObject, hitPoint, hitDirection, surfaceNormal);

        float damageDealt = Ability.AddOwnerBaseAttack ? damage + (OwnerCharacter.AttackModifier * Ability.OwnerBaseAttackScalar) : damage;
        Debug.Log($"Character:{OwnerCharacter.name} hit with:{Ability.Name} dealing:{damageDealt}");
        if (hitCharacter != null)
            Debug.Log($"Character:{OwnerCharacter.name} hit character:{hitCharacter.name} with:{Ability.Name} dealing:{damageDealt}, target def:{hitCharacter.DefenseModifier}");

        hitCharacter?.TakeDamage(damageDealt);
        


        foreach (var hitEffect in Ability.HitEffects)
        {
            useableAbility.InstantiateObject(hitEffect, hitPoint, Quaternion.identity);
        }

        OnEndAttack?.Invoke(hitCharacter);
    }

    private CharacterBase GetParentCharacterBase(Transform currentTransform)
    {
        CharacterBase characterBase = currentTransform.GetComponent<CharacterBase>();
        if (characterBase)
            return characterBase;

        if (!currentTransform.parent)
            return null;

        return GetParentCharacterBase(currentTransform.parent);
    }
}
