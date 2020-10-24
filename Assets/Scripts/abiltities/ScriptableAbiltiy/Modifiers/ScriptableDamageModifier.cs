using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/DamageModifier")]
public class ScriptableDamageModifier : ScriptableModifier
{
    public float Damage;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, Vector3 hitNormal,
        GameObject hitObject,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        ApplyEffects(targetCharacter);

        targetCharacter.TakeDamage(Damage);
        Debug.Log($"ScriptableDamageModifier: {ModifierName} applied {Damage} damage");
    }
}