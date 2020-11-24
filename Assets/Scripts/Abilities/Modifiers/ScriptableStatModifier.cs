using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/StatModifier")]
public class ScriptableStatModifier : ScriptableModifier
{
    public StatType Type;
    public float Amount;
    public bool Percentage;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, Vector3 hitDirection,
        Vector3 hitSurfaceNormal,
        GameObject hitObject,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        float appliedAmount = Amount;

        if(Percentage)
        {
            appliedAmount = targetCharacter.GetStatModifier(Type) * Amount;
        }

        characterInstanceData[targetCharacter] = appliedAmount;
        targetCharacter.ApplyStatModifier(Type,appliedAmount);

        ApplyEffects(targetCharacter);
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        if (characterInstanceData.ContainsKey(targetCharacter))
        {
            float appliedAmount = (float) characterInstanceData[targetCharacter];
            characterInstanceData.Remove(targetCharacter);

            targetCharacter.ApplyStatModifier(Type, -appliedAmount);
        }
    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {

    }
}