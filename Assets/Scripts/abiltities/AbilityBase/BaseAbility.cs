using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Ability is the base class for all abilities and is not part of mono behavior.
/// This class contains the Scriptable Ability that contains the data of an ability.
/// The BaseAbility also includes a reference to the UseableAbility script that gets set via the constructor
///
/// All abilties must implement the following methods:
///     Initilise()
///     Use()
/// 
/// To utilise the ability system you should use the UseableAbility script
/// </summary>
public abstract class BaseAbility
{
    protected readonly UseableAbility useableAbility;
    private readonly ScriptableUseableAbility ability;
    private readonly CharacterBase ownerCharacter;

    protected BaseAbility(UseableAbility useableAbility, CharacterBase ownerCharacter)
    {
        this.ability = useableAbility.ScriptableAbility;
        this.useableAbility = useableAbility;
        this.ownerCharacter = ownerCharacter;
    }

    public ScriptableUseableAbility Ability => ability;
    public CharacterBase OwnerCharacter => ownerCharacter;

    /// <summary>
    /// Initilise is used to initilse the ability - called on the UseableAbility awake method
    /// </summary>
    public abstract void Initilise();

    /// <summary>
    /// The Use method is the call that gets made when the character uses the ability E.G on player fire 
    /// </summary>
    public abstract void Use();

}

public static class AbilityFactory
{
    public static BaseAbility Create(UseableAbility useableAbility, Item scriptableAbility, CharacterBase ownerCharacter,
        GameObject worldGameObject, AttackAbility.HitAction hitAction = null, AttackAbility.EndAttackAction endAttackAction = null)
    {
        Func<BaseAbility> abilityFunc = () =>
        {
            switch (scriptableAbility)
            {
                case ScriptableMeleeAbility _:
                {
                    ForwardTriggerCollision forwardTrigger =
                        worldGameObject.GetComponentInChildren<ForwardTriggerCollision>();
                    MeleeAbility meleeAbility = new MeleeAbility(useableAbility, forwardTrigger, ownerCharacter);
                    if (hitAction != null)
                        meleeAbility.OnHit += hitAction;
                    if (endAttackAction != null)
                        meleeAbility.OnEndAttack += endAttackAction;

                    return meleeAbility;
                }
                case ScriptableRaycastAbility _:
                    RaycastAbilitiy raycastAbility = new RaycastAbilitiy(useableAbility, ownerCharacter);
                    if (hitAction != null)
                        raycastAbility.OnHit += hitAction;
                    if (endAttackAction != null)
                        raycastAbility.OnEndAttack += endAttackAction;
                    return raycastAbility;
                case ScriptableProjectileAbility _:
                    ProjectileAbility projectileAbility = new ProjectileAbility(useableAbility, ownerCharacter);
                    if (hitAction != null)
                        projectileAbility.OnHit += hitAction;
                    if (endAttackAction != null)
                        projectileAbility.OnEndAttack += endAttackAction;
                    return projectileAbility;
                case ScriptableUseableAbility _:
                    return new ModifierAbility(useableAbility, ownerCharacter);
            }

            throw new Exception($"{scriptableAbility.GetType()} doesn't exist within the AbilityFactory");
        };

        //Add base abilities to useable object
        BaseAbility baseAbility = abilityFunc.Invoke();
        useableAbility.Modifiers.AddRange(baseAbility.Ability.StartingModifiers);

        return baseAbility;
    }
}