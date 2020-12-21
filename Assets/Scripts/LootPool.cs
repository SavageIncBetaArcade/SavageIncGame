using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPool : MonoBehaviour
{
    public ScriptableUseableAbility[] Items;

    public ScriptableUseableAbility GetRandomItem()
    {
        if(Items.Length > 0)
        {
            return Items[Random.Range(0, Items.Length)];
        }

        return null;
    }
}
