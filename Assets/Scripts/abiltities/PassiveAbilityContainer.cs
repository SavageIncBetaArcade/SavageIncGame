using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PassiveAbilityContainer : MonoBehaviour
{
    public ScriptablePassiveAbility[] PassiveAbilities;
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
}