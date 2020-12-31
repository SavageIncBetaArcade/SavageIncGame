using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseableAblity : UseableAbility
{
    public string UseButton = "Fire1";
    public LayerMask HandLayerMask;

    private bool used = false;
    protected override void Initilise()
    {
        base.Initilise();

        //SetLayerRecursively(worldGameObject, HandLayerMask.value);
    }

    protected virtual void Update()
    {
        //TODO check if the current CharacterBase is the player, only attack on left click if player


        if (!used && ScriptableAbility != null && !OnCooldown() && (Input.GetButtonDown(UseButton) || Input.GetAxisRaw(UseButton) > 0))
        {
            ExecuteUse();
            used = true;
        }

        if (Input.GetButtonUp(UseButton) || Input.GetAxisRaw(UseButton) <= float.Epsilon)
            used = false;
    }
}
