using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseableAblity : UseableAbility
{
    public string UseButton = "Fire1";

    protected virtual void Update()
    {
        //TODO check if the current CharacterBase is the player, only attack on left click if player
        if (ScriptableAbility != null && !OnCooldown() && (Input.GetButtonDown(UseButton) || Input.GetAxisRaw(UseButton) > 0))
        {
            ExecuteUse();
        }
    }
}
