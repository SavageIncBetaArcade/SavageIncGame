using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAbilitiy : AttackAbility
{
    public float Damage;
    public float Range = 25.0f;
    public Transform RayOrigin;

    public override void Use()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(RayOrigin.position, RayOrigin.forward, out hitInfo, Range))
        {
            CharacterBase characterBase = hitInfo.transform.GetComponent<CharacterBase>();
            if (characterBase != null && characterBase != CharacterBase)
            {
                Hit(characterBase);
            }

        }


        Debug.Log("Attacking");
    }

    protected override void Hit(CharacterBase hitCharacter)
    {

        Debug.Log("Hit");
    }
}
