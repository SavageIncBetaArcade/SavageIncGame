using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/ProjectileAbility")]
public class ScriptableProjectileAbility : ScriptableUseableAbility
{
    public Projectile ProjectileGameObject;
    public bool GravityAffected;
    public float InitialForce = 5.0f;
    public float Damage = 5.0f;
    public float Range = 25.0f;
}

