using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackAbility : BaseAbility
{
    public delegate void HitAction(CharacterBase attackingCharacter, CharacterBase targetCharacter);
    public event HitAction OnHit;

    protected virtual void Hit(CharacterBase targetCharacter)
    {
        OnHit?.Invoke(CharacterBase,targetCharacter);

    }
}
