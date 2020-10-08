using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This script manages the passive abilities of the characterbase that is on the SAME gameobject
/// </summary>
public class PassiveAbilityContainer : MonoBehaviour
{
    public List<ScriptablePassiveAbility> PassiveAbilities;
    private CharacterBase characterBase;

    public void Awake()
    {
        characterBase = GetComponent<CharacterBase>();
    }

    public void Start()
    {
        foreach (var ability in PassiveAbilities)
        {
            ability.Apply(characterBase);
        }
    }

    public void AddPassive(ScriptablePassiveAbility passiveAbility)
    {
        if (!PassiveAbilities.Contains(passiveAbility))
        {
            passiveAbility.Apply(characterBase);
            PassiveAbilities.Add(passiveAbility);
        }
    }
}