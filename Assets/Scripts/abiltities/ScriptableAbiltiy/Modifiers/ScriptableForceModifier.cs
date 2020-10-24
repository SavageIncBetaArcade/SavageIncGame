using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/ForceModifier")]
class ScriptableForceModifier : ScriptableModifier
{
    public Vector3 ForceToApply;
    public bool ApplyLocalForce = false;

    public float ForceToApplyOnNormal = 0.0f;
    public bool ApplyForceToNormal = false;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, Vector3 hitNormal,
        GameObject hitObject,
        ref List<CharacterBase> affectedCharacters)
    {
        //First check if the hitObject has a rigid body
        Rigidbody hitRigidbody = hitObject.GetComponent<Rigidbody>();

        if(hitRigidbody == null)
            return;

        Vector3 force = ForceToApply;
        if (ApplyLocalForce)
            force = hitObject.transform.TransformDirection(ForceToApply);
        if (ApplyForceToNormal)
            force = -hitNormal * ForceToApplyOnNormal;

        hitRigidbody.AddForce(force,ForceMode.Impulse);
    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {

    }
}