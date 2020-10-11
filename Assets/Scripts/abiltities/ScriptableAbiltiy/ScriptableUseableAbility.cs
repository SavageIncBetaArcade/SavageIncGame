using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/UseableAbility")]
public class ScriptableUseableAbility : ScriptableAbility
{
    public float Cooldown;
    public bool UseAnimationCooldown;
    public GameObject AbilityPrefab;
}