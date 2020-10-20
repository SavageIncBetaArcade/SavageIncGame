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
    public bool Percentage;

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        affectedCharacters.Add(targetCharacter);
        if(!Percentage)
            targetCharacter.ApplyStatModifier(Type, Amount);
        else
        {
            float percentageAmount = targetCharacter.GetStatModifier(Type) * Amount;
            targetCharacter.StartCoroutine(
                targetCharacter.ApplyStatsModifierOverPeriod(Type, percentageAmount, ActivePeriod));
        }
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        affectedCharacters.Clear();
        if (!Percentage)
            targetCharacter.ApplyStatModifier(Type, -Amount);

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        ApplyEffects(targetCharacter);
    }
}