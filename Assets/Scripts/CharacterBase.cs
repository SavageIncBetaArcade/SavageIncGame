using System;
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
public class CharacterBase : MonoBehaviour, IDamageTaker
{
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

    [SerializeField]
    private AudioClip[] HitSounds;
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

    protected virtual void Awake()
    {
        appliedModifiers = new HashSet<Modifier>();
        portalableObject = GetComponent<PortalableObject>();
        portalableObject.HasTeleported += PortalableObjectOnHasTeleported;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        CharacterAudio = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        CurrentStunTime = Mathf.Max(currentStunTime -= Time.deltaTime, 0);
    }

    public virtual void PortalableObjectOnHasTeleported(Portal startPortal, Portal endPortal, Vector3 newposition, Quaternion newrotation)
    {
        // For character controller to update
        Physics.SyncTransforms();
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
}