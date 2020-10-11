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
    public List<ScriptableModifier> StartingPassiveModifiers;
    private CharacterBase characterBase;
    private List<Modifier> activePassiveModifiers;

    public void Awake()
    {
        characterBase = GetComponent<CharacterBase>();
        activePassiveModifiers = new List<Modifier>();
    }

    public void Start()
    {
        foreach (var modifier in StartingPassiveModifiers)
        {
            Modifier passive = new Modifier(modifier);
            passive.IsPassive = true;
            passive.Apply(characterBase);
            activePassiveModifiers.Add(passive);
        }
    }

    public void AddPassive(ScriptableModifier passiveModifier)
    {
        if (!StartingPassiveModifiers.Contains(passiveModifier))
        {
            Modifier passive = new Modifier(passiveModifier);
            passive.IsPassive = true;
            passive.Apply(characterBase);
            StartingPassiveModifiers.Add(passiveModifier);
        }
    }
}