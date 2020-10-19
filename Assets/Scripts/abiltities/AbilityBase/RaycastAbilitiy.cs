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
        RaycastHit hitInfo;
        if (Physics.Raycast(useableAbility.Origin.position, useableAbility.Origin.forward, out hitInfo, ((ScriptableRaycastAbility)Ability).Range))
        {
            CharacterBase hitCharacter = hitInfo.transform.GetComponent<CharacterBase>();
            if (hitCharacter != null && hitCharacter != OwnerCharacter)
            {
                ScriptableRaycastAbility raycastAbility = Ability as ScriptableRaycastAbility;
                if (raycastAbility == null)
                    Debug.LogError("RaycastAbility: ScriptableAbility is not of type ScriptableRaycastAbility");

                Hit(hitCharacter, raycastAbility != null ? raycastAbility.Damage : 0.0f);
            }

        }


        Debug.Log("Firing Raycast");
    }

    public override void Hit(CharacterBase hitCharacter, float damage)
    {
        Debug.Log("Hit");

        base.Hit(hitCharacter, damage);
    }
}
