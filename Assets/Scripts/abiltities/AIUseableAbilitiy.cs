using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUseableAbilitiy : UseableAbility
{
    public void Attack()
    {
        if (ScriptableAbility != null && !OnCooldown())
        {
            ExecuteUse();
        }
    }
}
