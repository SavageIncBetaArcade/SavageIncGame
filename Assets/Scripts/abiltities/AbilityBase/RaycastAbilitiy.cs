using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAbilitiy : AttackAbility
{
    public RaycastAbilitiy(UseableAbility useableAbility, CharacterBase ownerCharacter) : base(useableAbility, ownerCharacter)
    {

    }

    public override void Initilise()
    {

    }

    public override void Use()
    {
        ScriptableRaycastAbility raycastAbility = Ability as ScriptableRaycastAbility;
        if (raycastAbility == null)
        {
            Debug.LogError("RaycastAbility: ScriptableAbility is not of type ScriptableRaycastAbility returning out of Use method");
            return;
        }

        RaycastHit hitInfo;
        if (Portal.RaycastRecursive(useableAbility.Origin.position, useableAbility.Origin.forward, 8, out hitInfo, ((ScriptableRaycastAbility)Ability).Range))
        {
            fireBolt(raycastAbility, useableAbility.Origin.position, hitInfo.point);

            CharacterBase hitCharacter = hitInfo.transform.GetComponent<CharacterBase>();
            if (hitCharacter != null && hitCharacter != OwnerCharacter)
            {
                Hit(hitCharacter, raycastAbility.Damage);
            }
        }
        else
        {
            Vector3 target = useableAbility.Origin.position + (useableAbility.Origin.forward * raycastAbility.Range);
            fireBolt(raycastAbility, useableAbility.Origin.position, target);
        }


        Debug.Log("Firing Raycast");
    }

    public override void Hit(CharacterBase hitCharacter, float damage)
    {
        Debug.Log("Hit");

        base.Hit(hitCharacter, damage);
    }

    private void fireBolt(ScriptableRaycastAbility raycastAbility, Vector3 start, Vector3 end)
    {
        if (raycastAbility.RaycastBolt != null)
        {
            RaycastBolt bolt = useableAbility.InstantiateObject(raycastAbility.RaycastBolt.gameObject,
                useableAbility.Origin).GetComponent<RaycastBolt>();

            bolt.SetPoints(start, end);
        }
    }
}
