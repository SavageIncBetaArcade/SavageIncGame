using UnityEngine;

//ScriptableUseableAbility inheirts from Item so abilities can exist within the inventory 
[CreateAssetMenu(menuName = "Abilities/UseableAbility")]
public class ScriptableUseableAbility : Item
{
    public float EnergyCost;
    public float Cooldown;
    [Range(0,1)]
    public float HitModifierApplyPercentage = 1.0f;
    public bool UseAnimationCooldown;
    public GameObject AbilityPrefab;
    public AbilityModifier[] StartingModifiers;
    public GameObject[] HitEffects;
    
    public override string GetInfoDescription()
    {
        return getEnergyCostString() + Description;
    }

    private string getEnergyCostString() { return EnergyCost != 0 ? $"Cost: {EnergyCost} energy \n" : ""; }
}