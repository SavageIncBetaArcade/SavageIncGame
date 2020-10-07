using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class ScriptableAttackAbility : ScriptableAbility
{
    public abstract void Attack(CharacterBase targetCharacter);
}