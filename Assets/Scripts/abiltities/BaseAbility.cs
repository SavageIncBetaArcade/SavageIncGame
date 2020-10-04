using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseAbility : MonoBehaviour
{
    public string AbilityName;
    public float Damage;
    public float Cooldown;
    public bool UseAnimationCooldown;
    public string AnimationAttackBoolName;
    public Animator AttackAnimator;

    //TODO pass in the attackers character base
    public delegate void AttackAction();
    public event AttackAction OnAttack;

    public delegate void HitAction();
    public event HitAction OnHit;

    //TODO get character base on awake
    //public CharacterBase CharacterBase;

    private float _lastAttackTime;

    protected virtual void Awake()
    {
        //if(CharacterBase == null)
        //    Debug.LogWarning($"{AbilityName} doesn't have a character base, please make sure to set it");
    }

    protected virtual void Update()
    {
        if (!OnCooldown() && Input.GetButtonDown("Fire1"))
        {
            Attack();
            _lastAttackTime = Time.time;
        }
    }

    protected bool OnCooldown()
    {
        if (UseAnimationCooldown && AttackAnimator != null && !string.IsNullOrWhiteSpace(AnimationAttackBoolName))
        {
            return AttackAnimator.GetBool(AnimationAttackBoolName);
        }

        return !(Time.time >= _lastAttackTime + Cooldown);
    }

    public virtual void Attack()
    {
        OnAttack?.Invoke();

        if (AttackAnimator != null)
        {
            AttackAnimator.SetBool(AnimationAttackBoolName, true);
        }
    }

    public virtual void Hit()
    {
        OnHit?.Invoke();
    }


}
