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
    [SerializeField]
    private bool isPassive = false;
    private Coroutine coroutine;

    #region properties
    public string ModifierName => scriptableModifier.ModifierName;
    public string ModifierDescription => scriptableModifier.ModifierDescription;
    public float ActivePeriod => scriptableModifier.ActivePeriod;
    public float ApplyFrequency => scriptableModifier.ApplyFrequency;

    public bool IsPassive
    {
        get => isPassive;
        set => isPassive = value;
    }

    #endregion

    public Modifier(ScriptableModifier modifier)
    {
        scriptableModifier = modifier;
    }

    public void Apply(CharacterBase characterBase)
    {
        if (!isPassive && scriptableModifier.ActivePeriod <= 0.0f)
        {
            scriptableModifier.OnApply(characterBase);
            scriptableModifier.OnTick(characterBase);
            Remove(characterBase);
            return;
        }

        if (!isPassive)
        {
            //Check if the modifier is not already applied, if so renew it
            if (!characterBase.AppliedAbilities.Contains(scriptableModifier))
            {
                scriptableModifier.OnApply(characterBase);
                coroutine = characterBase.StartCoroutine(tickCoroutine(characterBase));
                characterBase.AppliedAbilities.Add(scriptableModifier);
            }
            else
            {
                Renew(characterBase);
            }
        }
        else
        {
            scriptableModifier.OnApply(characterBase);
            if(coroutine == null)
                coroutine = characterBase.StartCoroutine(tickCoroutine(characterBase));
        }
    }


    public void Remove(CharacterBase characterBase)
    {
        scriptableModifier.OnRemove(characterBase);
        if (!isPassive)
            characterBase.AppliedAbilities.Remove(scriptableModifier);

        if (coroutine != null)
            characterBase.StopCoroutine(coroutine);
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

        if (ApplyFrequency > 0.0f)
        {
            while (currentActiveTime <= scriptableModifier.ActivePeriod)
            {
                UnityEngine.Debug.Log(currentActiveTime);
                scriptableModifier.OnTick(characterBase);

                //Only update current active time if the ability is not passive as passive abilites will continuously tick
                if(!isPassive)
                    currentActiveTime += scriptableModifier.ApplyFrequency;

                yield return new WaitForSeconds(scriptableModifier.ApplyFrequency);
            }
        }
        else
        {
            scriptableModifier.OnTick(characterBase);
            yield return new WaitForSeconds(scriptableModifier.ActivePeriod);
        }

        if (!isPassive)
            Remove(characterBase);
    }
}