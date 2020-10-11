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
    private Dictionary<ScriptableModifier,Modifier> activePassiveModifiers;

    public void Awake()
    {
        characterBase = GetComponent<CharacterBase>();
        activePassiveModifiers = new Dictionary<ScriptableModifier, Modifier>();
    }

    public void Start()
    {
        foreach (var modifier in StartingPassiveModifiers)
        {
            AddPassive(modifier);
        }
    }

    public void AddPassive(ScriptableModifier passiveModifier)
    {
        if (!activePassiveModifiers.ContainsKey(passiveModifier))
        {
            Modifier passive = new Modifier(passiveModifier);
            passive.IsPassive = true;
            passive.Apply(characterBase);
            activePassiveModifiers.Add(passiveModifier,passive);
        }
    }

    public bool RemovePassive(ScriptableModifier passiveModifier)
    {
        if (activePassiveModifiers.ContainsKey(passiveModifier))
        {
            activePassiveModifiers[passiveModifier].Remove(characterBase);
            activePassiveModifiers.Remove(passiveModifier);
            return true;
        }

        return false;
    }

    public bool RemovePassive(string passiveName)
    {
        ScriptableModifier modifierToRemove = null;
        foreach (var passiveModifier in activePassiveModifiers)
        {
            if (passiveModifier.Key.ModifierName == passiveName)
            {
                passiveModifier.Value.Remove(characterBase);
                modifierToRemove = passiveModifier.Key;
                break;
            }
        }

        if (modifierToRemove != null)
        {
            return activePassiveModifiers.Remove(modifierToRemove);
        }

        return false;
    }

    public void RemoveAll()
    {
        foreach (var passiveModifier in activePassiveModifiers)
        {
            passiveModifier.Value.Remove(characterBase);
        }
        activePassiveModifiers.Clear();
    }
}