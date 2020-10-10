using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// The purpose of the Modifier class is to allow each modifier to have thier own coroutine
/// ScriptableModifier is just a single instance and will not work because of this.
/// </summary>
[Serializable]
public class Modifier
{
    [SerializeField]
    private ScriptableModifier scriptableModifier;
    private Coroutine coroutine;

    #region properties
    public string ModifierName => scriptableModifier.ModifierName;
    public string ModifierDescription => scriptableModifier.ModifierDescription;
    public float ActivePeriod => scriptableModifier.ActivePeriod;
    public float ApplyFrequency => scriptableModifier.ApplyFrequency;
    #endregion

    public Modifier(ScriptableModifier modifier)
    {
        scriptableModifier = modifier;
    }

    public void Apply(CharacterBase characterBase)
    {
        if (scriptableModifier.ActivePeriod <= 0.0f)
        {
            scriptableModifier.OnTick(characterBase);
            return;
        }

        //Check if the modifier is not already applied, if so renew it
        if (!characterBase.AppliedAbilities.Contains(scriptableModifier))
        {
            coroutine = characterBase.StartCoroutine(tickCoroutine(characterBase));
            characterBase.AppliedAbilities.Add(scriptableModifier);
        }
        else
        {
            Renew(characterBase);
        }

        scriptableModifier.OnApply(characterBase);
    }


    public void Remove(CharacterBase characterBase)
    {
        characterBase.AppliedAbilities.Remove(scriptableModifier);
    }

    private void Renew(CharacterBase characterBase)
    {
        if(coroutine != null)
            characterBase.StopCoroutine(coroutine);
        coroutine = characterBase.StartCoroutine(tickCoroutine(characterBase));
    }

    private IEnumerator tickCoroutine(CharacterBase characterBase)
    {
        float currentActiveTime = 0.0f;
        while (currentActiveTime <= scriptableModifier.ActivePeriod)
        {
            UnityEngine.Debug.Log(currentActiveTime);
            scriptableModifier.OnTick(characterBase);
            currentActiveTime += scriptableModifier.ApplyFrequency;
            yield return new WaitForSeconds(scriptableModifier.ApplyFrequency);
        }

        scriptableModifier.OnRemove(characterBase);
    }
}