using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ModifierHandler
{
    private List<AbilityModifier> allModifiers;

    //cache stage modifiers
    private List<AbilityModifier> preModifiers;
    private List<AbilityModifier> actionModifiers;
    private List<AbilityModifier> postModifiers;

    public ModifierHandler(List<AbilityModifier> modifiers)
    {
        allModifiers = modifiers;
        preModifiers = new List<AbilityModifier>();
        actionModifiers = new List<AbilityModifier>();
        postModifiers = new List<AbilityModifier>();

        foreach (var modifier in modifiers)
        {
            switch (modifier.Stage)
            {
                case ModifierStage.PRE_ACTION:
                    preModifiers.Add(modifier);
                    break;
                case ModifierStage.ACTION:
                    actionModifiers.Add(modifier);
                    break;
                case ModifierStage.POST_ACTION:
                    postModifiers.Add(modifier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void ApplyPreActionModifiers(CharacterBase characterBase, CharacterBase targetCharacterBase)
    {
        foreach (var abilityModifier in preModifiers)
        {
            applyModifier(abilityModifier,characterBase,targetCharacterBase);
        }
    }

    public void ApplyActionModifiers(CharacterBase characterBase, GameObject targetObject, Vector3 hitPoint,
        Vector3 hitNormal)
    {
        if(actionModifiers.Count == 0)
            return;

        CharacterBase targetcharacter = targetObject.GetComponent<CharacterBase>();
        HashSet<CharacterBase> affectedCharacters = new HashSet<CharacterBase>();

        if (targetcharacter)
            affectedCharacters.Add(targetcharacter);

        Modifier mod = new Modifier(actionModifiers[0].Modifier, characterBase);
        mod.Hit(hitPoint, hitNormal, targetObject);
        if(targetcharacter != null)
            mod.Apply(targetcharacter);

        foreach (var character in mod.AffectedCharacters)
        {
            affectedCharacters.Add(character);
        }



        for (var modifierIndex = 1; modifierIndex < actionModifiers.Count; modifierIndex++)
        {
            var abilityModifier = actionModifiers[modifierIndex];
            int count = affectedCharacters.Count;
            for (int i = 0; i < count; i++)
            {
                Modifier modifier = applyModifier(abilityModifier, characterBase, affectedCharacters.ElementAt(i));
                modifier.Hit(hitPoint, hitNormal, affectedCharacters.ElementAt(i).gameObject);
                foreach (var character in modifier.AffectedCharacters)
                {
                    affectedCharacters.Add(character);
                }
            }
        }
    }

    public void ApplyPostActionModifiers(CharacterBase characterBase, CharacterBase targetCharacterBase)
    {
        foreach (var abilityModifier in postModifiers)
        {
            applyModifier(abilityModifier, characterBase, targetCharacterBase);
        }
    }

    private Modifier applyModifier(AbilityModifier abilityModifier, CharacterBase ownerCharacter,
        CharacterBase targetCharacter)
    {
        if(abilityModifier.Modifier == null)
            return null;

        //create a new instance of the modifier
        Modifier modifier = new Modifier(abilityModifier.Modifier, ownerCharacter);

        switch (abilityModifier.Target)
        {
            case ModifierTarget.CASTER:
                modifier.Apply(ownerCharacter);
                break;
            case ModifierTarget.TARGET:
                modifier.Apply(targetCharacter);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return modifier;
    }
}