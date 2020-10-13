using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/ChainDamageModifier")]
public class ScriptableChainDamageModifier : ScriptableDamageModifier
{
    public int MaxTarget = 3;
    public float Range = 5.0f;
    public float Delay = 0.0f;

    private CharacterBase[] allCharacters;

    public override void OnApply(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        allCharacters = GetOrderedCharactersByPosition(targetCharacter);
        affectedCharacters.Add(targetCharacter);
        AddClosestCharacters(targetCharacter, ref affectedCharacters);
    }


    public override void OnRemove(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        //affectedCharacters.Clear();
    }

    public override void OnTick(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        targetCharacter.StartCoroutine(ChainAttack(affectedCharacters));
    }

    private void AddClosestCharacters(CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        if (affectedCharacters.Count >= MaxTarget)
            return;
        
        var orderedCharacters = GetOrderedCharactersByPosition(targetCharacter);

        foreach (var orderedCharacter in orderedCharacters)
        {
            if (orderedCharacter != targetCharacter && !affectedCharacters.Contains(orderedCharacter))
            {
                float distance =
                    Vector3.Distance(targetCharacter.transform.position, orderedCharacter.transform.position);

                if(distance > Range)
                    break;

                affectedCharacters.Add(orderedCharacter);
                AddClosestCharacters(orderedCharacter, ref affectedCharacters);
                break;
            }
        }
    }

    private IEnumerator ChainAttack(List<CharacterBase> affectedCharacters)
    {
        foreach (var affectedCharacter in affectedCharacters)
        {
            ApplyEffects(affectedCharacter);
            yield return new WaitForSeconds(Delay);
        }
    }
}
