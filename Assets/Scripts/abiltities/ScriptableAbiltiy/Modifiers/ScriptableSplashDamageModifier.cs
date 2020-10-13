using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/SplashDamageModifier")]
public class ScriptableSplashDamageModifier : ScriptableDamageModifier
{
    public float Radius = 5.0f;

    private List<CharacterBase> affectedCharacters;

    public override void OnApply(CharacterBase characterBase)
    {
        affectedCharacters = new List<CharacterBase>();
        //Get all characters within radius of the hit character
        foreach (var character in GetAllCharacters())
        {
            if (Vector3.Distance(character.transform.position, characterBase.transform.position) <= Radius)
            {
                affectedCharacters.Add(character);
            }

        }
    }

    public override void OnRemove(CharacterBase characterBase)
    {

    }

    public override void OnTick(CharacterBase characterBase)
    {
        foreach (var character in affectedCharacters)
        {
            ApplyEffects(character);
            //TODO apply damage
            Debug.Log($"Applied '{ModifierName}' dealing: {Damage} splash damage to {character.gameObject.name}");
        }
    }
}