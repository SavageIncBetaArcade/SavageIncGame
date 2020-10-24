using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityModifiers/ChainDamageModifier")]
public class ScriptableChainDamageModifier : ScriptableDamageModifier
{
    public int MaxTarget = 3;
    public float Range = 5.0f;
    public float Delay = 0.025f;
    public float LifeTime = 0.5f;

    public Material ElectricMaterial;
    public GameObject LineGameObject;
    private CharacterBase[] allCharacters;

    public override void OnHit(CharacterBase ownerCharacter, Vector3 hitPosition, ref List<CharacterBase> affectedCharacters)
    {
        
    }

    public override void OnApply(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {
        allCharacters = GetOrderedCharactersByPosition(targetCharacter.transform.position);
        affectedCharacters.Add(targetCharacter);
        AddClosestCharacters(ownerCharacter,targetCharacter, ref affectedCharacters);
        targetCharacter.StartCoroutine(ChainAttack(affectedCharacters));
    }


    public override void OnRemove(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    public override void OnTick(CharacterBase ownerCharacter, CharacterBase targetCharacter,
        ref List<CharacterBase> affectedCharacters)
    {

    }

    private void AddClosestCharacters(CharacterBase owner, CharacterBase targetCharacter, ref List<CharacterBase> affectedCharacters)
    {
        if (affectedCharacters.Count >= MaxTarget)
            return;
        
        var orderedCharacters = GetOrderedCharactersByPosition(targetCharacter.transform.position);

        foreach (var orderedCharacter in orderedCharacters)
        {
            if (orderedCharacter != owner && orderedCharacter != targetCharacter && !affectedCharacters.Contains(orderedCharacter))
            {
                float distance =
                    Vector3.Distance(targetCharacter.transform.position, orderedCharacter.transform.position);

                if(distance > Range)
                    break;

                affectedCharacters.Add(orderedCharacter);
                AddClosestCharacters(owner,orderedCharacter, ref affectedCharacters);
                break;
            }
        }
    }

    private IEnumerator ChainAttack(List<CharacterBase> affectedCharacters)
    {
        List<GameObject> bolts = new List<GameObject>();
        for (var i = 0; i < affectedCharacters.Count; i++)
        {
            if (i < affectedCharacters.Count-1)
            {
                var affectedCharacter = affectedCharacters[i];
                Transform start = affectedCharacter.transform;
                Transform end = affectedCharacters[i + 1].transform;

                GameObject gameObject = Instantiate(LineGameObject, start.position, start.rotation);
                bolts.Add(gameObject);
                RaycastBolt raycastBolt = gameObject.GetComponent<RaycastBolt>();
                raycastBolt.LifeTime = -1.0f;
                raycastBolt.TimeToTarget = Delay;
                raycastBolt.SetPoints(start.position , end.position);
            }


            affectedCharacters[i].TakeDamage(Damage);
            ApplyEffects(affectedCharacters[i]);
            yield return new WaitForSeconds(Delay);
        }


        yield return new WaitForSeconds(LifeTime);
        foreach (var affectedCharacter in affectedCharacters)
        {
            if (ElectricMaterial != null)
            {
                var meshRenderer = affectedCharacter.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    List<Material> matArray = meshRenderer.materials.ToList();
                    matArray.RemoveAt(1);
                    meshRenderer.materials = matArray.ToArray();
                }
            }
        }

        for (int i = 0; i < bolts.Count; i++)
        {
            Destroy(bolts[i]);
        }
    }

    protected override void ApplyEffects(CharacterBase targetCharacter)
    {
        base.ApplyEffects(targetCharacter);

        if (ElectricMaterial != null)
        {

            var meshRenderer = targetCharacter.gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                List<Material> matArray = meshRenderer.materials.ToList();
                matArray.Add(ElectricMaterial); 
                meshRenderer.materials = matArray.ToArray();
            }
        }
    }
}
