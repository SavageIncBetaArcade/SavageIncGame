﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The purpose of the Modifier class is to allow each modifier to have thier own coroutine
/// ScriptableModifier is just a single instance and will not work because of this.
/// </summary>
[Serializable]
public class Modifier
{
    [SerializeField]
    private ScriptableModifier scriptableModifier;
    [SerializeField]
    private bool isPassive = false;
    private readonly CharacterBase ownerCharacter;
    private Coroutine coroutine;

    private List<CharacterBase> affectedCharacters;

    private float timeSinceTakenDamage = 0.0f;

    #region properties

    public ScriptableModifier ScriptableModifier => scriptableModifier;
    public string ModifierName => scriptableModifier.ModifierName;
    public string ModifierDescription => scriptableModifier.ModifierDescription;
    public float ActivePeriod => scriptableModifier.ActivePeriod;
    public float ApplyFrequency => scriptableModifier.ApplyFrequency;
    public List<CharacterBase> AffectedCharacters => affectedCharacters;

    public bool IsPassive
    {
        get => isPassive;
        set => isPassive = value;
    }

    #endregion

    public Modifier(ScriptableModifier modifier, CharacterBase owner)
    {
        scriptableModifier = modifier;
        ownerCharacter = owner;
        affectedCharacters = new List<CharacterBase>();
    }

    public void Hit(Vector3 hitPoint, Vector3 hitDirection, Vector3 hitSurfaceNormal, GameObject hitObject)
    {
        if(scriptableModifier == null)
            return;

        scriptableModifier.OnHit(ownerCharacter, hitPoint, hitDirection, hitSurfaceNormal, hitObject, ref affectedCharacters);
    }

    public void Apply(CharacterBase targetCharacter)
    {
        if (ownerCharacter)
            ownerCharacter.OnDamage += onOwnerTakenDamage
                ;
        if (affectedCharacters == null)
            affectedCharacters = new List<CharacterBase>();

        if (!isPassive && scriptableModifier.ActivePeriod <= 0.0f)
        {
            scriptableModifier.OnApply(ownerCharacter, targetCharacter, ref affectedCharacters);
            //scriptableModifier.OnTick(ownerCharacter, targetCharacter, ref affectedCharacters);
            targetCharacter.AppliedModifiers.Add(this);
            //Remove(targetCharacter);
            return;
        }

        if (!isPassive)
        {
            //Check if the modifier is not already applied, if so renew it
            if (targetCharacter.AppliedModifiers.All(x => x.ScriptableModifier != scriptableModifier))
            {
                scriptableModifier.OnApply(ownerCharacter, targetCharacter, ref affectedCharacters);
                coroutine = targetCharacter.StartCoroutine(tickCoroutine(targetCharacter));
                targetCharacter.AppliedModifiers.Add(this);
            }
            else
            {
                Renew(targetCharacter);
            }
        }
        else
        {

            //passive don't need to apply as they work on ticks
            scriptableModifier.OnApply(ownerCharacter, targetCharacter, ref affectedCharacters);
            if(coroutine == null)
                coroutine = targetCharacter.StartCoroutine(tickCoroutine(targetCharacter));
        }
    }


    public void Remove(CharacterBase targetCharacter, bool forceRemove = false)
    {
        if (ownerCharacter)
            ownerCharacter.OnDamage -= onOwnerTakenDamage;

        scriptableModifier.OnRemove(ownerCharacter, targetCharacter, ref affectedCharacters);
        if (forceRemove || !isPassive)
            targetCharacter.AppliedModifiers.Remove(this);

        if (coroutine != null)
            targetCharacter.StopCoroutine(coroutine);
    }

    private void Renew(CharacterBase targetCharacter)
    {
        //get the currently applies modifier on the character
        var appliedModifier = targetCharacter.AppliedModifiers.FirstOrDefault(x => x.ScriptableModifier == scriptableModifier);

        //if the currently applied modifier has a coroutine running stop it and remove from applied modifiers set
        if (appliedModifier != null && appliedModifier.coroutine != null)
        {
            targetCharacter.StopCoroutine(appliedModifier.coroutine);
            targetCharacter.AppliedModifiers.Remove(appliedModifier);
        }
        //Start a new coroutine and add the new modifier to the applied set replacing the old one
        coroutine = targetCharacter.StartCoroutine(tickCoroutine(targetCharacter));
        targetCharacter.AppliedModifiers.Add(this);
    }

    private IEnumerator tickCoroutine(CharacterBase targetCharacter)
    {
        float currentActiveTime = scriptableModifier.ApplyFrequency;
        yield return new WaitForSeconds(scriptableModifier.ApplyFrequency);

        if (ApplyFrequency > 0.0f)
        {
            while (isPassive || (currentActiveTime <= scriptableModifier.ActivePeriod))
            {
                if (scriptableModifier is ScriptableEnergyModifier || scriptableModifier is ScriptableHealModifier)
                {
                    if(timeSinceTakenDamage + 5.0f < Time.time)
                        scriptableModifier.OnTick(ownerCharacter, targetCharacter, ref affectedCharacters);
                }
                else 
                    scriptableModifier.OnTick(ownerCharacter, targetCharacter, ref affectedCharacters);

                //Only update current active time if the ability is not passive as passive abilites will continuously tick
                if(!isPassive)
                    currentActiveTime += scriptableModifier.ApplyFrequency;

                yield return new WaitForSeconds(scriptableModifier.ApplyFrequency);
            }
        }
        else
        {
            scriptableModifier.OnTick(ownerCharacter, targetCharacter, ref affectedCharacters);
            yield return new WaitForSeconds(scriptableModifier.ActivePeriod);
        }

        if (!isPassive)
            Remove(targetCharacter);
    }

    void onOwnerTakenDamage()
    {
        timeSinceTakenDamage = Time.time;
    }
}