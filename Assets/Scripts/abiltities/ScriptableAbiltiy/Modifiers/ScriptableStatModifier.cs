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

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, Vector3 hitNormal,
        GameObject hitObject,
        ref List<CharacterBase> affectedCharacters)
    {
        throw new NotImplementedException();
    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        if(!Percentage)
            targetCharacter.ApplyStatModifier(Type, Amount);
        else
        {
            float percentageAmount = targetCharacter.GetStatModifier(Type) * Amount;
            targetCharacter.StartCoroutine(
                targetCharacter.ApplyStatsModifierOverPeriod(Type, percentageAmount, ActivePeriod));
        }

        ApplyEffects(targetCharacter);
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        if (!Percentage)
            targetCharacter.ApplyStatModifier(Type, -Amount);

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {

    }
}