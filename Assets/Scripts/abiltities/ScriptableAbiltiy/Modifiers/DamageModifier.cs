using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/DamageModifier")]
public class DamageModifier : BaseModifier
{
    public float Damage;

    public override void OnApply(CharacterBase characterBase, MonoBehaviour mono)
    {
        if (ActivePeriod <= 0.0f)
        {
            OnTick(characterBase);
            return;
        }

        mono.StartCoroutine(tickCoroutine(characterBase));
    }

    public override void OnRemove(CharacterBase characterBase)
    {

    }

    protected override void OnTick(CharacterBase characterBase)
    {
        //TODO add damage on tick
        Debug.Log($"DamageModifier: {ModifierName} applied {Damage} damage");
    }

    IEnumerator tickCoroutine(CharacterBase characterBase)
    {
        while (currentActiveTime <= ActivePeriod)
        {
            OnTick(characterBase);
            currentActiveTime += ApplyFrequency;
            yield return new WaitForSeconds(ApplyFrequency);
        }
    }
}