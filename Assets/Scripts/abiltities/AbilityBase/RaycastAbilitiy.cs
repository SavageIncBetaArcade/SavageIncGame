using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAbilitiy : AttackAbility
{
    public RaycastAbilitiy(UseableAbility useableAbility) : base(useableAbility)
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
            if (hitCharacter != null && hitCharacter != useableAbility.CharacterBase)
            {
                Hit(hitCharacter);
            }

        }


        Debug.Log("Firing Raycast");
    }

    protected override void Hit(CharacterBase hitCharacter)
    {
        Debug.Log("Hit");

        base.Hit(hitCharacter);
    }
}
