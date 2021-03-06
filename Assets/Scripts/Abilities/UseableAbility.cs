﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This is the script that allows the character to use abilities
/// The ScriptableUseableAbility that is attached to the ScriptableAbility is the ability that would get called
/// </summary>
public abstract class UseableAbility : MonoBehaviour
{
    //Abilities that use mono behavior are all useable I.E (Weapons, Buffs) - Passive abilities work in a different way
    public ScriptableUseableAbility ScriptableAbility;
    public string AnimationUseBoolName;
    public string AnimationRangeUseBoolName;
    public Animator UseAnimator;
    public AudioSource UseAudioSource;

    //TODO pass in the attackers character base
    public delegate void UseAction();
    public event UseAction OnUse;

    //TODO get character base on awake
    public CharacterBase CharacterBase;
    public Transform Origin;

    public List<AbilityModifier> Modifiers;

    private BaseAbility ability;
    protected GameObject worldGameObject;

    private ModifierHandler modifierHandler;
    private Coroutine useCoroutine;

    private Dictionary<ScriptableUseableAbility, float> lastUseDictionary;

    public Dictionary<ScriptableUseableAbility, float> LastUseDictionary => lastUseDictionary;


    protected virtual void Initilise()
    {
        if (ScriptableAbility)
        {
            worldGameObject = Instantiate(ScriptableAbility.AbilityPrefab, transform.position, transform.rotation,
                transform);

                SetLayerRecursively(worldGameObject,gameObject.layer);

            ability = AbilityFactory.Create(this, ScriptableAbility, CharacterBase, worldGameObject, HitAction,
                AttackEnded);

            //Create a copy of the Modifiers so this ability has its own instance
            modifierHandler = new ModifierHandler(Modifiers);
        }
    }

    public void SetAbility(WeaponItem weapon, ScriptableUseableAbility newAbility)
    {
        ScriptableAbility = newAbility;
        Modifiers.Clear();

        if(weapon)
        {
            //override the modifiers to add the attack bonus 
            var attackModifier = ScriptableObject.CreateInstance<ScriptableStatModifier>();
            attackModifier.Type = StatType.ATTACK;
            attackModifier.Amount = weapon.attack;

            var AbilityModifier = new AbilityModifier();
            AbilityModifier.Modifier = attackModifier;
            AbilityModifier.Stage = ModifierStage.ACTION;
            AbilityModifier.Target = ModifierTarget.CASTER;

            Modifiers.Insert(0, AbilityModifier);
        }

        if (worldGameObject)
            Destroy(worldGameObject);

        Initilise();
    }

    protected virtual void Awake()
    {
        lastUseDictionary = new Dictionary<ScriptableUseableAbility, float>();
        if (ScriptableAbility != null && ScriptableAbility.AbilityPrefab != null)
        {
            Initilise();
        }
        else
        {
            Debug.LogWarning("Base ability doesn't have a ability or an ability prefab");
        }

        if (CharacterBase == null)
            Debug.LogWarning("Base ability doesn't have a character base, please make sure to set it");

    }

    protected bool OnCooldown()
    {
        if (ScriptableAbility.UseAnimationCooldown && UseAnimator != null && !string.IsNullOrWhiteSpace(AnimationUseBoolName))
        {
            return UseAnimator.GetBool(AnimationUseBoolName);
        }

        if (lastUseDictionary.ContainsKey(ScriptableAbility))
        {
            float lastUseTime = lastUseDictionary[ScriptableAbility];
            return !(Time.time >= lastUseTime + ScriptableAbility.Cooldown);
        }

        return false;
    }

    protected void ExecuteUse()
    {
        if (!IsValid())
            return;

        UseAudioSource?.PlayOneShot(ScriptableAbility.UseSound);

        if (useCoroutine != null)
            StopCoroutine(useCoroutine);
        useCoroutine = StartCoroutine(UseCoroutine());

        lastUseDictionary[ScriptableAbility] = Time.time;
    }

    private IEnumerator UseCoroutine()
    {
        float currentActiveTime = 0.0f;

        //check if the ability has a period, if so use the use frequency to use the ability multiple times till the period is over
        do
        {
            modifierHandler.ApplyPreActionModifiers(CharacterBase, CharacterBase);

            OnUse?.Invoke();

            if (UseAnimator != null)
            {
                switch (ability.Ability)
                {
                    case ScriptableMeleeAbility scriptableMeleeAbility:
                        UseAnimator.SetBool(AnimationUseBoolName, true);
                        break;
                    case ScriptableProjectileAbility scriptableProjectileAbility:
                        UseAnimator.SetBool(AnimationRangeUseBoolName, true);
                        break;
                    case ScriptableRaycastAbility scriptableRaycastAbility:
                        UseAnimator.SetBool(AnimationRangeUseBoolName, true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }

            ability.Use();
            CharacterBase.LoseEnergy(ScriptableAbility.EnergyCost);

            modifierHandler.ApplyPostActionModifiers(CharacterBase, CharacterBase);

            if(ability.Ability.ActivePeriod <= 0.0f)
                break;

            yield return new WaitForSeconds(ability.Ability.UseFrequency);
            currentActiveTime += ability.Ability.UseFrequency;
        }
        while (currentActiveTime < ability.Ability.ActivePeriod);
    }

    public GameObject InstantiateObject(GameObject gameObject, Vector3 positon, Quaternion rotation)
    {
        return Instantiate(gameObject, positon, rotation);
    }

    private void HitAction(CharacterBase attackingcharacter, GameObject targetObject, Vector3 hitPoint, Vector3 hitDirection,
        Vector3 hitSurfaceNormal)
    {
        if(ScriptableAbility.HitModifierApplyPercentage >= Random.Range(0.0f,1.0f))
            modifierHandler.ApplyActionModifiers(attackingcharacter, targetObject, hitPoint, hitDirection, hitSurfaceNormal);
    }

    private void AttackEnded(CharacterBase attackingCharacter)
    {
        modifierHandler.RemoveInstantModifiers();
    }

    private bool IsValid()
    {
        return ability != null && ScriptableAbility && !CharacterBase.IsStunned && Time.timeScale > 0.0f && CharacterBase.CurrentEnergy >= ScriptableAbility.EnergyCost;
    }

    private void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
