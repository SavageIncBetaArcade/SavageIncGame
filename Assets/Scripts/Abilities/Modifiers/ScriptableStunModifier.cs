using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/StunModifier")]
class ScriptableStunModifier : ScriptableModifier
{
    public float StunTime = 1.0f;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, Vector3 hitDirection, Vector3 hitSurfaceNormal,
        GameObject hitObject, ref List<CharacterBase> affectedCharacters)
    {
        
    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        targetCharacter.CurrentStunTime += StunTime;
    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        
    }
}
