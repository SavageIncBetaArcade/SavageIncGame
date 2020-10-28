using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/EnergyModifier")]
public class ScriptableEnergyModifier : ScriptableModifier
{
    public float Amount = 5.0f;
    public bool Percentage = false;
    public bool ReplenishOwner = false;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, Vector3 hitDirection,
        Vector3 hitSurfaceNormal,
        GameObject hitObject,
        ref List<CharacterBase> affectedCharacters)
    {
        throw new NotImplementedException();
    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        CharacterBase target = ReplenishOwner ? ownerCharacter : targetCharacter;
        float amount = Percentage ? target.MaxEnergy * Amount : Amount;
        target.ReplenishEnergy(amount);
        ApplyEffects(target);
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {

    }
}

