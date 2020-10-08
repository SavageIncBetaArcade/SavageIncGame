using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// The DurationAbility is a BaseAbility that Applies functionality and Removes after a period of time
/// The DurationAbility is usefully for temporary buffs
/// </summary>
public abstract class DurationAbility : BaseAbility
{
    //How long should the effects be applied for? (In seconds)
    public float ActivePeriod = 10.0f;

    protected DurationAbility(UseableAbility useableAbility) : base(useableAbility)
    {
    }

    /// <summary>
    /// Applies functionality for a set period of time, then removes the functionality
    /// </summary>
    public override void Use()
    {
        //Call the method that would grant the character the passive ability
        Apply();

        //After the Active perios has ended call diminish to cancel the passive effect
        //If the active period is zero the passive ability would never diminish
        int miliseconds = Mathf.CeilToInt(ActivePeriod * 1000.0f);
        Task.Delay(miliseconds).ContinueWith(x => Remove());
    }

    public abstract void Apply();
    public abstract void Remove();

}
