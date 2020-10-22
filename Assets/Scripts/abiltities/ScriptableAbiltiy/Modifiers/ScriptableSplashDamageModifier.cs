using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/SplashDamageModifier")]
public class ScriptableSplashDamageModifier : ScriptableDamageModifier
{
    public float Radius = 5.0f;

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        //Get all characters within radius of the hit character
        foreach (var character in GetAllCharacters())
        {
            if(character == ownerCharacter)
                continue;
            
            if (Vector3.Distance(character.transform.position, targetCharacter.transform.position) <= Radius)
            {
                affectedCharacters.Add(character);
            }

        }

        foreach (var character in affectedCharacters)
        {
            targetCharacter.TakeDamage(Damage);
            Debug.Log($"Applied '{ModifierName}' dealing: {Damage} splash damage to {character.gameObject.name}");

            ApplyEffects(character);
        }
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        
    }
}