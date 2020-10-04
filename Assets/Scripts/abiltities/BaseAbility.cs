using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseAbility : MonoBehaviour
{
    public string AbilityName;
    public float Damage;
    public float Cooldown;

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
        Cooldown = 0.0f;

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
        return !(Time.time >= _lastAttackTime + Cooldown);
    }

    public virtual void Attack()
    {
        OnAttack?.Invoke();
    }

    protected virtual void Hit()
    {
        OnHit?.Invoke();
    }


}
