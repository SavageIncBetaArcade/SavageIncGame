using UnityEngine;

//ScriptableUseableAbility inheirts from Item so abilities can exist within the inventory 
[CreateAssetMenu(menuName = "Abilities/UseableAbility")]
public class ScriptableUseableAbility : Item
{
    public float EnergyCost;
    public float Cooldown;

    /// <summary>
    /// How Long is the ability is active
    /// activePeriod = 0 means the ability is only single use 
    /// </summary>
    public float ActivePeriod = 0.0f;

    /// <summary>
    /// How many times to use the ability whilst it is active
    /// Active period must be greater than 0 for this to take effect
    /// </summary>
    public float UseFrequency = 0.0f;

    [Range(0,1)]
    public float HitModifierApplyPercentage = 1.0f;
    public bool UseAnimationCooldown;
    public GameObject AbilityPrefab;
    public AbilityModifier[] StartingModifiers;
    public GameObject[] HitEffects;
}