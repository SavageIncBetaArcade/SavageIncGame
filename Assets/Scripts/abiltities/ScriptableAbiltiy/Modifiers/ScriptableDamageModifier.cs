using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/DamageModifier")]
public class ScriptableDamageModifier : ScriptableModifier
{
    public float Damage;

    public override void OnApply(CharacterBase characterBase)
    {
        base.OnApply(characterBase);
    }

    public override void OnRemove(CharacterBase characterBase)
    {

    }

    protected override void OnTick(CharacterBase characterBase)
    {
        //TODO add damage on tick
        Debug.Log($"ScriptableDamageModifier: {ModifierName} applied {Damage} damage");
    }
}