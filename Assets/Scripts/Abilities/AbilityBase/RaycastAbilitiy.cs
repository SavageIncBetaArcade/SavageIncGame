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
            shootBolt(raycastAbility, useableAbility.Origin.position, hitInfo.point);

            //CharacterBase hitObject = hitInfo.transform.GetComponent<CharacterBase>();
            Hit(hitInfo.transform.gameObject, raycastAbility.Damage, hitInfo.point, useableAbility.Origin.forward, hitInfo.normal);
        }
        else
        {
            Vector3 target = useableAbility.Origin.position + (useableAbility.Origin.forward * raycastAbility.Range);
            shootBolt(raycastAbility, useableAbility.Origin.position, target);
        }


        Debug.Log("Firing Raycast");
    }

    public override void Hit(GameObject hitObject, float damage, Vector3 hitPoint, Vector3 hitDirection,
        Vector3 surfaceNormal)
    {
        Debug.Log("Hit");

        base.Hit(hitObject, damage, hitPoint, hitDirection, surfaceNormal);
    }

    private void shootBolt(ScriptableRaycastAbility raycastAbility, Vector3 start, Vector3 end)
    {
        if (raycastAbility.RaycastBolt != null)
        {
            RaycastBolt bolt = useableAbility.InstantiateObject(raycastAbility.RaycastBolt.gameObject,
                useableAbility.Origin.position).GetComponent<RaycastBolt>();

            bolt.SetPoints(start, end);
        }
    }
}
