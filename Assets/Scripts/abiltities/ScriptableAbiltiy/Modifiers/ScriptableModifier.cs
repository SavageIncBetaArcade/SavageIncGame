using System;
using System.Collections;
using System.Collections.Generic;
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

    #region properties
    public string ModifierName => modifierName;
    public string ModifierDescription => modifierDescription;
    public float ActivePeriod => activePeriod;
    public float ApplyFrequency => applyFrequency;
    #endregion

    public abstract void OnApply(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters);
    public abstract void OnRemove(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters);
    public abstract void OnTick(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters);

    protected void ApplyEffects(CharacterBase targetCharacter)
    {
        foreach (var effect in tickEffectGameObjects)
        {
            Instantiate(effect, targetCharacter.transform.position, targetCharacter.transform.rotation);
        }
    }

    protected CharacterBase[] GetAllCharacters()
    {
        return FindObjectsOfType<CharacterBase>();
    }
}

[Serializable]
public struct AbilityModifier
{
    public Modifier Modifier;
    public ModifierStage Stage;
    public ModifierTarget Target;
}