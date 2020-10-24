using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/HealthModifier")]
public class ScriptableHealModifier : ScriptableModifier
{
    public float Amount = 5.0f;
    public bool Percentage = false;
    public bool HealOwner = false;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, ref List<CharacterBase> affectedCharacters)
    {
        throw new NotImplementedException();
    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        CharacterBase target = HealOwner ? ownerCharacter : targetCharacter;
        float amount = Percentage ? target.MaxHealth * Amount : Amount;
        target.Heal(amount);
        ApplyEffects(target);
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {

    }
}

