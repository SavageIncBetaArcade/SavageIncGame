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

    }

    public override void OnRemove(CharacterBase characterBase)
    {

    }

    public override void OnTick(CharacterBase characterBase)
    {
        ApplyEffects(characterBase);

        //TODO add damage on tick
        Debug.Log($"ScriptableDamageModifier: {ModifierName} applied {Damage} damage");
    }
}