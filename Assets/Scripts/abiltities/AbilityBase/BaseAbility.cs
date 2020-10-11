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
    protected readonly ScriptableUseableAbility ability;
    protected readonly UseableAbility useableAbility;

    protected BaseAbility(UseableAbility useableAbility)
    {
        this.ability = useableAbility.ScriptableAbility;
        this.useableAbility = useableAbility;
    }

    public ScriptableUseableAbility Ability => ability;

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
    public static BaseAbility Create(UseableAbility useableAbility, ScriptableAbility scriptableAbility, 
        GameObject worldGameObject, AttackAbility.HitAction hitAction = null)
    {

        switch (scriptableAbility)
        {
            case ScriptableMeleeAbility _:
            {
                ForwardTriggerCollision forwardTrigger = worldGameObject.GetComponentInChildren<ForwardTriggerCollision>();
                MeleeAbility meleeAbility = new MeleeAbility(useableAbility, forwardTrigger);
                if (hitAction != null)
                    meleeAbility.OnHit += hitAction;

                return meleeAbility;
            }
            case ScriptableRaycastAbility _:
                RaycastAbilitiy raycastAbility = new RaycastAbilitiy(useableAbility);
                if (hitAction != null)
                    raycastAbility.OnHit += hitAction;
                return raycastAbility;
            case ScriptableProjectileAbility _:
                ProjectileAbility projectileAbility = new ProjectileAbility(useableAbility);
                if (hitAction != null)
                    projectileAbility.OnHit += hitAction;
                return projectileAbility;
            case ScriptableUseableAbility _:
                return new ModifierAbility(useableAbility);
        }

        throw new Exception($"{scriptableAbility.GetType()} doesn't exist within the AbilityFactory");
    }
}