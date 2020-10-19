using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/StatModifier")]
public class ScriptableStatModifier : ScriptableModifier
{
    public StatType Type;
    public float Amount;

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        affectedCharacters.Add(targetCharacter);
        targetCharacter.ApplyStatModifier(Type, Amount);
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        affectedCharacters.Clear();
        targetCharacter.ApplyStatModifier(Type, -Amount);
    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        ApplyEffects(targetCharacter);
    }
}