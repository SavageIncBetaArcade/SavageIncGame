﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ModifierStage
{
    PRE_ACTION,
    ACTION,
    POST_ACTION
}

public enum ModifierTarget
{
    CASTER,
    TARGET
}

public abstract class ScriptableModifier : ScriptableObject
{
    [SerializeField]
    protected string modifierName;
    [SerializeField]
    protected string modifierDescription;
    [SerializeField] 
    protected int modifierLevel = 1;

    /// <summary>
    /// How Long is the modifier active for till it gets removed from the target?
    /// activePeriod = 0 means the Modifier is only affected for a single frame (E.G instant damage)
    /// </summary>
    [SerializeField]
    protected float activePeriod = 0.0f;

    /// <summary>
    /// How many times to apply the modifier effect?
    /// applyFrequency = 0.0f means the effect only gets applied once
    /// applyFrequency = 0.5f means the effect gets applied every 0.5 seconds while the effective is active
    /// </summary>
    [SerializeField]
    protected float applyFrequency = 0.0f;

    [SerializeField]
    protected GameObject[] tickEffectGameObjects;

    //Have a dictionary to hold per character instance data
    protected Dictionary<CharacterBase, object> characterInstanceData;

    #region properties
    public string ModifierName => modifierName;
    public string ModifierDescription => modifierDescription;
    public float ActivePeriod => activePeriod;
    public float ApplyFrequency => applyFrequency;
    #endregion

    protected ScriptableModifier()
    {
        characterInstanceData = new Dictionary<CharacterBase, object>();
    }

    public abstract void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition,
        Vector3 hitDirection, Vector3 hitSurfaceNormal,
        GameObject hitObject,
        ref List<CharacterBase> affectedCharacters);
    public abstract void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters);
    public abstract void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters);
    public abstract void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters);

    protected virtual void ApplyEffects(CharacterBase targetCharacter)
    {
        if (tickEffectGameObjects == null || !tickEffectGameObjects.Any())
            return;

        foreach (var effect in tickEffectGameObjects)
        {
            if(effect != null)
                Instantiate(effect, targetCharacter.transform.position, targetCharacter.transform.rotation);
        }
    }

    protected CharacterBase[] GetAllCharacters()
    {
        return FindObjectsOfType<CharacterBase>();
    }

    protected CharacterBase[] GetOrderedCharactersByPosition(Vector3 position)
    {
        return GetAllCharacters().OrderBy(x => Vector3.Distance(position,x.transform.position)).ToArray();
    }
}

[Serializable]
public struct AbilityModifier
{
    public ScriptableModifier Modifier;
    public ModifierStage Stage;
    public ModifierTarget Target;
}