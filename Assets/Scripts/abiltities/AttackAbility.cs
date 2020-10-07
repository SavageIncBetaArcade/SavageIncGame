using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackAbility : BaseAbility
{
    public delegate void HitAction();
    public event HitAction OnHit;

    protected virtual void Hit()
    {
        OnHit?.Invoke();

    }
}
