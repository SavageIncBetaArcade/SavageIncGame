using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/ForceModifier")]
class ScriptableForceModifier : ScriptableModifier
{
    public enum ForceModifierType
    {
        WORLD_UP,
        WORLD_FORWARD,
        WORLD_RIGHT,
        LOCAL_UP,
        LOCAL_FORWARD,
        LOCAL_RIGHT,
        HIT_DIRECTION,
        HIT_SURFACE_NORMAL
    }

    public ForceModifierType ForceType;
    public float ForceToApply = 5.0f;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, Vector3 hitDirection,
        Vector3 hitSurfaceNormal,
        GameObject hitObject,
        ref List<CharacterBase> affectedCharacters)
    {
        //First check if the hitObject has a rigid body
        Rigidbody hitRigidbody = hitObject.GetComponent<Rigidbody>();

        if(hitRigidbody == null)
            return;

        Vector3 force = getForce(ForceType, hitObject, hitDirection, hitSurfaceNormal);
        hitRigidbody.AddForce(force, ForceMode.Impulse);

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

    private Vector3 getForce(ForceModifierType forceType, GameObject gameObject, Vector3 hitDirection, Vector3 hitSurfaceNormal)
    {
        Vector3 force;

        switch (forceType)
        {
            case ForceModifierType.WORLD_UP:
                force = Vector3.up;
                break;
            case ForceModifierType.WORLD_FORWARD:
                force = Vector3.forward;
                break;
            case ForceModifierType.WORLD_RIGHT:
                force = Vector3.right;
                break;
            case ForceModifierType.LOCAL_UP:
                force = gameObject.transform.up;
                break;
            case ForceModifierType.LOCAL_FORWARD:
                force = gameObject.transform.up;
                break;
            case ForceModifierType.LOCAL_RIGHT:
                force = gameObject.transform.right;
                break;
            case ForceModifierType.HIT_DIRECTION:
                force = hitDirection;
                break;
            case ForceModifierType.HIT_SURFACE_NORMAL:
                force = -hitSurfaceNormal;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(forceType), forceType, null);
        }
        return force * ForceToApply;
    }
}