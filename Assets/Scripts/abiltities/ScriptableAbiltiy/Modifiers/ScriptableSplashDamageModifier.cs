using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/SplashDamageModifier")]
public class ScriptableSplashDamageModifier : ScriptableDamageModifier
{
    public float Radius = 5.0f;

    public override void OnApply(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        //Get all characters within radius of the hit character
        foreach (var character in GetAllCharacters())
        {
            if (Vector3.Distance(character.transform.position, targetCharacter.transform.position) <= Radius)
            {
                affectedCharacters.Add(character);
            }

        }
    }

    public override void OnRemove(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        affectedCharacters.Clear();
    }

    public override void OnTick(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        foreach (var character in affectedCharacters)
        {
            ApplyEffects(character);
            //TODO apply damage
            Debug.Log($"Applied '{ModifierName}' dealing: {Damage} splash damage to {character.gameObject.name}");
        }
    }
}