using UnityEngine;

//ScriptableUseableAbility inheirts from Item so abilities can exist within the inventory 
[CreateAssetMenu(menuName = "Abilities/UseableAbility")]
public class ScriptableUseableAbility : Item
{
    public float Cooldown;
    public bool UseAnimationCooldown;
    public GameObject AbilityPrefab;
    public AbilityModifier[] StartingModifiers;
    public GameObject[] HitEffects;
}