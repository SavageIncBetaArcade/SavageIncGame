using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MeleeAbility")]
public class ScriptableMeleeAbility : ScriptableAttackAbility
{
    public float Damage = 5;

    private MeleeAbility meleeAbilityTrigger;

    public override void Initialize(GameObject hand)
    {
        meleeAbilityTrigger = hand.GetComponent<MeleeAbility>();
    }

    public override void UseAbility()
    {
        if (meleeAbilityTrigger != null)
        {
            Debug.Log("Attacking");
        }
    }


    public override void Attack(CharacterBase targetCharacter)
    {
        //TODO implement damage call
        Debug.Log("Hit");
    }

}