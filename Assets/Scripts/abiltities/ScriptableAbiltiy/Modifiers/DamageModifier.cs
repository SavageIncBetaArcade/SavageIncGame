using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/DamageModifier")]
public class DamageModifier : BaseModifier
{
    public float Damage;

    public override void OnApply(CharacterBase characterBase)
    {

    }

    public override void OnRemove(CharacterBase characterBase)
    {

    }

    protected override void OnTick(CharacterBase characterBase)
    {
        //TODO add damage on tick
        Debug.Log($"DamageModifier: {ModifierName} applied {Damage} damage");
    }
}