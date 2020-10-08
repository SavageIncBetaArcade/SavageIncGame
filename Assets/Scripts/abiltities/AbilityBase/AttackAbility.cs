using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackAbility : BaseAbility
{
    public delegate void HitAction(CharacterBase attackingCharacter, CharacterBase targetCharacter);
    public event HitAction OnHit;

    protected AttackAbility(UseableAbility useableAbility) : base(useableAbility)
    {

    }

    protected virtual void Hit(CharacterBase targetCharacter)
    {
        //TODO make calls to damage system

        OnHit?.Invoke(useableAbility.CharacterBase,targetCharacter);

    }

}
