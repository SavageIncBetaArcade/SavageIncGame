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

    private List<Tuple<Modifier,CharacterBase>> appliedInstantModifiers;

    public ModifierHandler(List<AbilityModifier> modifiers)
    {
        allModifiers = modifiers;
        preModifiers = new List<AbilityModifier>();
        actionModifiers = new List<AbilityModifier>();
        postModifiers = new List<AbilityModifier>();
        appliedInstantModifiers = new List<Tuple<Modifier, CharacterBase>>();

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

        RemoveInstantModifiers();
    }

    public void ApplyActionModifiers(CharacterBase characterBase, GameObject targetObject, Vector3 hitPoint,
        Vector3 hitDirection, Vector3 hitSurfaceNormal)
    {
        if(actionModifiers.Count == 0)
            return;

        CharacterBase targetcharacter = targetObject.GetComponent<CharacterBase>();
        HashSet<CharacterBase> affectedCharacters = new HashSet<CharacterBase>();

        if (targetcharacter)
            affectedCharacters.Add(targetcharacter);

        Modifier mod = new Modifier(actionModifiers[0].Modifier, characterBase);
        mod.Hit(hitPoint, hitDirection, hitSurfaceNormal, targetObject);
        if (targetcharacter != null)
        {
            mod.Apply(targetcharacter);

            if (mod.ActivePeriod <= 0.0f)
                appliedInstantModifiers.Add(Tuple.Create(mod, targetcharacter));
        }


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
                modifier.Hit(hitPoint, hitDirection, hitSurfaceNormal, affectedCharacters.ElementAt(i).gameObject);
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

        RemoveInstantModifiers();
    }

    public void RemoveInstantModifiers()
    {
        foreach (var instantModifier in appliedInstantModifiers)
        {
            instantModifier.Item1.Remove(instantModifier.Item2);
        }
        appliedInstantModifiers.Clear();
    }

    private Modifier applyModifier(AbilityModifier abilityModifier, CharacterBase ownerCharacter,
        CharacterBase targetCharacter)
    {
        if(abilityModifier.Modifier == null)
            return null;

        //create a new instance of the modifier
        Modifier modifier = new Modifier(abilityModifier.Modifier, ownerCharacter);
        modifier.Apply(targetCharacter);


        if (abilityModifier.Modifier.ActivePeriod <= 0.0f)
            appliedInstantModifiers.Add(Tuple.Create(modifier, targetCharacter));

        return modifier;
    }
}