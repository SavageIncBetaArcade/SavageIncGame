using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUseableAbilitiy : UseableAbility
{
    public bool IgnoreCooldown = false;

    public void Attack()
    {
        if (ScriptableAbility != null && (IgnoreCooldown || !OnCooldown()))
        {
            ExecuteUse();
        }
    }
}
