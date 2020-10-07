using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    //Abilities that use mono behavior are all useable I.E (Weapons, Buffs) - Passive abilities work in a different way
    public ScriptableUseableAbility Ability;
    public string AnimationUseBoolName;
    public Animator UseAnimator;

    //TODO pass in the attackers character base
    public delegate void UseAction();
    public event UseAction OnUse;

    //TODO get character base on awake
    public CharacterBase CharacterBase;
    private float _lastUseTime;

    protected virtual void Awake()
    {
        if (Ability != null && Ability.AbilityPrefab != null)
        {
            Instantiate(Ability.AbilityPrefab, transform);
        }
        else
        {
            Debug.LogWarning("Base ability have a ability or an ability prefab");
        }

        if (CharacterBase == null)
            Debug.LogWarning("Base ability doesn't have a character base, please make sure to set it");

    }

    protected virtual void Update()
    {
        //TODO check if the current CharacterBase is the player, only attack on left click if player
        if (!OnCooldown() && Input.GetButtonDown("Fire1"))
        {
            ExecuteUse();
            _lastUseTime = Time.time;
        }
    }

    protected bool OnCooldown()
    {
        if (Ability.UseAnimationCooldown && UseAnimator != null && !string.IsNullOrWhiteSpace(AnimationUseBoolName))
        {
            return UseAnimator.GetBool(AnimationUseBoolName);
        }

        return !(Time.time >= _lastUseTime + Ability.Cooldown);
    }

    public abstract void Use();

    private void ExecuteUse()
    {
        OnUse?.Invoke();

        if (UseAnimator != null)
        {
            UseAnimator.SetBool(AnimationUseBoolName, true);
        }

        Use();
    }

}
