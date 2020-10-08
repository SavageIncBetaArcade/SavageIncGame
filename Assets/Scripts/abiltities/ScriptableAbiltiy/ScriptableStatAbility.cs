using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/StatAbility")]
public class ScriptableStatAbility : ScriptableUseableAbility
{
    public StatType StatType;
    public int Modifier = 1;
    public float Period = 5.0f;
}
