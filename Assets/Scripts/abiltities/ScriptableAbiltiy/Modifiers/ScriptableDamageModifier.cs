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
    public bool Percentage = false;

    //TODO get the equiped base weapon in hand damage
    public bool AddBaseWeaponDamage = true;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, Vector3 hitDirection,
        Vector3 hitSurfaceNormal,
        GameObject hitObject,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        damage(targetCharacter);
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        damage(targetCharacter);
    }

    private void damage(CharacterBase targetCharacter)
    {
        ApplyEffects(targetCharacter);

        float damage = Percentage ? targetCharacter.MaxHealth * Damage : Damage;
        targetCharacter.TakeDamage(damage);
        Debug.Log($"ScriptableDamageModifier: {ModifierName} applied {damage} damage");
    }
}