﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    ATTACK,
    DEFENSE,
    HEALTH,
    ENERGY,
    SPEED,
    JUMP_HEIGHT
}

[RequireComponent(typeof(PortalableObject))]
[RequireComponent(typeof(UUID))]
public class CharacterBase : MonoBehaviour, IDamageTaker, IDataPersistance
{
    #region members
    [SerializeField] 
    private float attackModifier, defenseModifier, maxHealth, maxEnergy;
    [SerializeField]
    private float speed = 6.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float currentHealth, currentEnergy;
    [SerializeField] 
    private float currentStunTime = 0.0f;

    protected bool onGround;

    [SerializeField]
    private AudioClip[] HitSounds;
    [SerializeField]
    private AudioClip[] FootstepSounds;

    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private string SpeedParamaterName;
    private Vector3 lastPosition;

    [SerializeField]
    private AudioSource CharacterTravelAudioSource;
    [SerializeField]
    private float PlayTravelAudioDistance = 0.75f; //distance needed to travel to play travel sound (footsteps)
    private Vector3 lastTravelSoundPlayed;

    protected AudioSource CharacterAudio;

    public delegate void DeathAction();
    public event DeathAction OnDeath;
    public delegate void DamageAction();
    public event DamageAction OnDamage;
    public delegate void HealAction();
    public event HealAction OnHeal;
    public delegate void MaxHealthChange();
    public event MaxHealthChange OnMaxHealthChange;
    public delegate void ReplenishEnergyAction();
    public event ReplenishEnergyAction OnReplenishEnergy;
    public delegate void LoseEnergyAction();
    public event LoseEnergyAction OnLoseEnergy;
    public delegate void MaxEnergyChange();
    public event MaxEnergyChange OnMaxEnergyChange;

    private HashSet<Modifier> appliedModifiers;
    private PortalableObject portalableObject;
    #endregion

    #region Properties
    public float Gravity { get; } = -9.81f;

    public float AttackModifier
    {
        get => attackModifier;
        set => attackModifier = value;
    }

    public float DefenseModifier
    {
        get => defenseModifier;
        set => defenseModifier = value;
    }

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float MaxEnergy
    {
        get => maxEnergy;
        set => maxEnergy = value;
    }
    
    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    public float CurrentEnergy
    {
        get => currentEnergy;
        set => currentEnergy = value;
    }


    public HashSet<Modifier> AppliedModifiers => appliedModifiers;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float JumpHeight
    {
        get => jumpHeight;
        set => jumpHeight = value;
    }

    public float CurrentStunTime
    {
        get => currentStunTime;
        set => currentStunTime = value;
    }

    public bool IsStunned => currentStunTime > 0.0f;
    public bool IsAlive => currentHealth >= 0.0f;

    #endregion

    #region methods
    protected virtual void Awake()
    {
        appliedModifiers = new HashSet<Modifier>();
        portalableObject = GetComponent<PortalableObject>();
        portalableObject.HasTeleported += PortalableObjectOnHasTeleported;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        CharacterAudio = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    protected virtual void Update()
    {
        CurrentStunTime = Mathf.Max(currentStunTime -= Time.deltaTime, 0);

        //update speed param in animator
        if ((Animator || !string.IsNullOrWhiteSpace(SpeedParamaterName)) && Time.deltaTime > 0.0f)
        {
            var velocity = (transform.position - lastPosition) / Time.deltaTime;
            float speed = new Vector2(velocity.x, velocity.z).magnitude;
            var velocityNorm = speed / (Speed * 1.5f);
            //Debug.Log(velocityNorm); //1.5f for sprint
            Animator.SetFloat(SpeedParamaterName, velocityNorm, 0.1f, Time.deltaTime);
        }

        //travelSounds
        if (CharacterTravelAudioSource && FootstepSounds != null && FootstepSounds.Length > 0
            && onGround
            && Vector3.Distance(lastTravelSoundPlayed,transform.position) >= PlayTravelAudioDistance)
        {
            CharacterTravelAudioSource.PlayOneShot(FootstepSounds[UnityEngine.Random.Range(0,FootstepSounds.Length)]);
            lastTravelSoundPlayed = transform.position;
        }

        lastPosition = transform.position;
    }

    public virtual void PortalableObjectOnHasTeleported(Portal startPortal, Portal endPortal, Vector3 newposition, Quaternion newrotation)
    {
        // For character controller to update
        Physics.SyncTransforms();

        lastPosition = newposition;
    }

    protected virtual void OnDestroy()
    {
        portalableObject.HasTeleported -= PortalableObjectOnHasTeleported;
    }

    public void ApplyStatModifier(StatType type, float amount)
    {
        switch (type)
        {
            case StatType.ATTACK:
                AttackModifier += amount;
                break;
            case StatType.DEFENSE:
                DefenseModifier += amount;
                break;
            case StatType.HEALTH:
                MaxHealth += amount;
                OnMaxHealthChange?.Invoke();
                break;
            case StatType.ENERGY:
                MaxEnergy += amount;
            OnMaxEnergyChange?.Invoke();
                break;
            case StatType.SPEED:
                Speed += amount;
                break;
            case StatType.JUMP_HEIGHT:
                JumpHeight += amount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public IEnumerator ApplyStatsModifierOverPeriod(StatType type, float amount, float activePeriod)
    {
        ApplyStatModifier(type, amount);
        yield return new WaitForSeconds(activePeriod);
        ApplyStatModifier(type, -amount);
    }

    public float GetStatModifier(StatType type)
    {
        switch (type)
        {
            case StatType.ATTACK:
                return AttackModifier;
            case StatType.DEFENSE:
                return DefenseModifier;
            case StatType.HEALTH:
                return MaxHealth;
            case StatType.ENERGY:
                return MaxEnergy;
            case StatType.SPEED:
                return Speed;
            case StatType.JUMP_HEIGHT:
                return JumpHeight;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void TakeDamage(float attackDamage)
    {
        currentHealth = Mathf.Clamp((float)(currentHealth - attackDamage * Math.Pow(0.95, defenseModifier)), 0f, maxHealth);
        OnDamage?.Invoke();
        if (currentHealth == 0) OnDeath?.Invoke();

        //play hit sound
        if (CharacterAudio != null && HitSounds != null && HitSounds.Length > 0)
        {
            int clipIndex = UnityEngine.Random.Range(0, HitSounds.Length);
            CharacterAudio.clip = HitSounds[clipIndex];
            CharacterAudio.Play();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        OnHeal?.Invoke();
    }

    public void ReplenishEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
        OnReplenishEnergy?.Invoke();
    }
    
    public void LoseEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy - amount, 0f, maxEnergy);
        OnLoseEnergy?.Invoke();
    }
    #endregion

    #region IDataPersistance
    public virtual Dictionary<string, object> Save()
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        //save json to file
        var UUID = GetComponent<UUID>()?.ID;
        if (string.IsNullOrWhiteSpace(UUID))
        {
            Debug.LogError("CharacterBase doesn't have an UUID (Can't load data from json)");
            return dataDictionary;
        }

        //Load currently saved values
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, UUID);

        //save transform
        DataPersitanceHelpers.SaveTransform(ref dataDictionary, transform, "characterTransform");

        //save member vars
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "attackModifier", attackModifier);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "defenseModifier", defenseModifier);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "maxHealth", maxHealth);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "maxEnergy", maxEnergy);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "maxEnergy", maxEnergy);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "speed", speed);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "jumpHeight", jumpHeight);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "currentHealth", currentHealth);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "currentEnergy", currentEnergy);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "currentStunTime", currentStunTime);
        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "lastPosition", lastPosition);
        //todo save applied modifiers

        DataPersitanceHelpers.SaveDictionary(ref dataDictionary, UUID);

        return dataDictionary;
    }

    public virtual Dictionary<string, object> Load(bool destroyUnloaded = false)
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        var UUID = GetComponent<UUID>()?.ID;
        if (string.IsNullOrWhiteSpace(UUID))
        {
            Debug.LogError("CharacterBase doesn't have an UUID (Can't load data from json)");
            return dataDictionary;
        }

        //load dictionary
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, UUID);

        //load transform
        DataPersitanceHelpers.LoadTransform(ref dataDictionary, transform, "characterTransform");
        //load member vars
        attackModifier = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "attackModifier");
        defenseModifier = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "defenseModifier");
        maxHealth = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "maxHealth");
        maxEnergy = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "maxEnergy");
        speed = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "speed");
        jumpHeight = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "jumpHeight");
        currentHealth = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "currentHealth");
        currentEnergy = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "currentEnergy");
        currentStunTime = DataPersitanceHelpers.GetValueFromDictionary<float>(ref dataDictionary, "currentStunTime");
        lastPosition = DataPersitanceHelpers.GetValueFromDictionary<Vector3>(ref dataDictionary, "lastPosition");
        return dataDictionary;
    }

    #endregion
}