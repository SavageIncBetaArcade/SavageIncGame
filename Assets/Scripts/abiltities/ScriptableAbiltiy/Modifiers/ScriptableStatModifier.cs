using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/StatModifier")]
public class ScriptableStatModifier : ScriptableModifier
{
    public StatType Type;
    public float Amount;

    public override void OnApply(CharacterBase characterBase)
    {
        characterBase.ApplyStatModifier(Type, Amount);
    }

    public override void OnRemove(CharacterBase characterBase)
    {
        characterBase.ApplyStatModifier(Type, -Amount);
    }

    public override void OnTick(CharacterBase characterBase)
    {

    }
}