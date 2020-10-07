using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public string AbilityName;
    public float Cooldown;
    public bool UseAnimationCooldown;
    public string AnimationUseBoolName;
    public Animator UseAnimator;

    //TODO pass in the attackers character base
    public delegate void AttackAction();
    public event AttackAction OnUse;

    //TODO get character base on awake
    //public CharacterBase CharacterBase;

    private float _lastUseTime;

    protected virtual void Awake()
    {
        //if(CharacterBase == null)
        //    Debug.LogWarning($"{AbilityName} doesn't have a character base, please make sure to set it");
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
        if (UseAnimationCooldown && UseAnimator != null && !string.IsNullOrWhiteSpace(AnimationUseBoolName))
        {
            return UseAnimator.GetBool(AnimationUseBoolName);
        }

        return !(Time.time >= _lastUseTime + Cooldown);
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
