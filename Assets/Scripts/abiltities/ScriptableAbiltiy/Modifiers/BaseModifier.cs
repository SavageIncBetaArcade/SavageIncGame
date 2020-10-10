using System;
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

public abstract class BaseModifier : ScriptableObject
{
    [SerializeField]
    protected string ModifierName;
    protected string ModifierDescription;

    /// <summary>
    /// How Long is the modifier active for till it gets removed from the target?
    /// ActivePeriod = 0 means the Modifier is only affected for a single frame (E.G instant damage)
    /// </summary>
    protected float ActivePeriod = 0.0f;

    public abstract void Apply(CharacterBase characterBase);
    public abstract void Remove(CharacterBase characterBase);
}

[Serializable]
public struct AbilityModifier
{
    public BaseModifier Modifier;
    public ModifierStage Stage;
    public ModifierTarget Target;
}