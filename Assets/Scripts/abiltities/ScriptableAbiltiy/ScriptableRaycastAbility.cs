using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/RaycastAbility")]
public class ScriptableRaycastAbility : ScriptableUseableAbility
{
    public float Damage = 5.0f;
    public float Range = 25.0f;
}
